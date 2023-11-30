
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private SceneDirector sceneDirector;

    private void Awake()
    {
        sceneDirector = SceneDirector.GetInstance();
        if (!sceneDirector) Debug.LogError("Missing SceneDirector.", gameObject);

        Time.timeScale = 1.0f;
    }

    public void LoadLevel1()
    {
        Debug.Log("Clieck load ");
        sceneDirector.Load(SceneDirector.SceneNames.LEVEL1_SCENE, true);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
