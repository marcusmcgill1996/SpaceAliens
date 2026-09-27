using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;

public class GameOver : MonoBehaviour
{
    public TMP_Text runProgressText;
    public TMP_Text enemiesDestroyedText;
    public TMP_Text runDurationText;

    public Button saveRunButton;
    public TMP_Text saveRunButtonText;
    public TMP_Text titleText;
    public TMP_Text backButtonText;
    public TMP_Text runDateText;
    public PlayerStatsDisplay statsDisplay;

    private bool runSaved = false;
    private bool viewingHistory;
    private RunRecord displayedRun;

    void Start()
    {
        displayedRun = RunResultsNavigation.TakeSelectedRun();
        viewingHistory = displayedRun != null;

        if (!viewingHistory)
        {
            if (GameManager.Instance == null)
            {
                titleText.text = "No Run Selected";
                saveRunButton.gameObject.SetActive(false);
                Debug.LogWarning("Open Game Over by ending a run or selecting a saved run.");
                return;
            }

            GameManager.Instance.CaptureCompletedRun();
            displayedRun = GameManager.Instance.CompletedRun;
        }

        titleText.text = viewingHistory ? "Run Details" : "Game Over";
        backButtonText.text = viewingHistory ? "Back to History" : "Start Menu";
        saveRunButton.gameObject.SetActive(!viewingHistory);

        runProgressText.text = "Level " + displayedRun.levelReached;

        enemiesDestroyedText.text = "Enemies Destroyed: " + displayedRun.enemiesDestroyed;

        long seconds = (long)displayedRun.durationSeconds;
        runDurationText.text = $"Run Time: {seconds / 60:00}:{seconds % 60:00}";

        runDateText.gameObject.SetActive(viewingHistory);
        if (viewingHistory)
        {
            runDateText.text = DateTimeOffset.TryParse(displayedRun.endedAtUtc,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset endedAt)
                ? endedAt.ToLocalTime().ToString("dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture)
                : "Unknown date";
        }

        // Always display from the record, so live results and history look identical.
        statsDisplay.RefreshStats(displayedRun.CreateDisplayStats());
    }

    public void SaveRun()
    {
        if (runSaved || viewingHistory || displayedRun == null) return;

        bool success = RunHistoryStorage.TrySave(displayedRun, out string message);

        if (!success)
        {
            Debug.LogWarning(message);
            return;
        }

        runSaved = true;
        saveRunButtonText.text = "Saved";
        saveRunButton.interactable = false;

        Debug.Log(message);
    }

    public void BackToStart()
    {
        SceneManager.LoadScene(viewingHistory
            ? GameManager.RunHistorySceneName
            : GameManager.StartMenuSceneName);
    }
}
