using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Goal: Checks win & lose conditions.
/// </summary>

public sealed class LevelManager : MonoBehaviour
{
    [SerializeField] private float cameraScrollSpeed = 1.0f;

    GameManager gameManager;
    InputSystem inputSystem;

    public Action OnGameOver;
    public Action OnGameWon;


    [SerializeField] NightBehavior nightBehavior;
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

    [SerializeField] float delayBetweenCycle = 10.0f;
}
