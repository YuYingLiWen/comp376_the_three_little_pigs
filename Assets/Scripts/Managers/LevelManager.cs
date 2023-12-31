using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Goal: Checks win & lose conditions.
/// 
/// Objectives: 
/// 1) Construct a tier3 Town Center
/// 2) Defend for 3 waves against massive wave of enemies.
/// 
/// 
/// </summary>

public sealed class LevelManager : MonoBehaviour
{
    [SerializeField] NightBehavior nightBehavior;
    // Cave[] caves;
    public List<Cave> caves = new List<Cave>();

    private static LevelManager instance = null;
    public static LevelManager Instance => instance;

    OverlayUIController uiController;

    bool isPaused = false;

    // Check game is paused or not
    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        gameManager = GameManager.Instance;
        if (!gameManager) Debug.LogError("Missing Game Manager", gameObject);

       // if(!debug) inputSystem = gameManager.GetInputSystem();

        /*caves = FindObjectsByType<Cave>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        Debug.Log("Cave count: " + caves.Length);*/
        if(!uiController) uiController = FindFirstObjectByType<OverlayUIController>();
    }

    private void OnEnable()
    {
        OnStoneUpdate += uiController.HandleStoneUpdate;
        OnWoodUpdate += uiController.HandleWoodUpdate;
        OnNightCycleUpdate += uiController.HandleNightCycleUpdate;
        tc.OnHouseUpgrade += HandleOnHouseUpgrade;

        OnGameOver += gameManager.HandleGameOver;
        OnGameOver += HandleGameOver;

        OnGameWon += gameManager.HandleGameWon;

       // inputSystem.OnMouseLeftClick += HandleMouseLeftClick;
       // inputSystem.OnMapScroll += HandleMapScroll;
    }

    private void OnDisable()
    {
        OnStoneUpdate -= uiController.HandleStoneUpdate;
        OnWoodUpdate -= uiController.HandleWoodUpdate;
        OnNightCycleUpdate -= uiController.HandleNightCycleUpdate;
        tc.OnHouseUpgrade -= HandleOnHouseUpgrade;

        OnGameOver -= gameManager.HandleGameOver;
        OnGameOver -= HandleGameOver;

        OnGameWon -= gameManager.HandleGameWon;

        //inputSystem.OnMouseLeftClick -= HandleMouseLeftClick;
        //inputSystem.OnMapScroll -= HandleMapScroll;


        instance = null;
    }

    private void Start()
    {
        StartCoroutine(DayNightRoutine());

        OnWoodUpdate?.Invoke(resourceWood);
        OnStoneUpdate?.Invoke(resourceStone);
    }


    public void Pause()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    private void HandleMapScroll(Vector2 axis)
    {
    }

    private void HandleMouseLeftClick()
    {
        
    }

    public void AddWood(int amount)
    {
        if (amount < 0) return;
        resourceWood += amount;

        OnWoodUpdate?.Invoke(resourceWood);
    }

    public void ConsumeWood(int amount)
    {
        if (resourceWood - amount < 0)
        {
            Debug.Log("Not enough woods.");
            return;
        }
        resourceWood -= amount;

        OnWoodUpdate?.Invoke(resourceWood);
    }

    public void AddStone(int amount)
    {
        if (amount < 0) return;
        resourceStone += amount;

        OnStoneUpdate?.Invoke(resourceStone);
    }

    public void ConsumeStone(int amount)
    {
        if (resourceStone - amount < 0)
        {
            Debug.Log("Not enough stone.");
            return;
        }
        resourceStone -= amount;

        OnStoneUpdate?.Invoke(resourceStone);
    }

    public void ConsumeResources(int wood, int stone)
    {
        ConsumeStone(stone);
        ConsumeWood(wood);
    }

    // Get Player's stone count
    public int Stone => resourceStone;
    
    // Get Player's wood count
    public int Wood => resourceWood;

    //Checks if has enough resources
    public bool HasWood(int cost)
    {
        if (cost <= Wood) Debug.Log("Has engouh wood.");
        else Debug.Log("Not Enough wood");

        return cost <= Wood;
    }

    //Checks if has enough resources
    public bool HasStones(int cost)
    {
        if (cost <= Stone) Debug.Log("Has engouh stone.");
        else Debug.Log("Not Enough stones");

        return cost <= Stone;

    }


    public bool HasResources(int woodCost, int stoneCost) => HasWood(woodCost) && HasStones(stoneCost);

    IEnumerator DayNightRoutine()
    {
        float timeElapsed = 0.0f;
        while(true)
        {
            if(timeElapsed >= delayBetweenCycle)
            {
                if (isNightTime)
                {
                    nightBehavior.ToDay();
                    StopSpawningEnemies();
                }
                else
                {
                    nightBehavior.ToNight();
                    SpawnEnemies();
                }

                isNightTime = !isNightTime;
                timeElapsed = 0.0f;
                delayBetweenCycle *= 1.5f;

                if (atFinalObjective)
                {
                    currentWave += 1;

                    CheckWinCondition();
                }
            }

            OnNightCycleUpdate?.Invoke(timeElapsed / delayBetweenCycle);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    private void CheckWinCondition()
    {
        objectives.UpdateWavesLeft(wavesToSurive - currentWave);

        if(currentWave >= wavesToSurive)
        {
            Debug.Log("Game WWON!");

            objectives.OnObjective2Completed();

            OnGameWon?.Invoke();
            StopSpawningEnemies();
            KillAllEnemies();
        }
    }

    // Kill all remaining enemies on map.
    void KillAllEnemies()
    {
        var wolves = FindObjectsByType<Wolf>(FindObjectsSortMode.None);

        foreach (var wolf in wolves)
        {
            wolf.InstantDeath();
        }
    }

    void SpawnEnemies()
    {
        foreach (Cave cave in caves) cave.Spawn();
    }

    void StopSpawningEnemies()
    {
        foreach (Cave cave in caves) cave.StopAllCoroutines();
    }

    void HandleOnHouseUpgrade(int level)
    {
        objectives.UpdateHouseLevel(level);

        if(level == 3)
        {
            atFinalObjective = true;
            objectives.OnObjective1Completed();
        }
    }

    void HandleGameOver()
    {
        Time.timeScale = 0.0f;

        var t = FindObjectsByType<Towers>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var tower in t)
        {
            Destroy(tower.gameObject);
        }
    }

    bool atFinalObjective = false;
    readonly int wavesToSurive = 3;
    int currentWave = 0;

    // Resources
    int resourceWood = 0, resourceStone = 0;

    [SerializeField] float delayBetweenCycle = 10.0f;

    GameManager gameManager;
    InputSystem inputSystem;

    public Action OnGameOver;
    public Action OnGameWon;

    public Action<int> OnWoodUpdate, OnStoneUpdate;
    public Action<float> OnNightCycleUpdate;

    bool isNightTime = false;

    [SerializeField] Objectives objectives;
    [SerializeField] TownCenter tc;
}
