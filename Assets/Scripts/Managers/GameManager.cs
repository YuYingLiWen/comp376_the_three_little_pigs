
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Note: Script MUST be attached to DDOL
    private static GameManager instance = null;

    public static GameManager GetInstance() => instance;

    [SerializeField]private SceneManager sceneManager = null;
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private InputSystem inputSystem = null;


    private void Awake()
    {
        if(!instance) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        inputSystem = gameObject.GetComponentInChildren<InputSystem>();
        if (!inputSystem) Debug.LogError("Missing Input System", gameObject);

        sceneManager = gameObject.GetComponent<SceneManager>();
        if (!sceneManager) Debug.LogError("Missing Scene Manager", gameObject);

        SubscribeInputEvents();
    }

    private void SubscribeInputEvents()
    {
        inputSystem.OnPause += ShowPauseMenu;

    }

    private void OnLevelSceneActivation()
    {
        // Grab the level manager
        var obj = GameObject.Find(LEVEL_MANAGER);
        if (obj) levelManager = GameObject.Find(LEVEL_MANAGER).GetComponent<LevelManager>();
        else Debug.LogError("Missing Level Manager", gameObject);

        LinkToLevelManagerEvents();

        inputSystem.enabled = true;
    }

    private void LinkToLevelManagerEvents()
    {
        levelManager.OnGameOver += ShowGameOverMenu;
        levelManager.OnGameWon += ShowGameWonMenu;
    }

    private void ShowGameOverMenu()
    {

    }

    private void ShowGameWonMenu()
    {

    }

    private void ShowMainMenu()
    {

    }

    private void ShowCredits()
    {

    }

    private void ShowPauseMenu(bool enable)
    {
        if(enable) // Show pause menu
        {

        }
        else // hide pause menu
        {

        }
    }

    private const string LEVEL_MANAGER = "LevelManager";
}
