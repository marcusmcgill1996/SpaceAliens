using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// From Space Rouge's PauseManager.cs (class PauseMenu). File renamed to match the class.
public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    public TMP_Text runProgressText;
    public TMP_Text enemiesDestroyedText;
    public TMP_Text runDurationText;

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            // Something else already froze the game (level-up screen, death). Leave it alone.
            if (Time.timeScale == 0f) return;

            PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;

        runProgressText.text = "Level " + GameManager.Instance.playerLevel;
        enemiesDestroyedText.text = "Enemies Destroyed: " + GameManager.Instance.enemiesDestroyed;
        runDurationText.text = "Run Time: " + GameManager.Instance.GetFormattedRunDuration();

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Public so a Resume button can call it too.
    public void ResumeGame()
    {
        isPaused = false;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitRun()
    {
        GameManager.Instance.EndRun();
    }
}
