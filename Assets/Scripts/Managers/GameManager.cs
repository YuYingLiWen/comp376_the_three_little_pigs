
using System;
using UnityEngine;

/// <summary>
/// Goal: Manages the overall game system.
/// </summary>

public class GameManager : MonoBehaviour
{
    // Note: Script MUST be attached to DDOL
    private static GameManager instance = null;
    public static GameManager GetInstance() => instance;

    [SerializeField] private SceneDirector sceneManager = null;
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private InputSystem inputSystem = null;

    // Game Pause
    private enum GameState { PLAY, PAUSED, MAIN_MENU, CREDITS };
    private GameState currentGameState = GameState.MAIN_MENU;

    public Action OnGamePause;

    private void Awake()
    {
        if(!instance) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        OnGamePause += HandleGamePause;

    }

    private void Start()
    {
        inputSystem = gameObject.GetComponentInChildren<InputSystem>();
        if (!inputSystem) Debug.LogError("Missing Input System", gameObject);

        sceneManager = gameObject.GetComponent<SceneDirector>();
        if (!sceneManager) Debug.LogError("Missing Scene Manager", gameObject);
    }

    private void OnDisable()
    {
        OnGamePause -= HandleGamePause;
    }

    public void HandleLevelSceneActivation()
    {
        currentGameState = GameState.PLAY;

        inputSystem.enabled = true;
    }

    public void HandleCreditsSceneActivation()
    {
        currentGameState = GameState.CREDITS;

    }

    public void HandleMainMenuSceneActivation()
    {
        currentGameState = GameState.MAIN_MENU;

    }

    public void HandleGameOver()
    {

    }

    public void HandleGameWon()
    {

    }

    private void HandleGamePause()
    {
        if      (currentGameState == GameState.PLAY)   currentGameState = GameState.PAUSED;
        else if (currentGameState == GameState.PAUSED) currentGameState = GameState.PLAY;


        if (currentGameState == GameState.PAUSED) // Show pause menu
        {

        }
        else // hide pause menu
        {

        }
    }

    public InputSystem GetInputSystem() => inputSystem;

    private const string LEVEL_MANAGER = "LevelManager";
}
