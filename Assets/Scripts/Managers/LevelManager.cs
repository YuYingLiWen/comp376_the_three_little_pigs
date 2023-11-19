using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Goal: Checks win & lose conditions.
/// </summary>

public sealed class LevelManager : MonoBehaviour
{
    TMP_Text wood, stone, wave, timer;
    float timeLeft;

    [SerializeField] NightBehavior nightBehavior;
    [SerializeField] GameObject uiOverlay;
    Cave[] caves;

    //Cache

    // gather 1000 stones

    public bool debug = false;

    private void Awake()
    {
        gameManager = GameManager.GetInstance();
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

        TMP_Text[] resourceTexts = uiOverlay.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in resourceTexts)
        {
            if (text.name == "WoodCount")
            {
                wood = text;
            }
            else if (text.name == "StoneCount")
            {
                stone = text;
            }
            else if (text.name == "WaveNum")
            {
                wave = text;
            }
            else if (text.name == "Timer")
            {
                timer = text;
                timeLeft = float.Parse(timer.text);
            }
        }

    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timer.text = timeLeft.ToString("0");

    }

    private void OnDisable()
    {
        if (debug) return;

        OnGameOver -= gameManager.HandleGameOver;
        OnGameWon -= gameManager.HandleGameWon;

        inputSystem.OnMouseLeftClick -= HandleMouseLeftClick;
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

        // Update UI

    }

    public void AddStone(int amount)
    {
        if (amount < 0) return;
        resourceStone += amount;

        // Update UI

    }

    public void ConsumeWood(int amount)
    {
        if (amount - resourceWood < 0)
        {
            Debug.Log("Not enough woods.");
            return;
        }
        resourceWood -= amount;

        // Update UI
    }

    public void ConsumeStone(int amount)
    {
        if (amount -resourceStone < 0)
        {
            Debug.Log("Not enough stone.");
            return;
        }
        resourceStone -= amount;

        // Update UI
    }

    public void ConsumeResources(int wood, int stone)
    {
        ConsumeStone(stone);
        ConsumeWood(wood);
    }

    public void AddResources(int amt, string resourceType)
    {
        switch (resourceType)
        {
            case "wood":
                int temp = Int32.Parse(wood.text) + amt;
                wood.text = (Int32.Parse(wood.text) + amt).ToString();
                Debug.Log("temp: " + temp);
                Debug.Log("text: " + wood.text);
                break;
            case "stone":
                stone.text = (Int32.Parse(stone.text) + amt).ToString();
                break;
            default:
                break;
        }
    }


    public int Stone => resourceStone;
    public int Wood => resourceWood;

    IEnumerator DayNightRoutine()
    {
        bool night = true;

        float timeElapsed = 0.0f;
        while(true)
        {
            if(timeElapsed >= delayBetweenCycle)
            {
                if (night)
                {
                    nightBehavior.ToDay();
                    foreach(Cave cave in caves) cave.Spawn();
                }
                else
                {
                    nightBehavior.ToNight();
                    foreach (Cave cave in caves) cave.StopAllCoroutines();
                }

                night = !night;
                timeElapsed = 0.0f;
                waveNum += 1;
                
                // Update Wave Number & Time here

            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    int waveNum = 1;

    // Resources
    int resourceWood = 0, resourceStone = 0;

    [SerializeField] float delayBetweenCycle = 10.0f;

    GameManager gameManager;
    InputSystem inputSystem;

    public Action OnGameOver;
    public Action OnGameWon;
}
