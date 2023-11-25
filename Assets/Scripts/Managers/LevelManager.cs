using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Goal: Checks win & lose conditions.
/// </summary>

public sealed class LevelManager : MonoBehaviour
{
    [SerializeField] NightBehavior nightBehavior;
    [SerializeField] GameObject uiOverlay;
    Cave[] caves;

    private static LevelManager instance = null;
    public static LevelManager Instance => instance;

    public bool debug = false;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);

        gameManager = GameManager.Instance;
        if (!gameManager) Debug.LogError("Missing Game Manager", gameObject);

        if(!debug) inputSystem = gameManager.GetInputSystem();

        caves = FindObjectsByType<Cave>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        Debug.Log(caves.Length);
    }

    private void OnEnable()
    {
        if (debug) return;

        OnGameOver += gameManager.HandleGameOver;
        OnGameWon += gameManager.HandleGameWon;

        inputSystem.OnMouseLeftClick += HandleMouseLeftClick;
        inputSystem.OnMapScroll += HandleMapScroll;
    }

    private void Start()
    {
        StartCoroutine(DayNightRoutine());

        UpdateStoneUI();
        UpdateWoodUI();
    }
    private void OnDisable()
    {
        if (debug) return;

        OnGameOver -= gameManager.HandleGameOver;
        OnGameWon -= gameManager.HandleGameWon;

        inputSystem.OnMouseLeftClick -= HandleMouseLeftClick;

        instance = null;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }

    private void GameWon()
    {
        OnGameWon?.Invoke();
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

        UpdateWoodUI();
    }

    public void ConsumeWood(int amount)
    {
        if (amount - resourceWood < 0)
        {
            Debug.Log("Not enough woods.");
            return;
        }
        resourceWood -= amount;

        UpdateWoodUI();
    }

    public void AddStone(int amount)
    {
        if (amount < 0) return;
        resourceStone += amount;

        UpdateStoneUI();
    }    

    public void ConsumeStone(int amount)
    {
        if (amount - resourceStone < 0)
        {
            Debug.Log("Not enough stone.");
            return;
        }
        resourceStone -= amount;

        UpdateStoneUI();
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
                    foreach(Cave cave in caves) cave.Spawn();
                }
                else
                {
                    nightBehavior.ToNight();
                    foreach (Cave cave in caves) cave.StopAllCoroutines();
                }

                isNightTime = !isNightTime;
                timeElapsed = 0.0f;
                waveNum += 1;
            }

            waveImg.fillAmount = timeElapsed / delayBetweenCycle;

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    void UpdateStoneUI()
    {
        stoneCountUI.text = resourceStone.ToString();
    }

    void UpdateWoodUI()
    {
        woodCountUI.text = resourceWood.ToString();
    }

    int waveNum = 1;

    // Resources
    int resourceWood = 0, resourceStone = 0;

    [SerializeField] float delayBetweenCycle = 10.0f;

    GameManager gameManager;
    InputSystem inputSystem;

    public Action OnGameOver;
    public Action OnGameWon;

    [SerializeField] TMP_Text woodCountUI;
    [SerializeField] TMP_Text stoneCountUI;
    [SerializeField] TMP_Text waveText;
    [SerializeField] Image waveImg;

    bool isNightTime = true;
}
