using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RunHistoryRowUI : MonoBehaviour
{
    // FormerlySerializedAs keeps the Inspector link from when this field was called waveText.
    [FormerlySerializedAs("waveText")]
    public TMP_Text levelText;
    public TMP_Text runTimeText;
    public TMP_Text enemiesText;
    public TMP_Text dateTimeText;

    public RunRecord Record { get; private set; }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OpenRun);
    }

    void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(OpenRun);
    }

    private void OpenRun()
    {
        RunResultsNavigation.OpenSavedRun(Record);
    }

    public void DisplayRun(RunRecord record)
    {
        Record = record;

        levelText.text = record.levelReached.ToString();

        long totalSeconds = (long)record.durationSeconds;
        runTimeText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";

        enemiesText.text = record.enemiesDestroyed.ToString();

        if (DateTimeOffset.TryParse(
            record.endedAtUtc,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTimeOffset endedAt))
        {
            dateTimeText.text = endedAt.ToLocalTime().ToString(
                "dd-MM-yyyy hh:mm tt",
                CultureInfo.InvariantCulture
            );
        }
        else
        {
            dateTimeText.text = "Unknown date";
        }
    }
}
