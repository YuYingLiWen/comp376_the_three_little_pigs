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
    //Cache

    // gather 1000 stones

    public bool debug = false;

    private void Awake()
    {
        gameManager = GameManager.GetInstance();
        if (!gameManager) Debug.LogError("Missing Game Manager", gameObject);

        if(!debug) inputSystem = gameManager.GetInputSystem();
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
    }

    public void AddStone(int amount)
    {
        if (amount < 0) return;
        resourceStone += amount;
    }

    public void ConsumeWood(int amount)
    {
        if (amount - resourceWood < 0)
        {
            Debug.Log("Not enough woods.");
            return;
        }
        resourceWood -= amount;
    }

    public void ConsumeStone(int amount)
    {
        if (amount -resourceStone < 0)
        {
            Debug.Log("Not enough stone.");
            return;
        }
        resourceStone -= amount;
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


    IEnumerator DayNightRoutine()
    {
        bool toggle = false;

        while(true)
        {
            if(toggle)
            {
                nightBehavior.ToDay();
            }else
            {
                nightBehavior.ToNight();
            }

            toggle = !toggle;

            yield return new WaitForSeconds(delayBetweenCycle);
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
