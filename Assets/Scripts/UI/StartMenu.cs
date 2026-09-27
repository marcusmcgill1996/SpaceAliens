using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OpenRunHistory()
    {
        SceneManager.LoadScene(GameManager.RunHistorySceneName);
    }

    public void StartGame()
    {
        GameManager.Instance.ResetRun();

        SceneManager.LoadScene(GameManager.GameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does nothing in the Editor, so stop Play Mode instead.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
