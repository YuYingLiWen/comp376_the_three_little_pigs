
using TMPro;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] TMP_Text winLoseText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winloseMenu;

    private void OnEnable()
    {
        LevelManager.Instance.OnGameOver += DisplayLose;
        LevelManager.Instance.OnGameWon += DisplayWin;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnGameOver -= DisplayLose;
        LevelManager.Instance.OnGameWon -= DisplayWin;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (LevelManager.Instance.IsPaused)
                Unpause();
            else
                Pause();
        }
    }

    public void DisplayWin()
    {
        winloseMenu.SetActive(true);
        winLoseText.text = "You've survived!";
    }

    public void DisplayLose()
    {
        winloseMenu.SetActive(true);
        winLoseText.text = "Only pig carcasses remained.";
    }

    public void Unpause()
    {
        LevelManager.Instance.Unpause();

        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        LevelManager.Instance.Pause();

        pauseMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneDirector.GetInstance().Load(SceneDirector.SceneNames.MAIN_MENU_SCENE, true);
    }
}
