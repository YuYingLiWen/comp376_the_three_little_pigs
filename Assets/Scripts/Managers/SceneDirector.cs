using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles anything that has to do with loading, unloading scene and scene activation.
/// </summary>

public class SceneDirector : MonoBehaviour
{
    private GameManager gameManager;

    private AsyncOperation asyncOps = null;

    private Coroutine loadRoutine = null;

    void Start()
    {
        gameManager = GameManager.GetInstance();
        if (!gameManager) Debug.LogError("Missing Game Manager", gameObject);
    }

    public void Load(string sceneName)
    {
        if (loadRoutine == null) loadRoutine = StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void ActivateLoadedScene()
    {
        if (asyncOps == null) return;

        if(asyncOps.isDone)
        {
            asyncOps.allowSceneActivation = true;
            asyncOps.allowSceneActivation = false;
            asyncOps = null;
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        asyncOps = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOps.isDone)
        {
            yield return null;
        }
    }
}
