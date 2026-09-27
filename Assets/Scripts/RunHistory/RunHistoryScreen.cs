using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RunHistoryScreen : MonoBehaviour
{
    public Transform content;
    public RunHistoryRowUI rowPrefab;
    public TMP_Text statusText;

    private readonly List<RunHistoryRowUI> rows = new List<RunHistoryRowUI>();

    private enum SortColumn { Level, RunTime, Enemies, Date }
    private SortColumn? activeSort;
    private bool descending;

    public void BackToStart()
    {
        SceneManager.LoadScene(GameManager.StartMenuSceneName);
    }

    void Start()
    {
        if (content == null || rowPrefab == null)
        {
            Debug.LogError("Assign Content and Row Prefab on RunHistoryScreen.", this);
            return;
        }

        try
        {
            RunHistoryData history = RunHistoryStorage.Load();
            int shown = 0;

            foreach (RunRecord record in history.runs)
            {
                if (record == null) continue;

                RunHistoryRowUI row = Instantiate(rowPrefab, content, false);
                row.DisplayRun(record);
                rows.Add(row);
                shown++;
            }

            if (statusText != null)
            {
                statusText.text = shown == 0
                    ? "No saved runs yet. Choose Save Run on Game Over to keep a run."
                    : "Select a run to view its full results.";
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            if (statusText != null)
                statusText.text = "Could not load run history. Your save file has not been changed.";
        }
    }

    public void SortByLevel()
    {
        SortRows(SortColumn.Level, "Level", "highest", "lowest");
    }

    public void SortByRunTime()
    {
        SortRows(SortColumn.RunTime, "Run Time", "longest", "shortest");
    }

    public void SortByEnemies()
    {
        SortRows(SortColumn.Enemies, "Enemies", "most", "fewest");
    }

    public void SortByDate()
    {
        SortRows(SortColumn.Date, "Date & Time", "newest", "oldest");
    }

    private void SortRows(SortColumn column, string label, string high, string low)
    {
        if (rows.Count == 0) return;

        // A different column starts highest-first; repeating it reverses direction.
        descending = activeSort != column || !descending;
        activeSort = column;

        rows.Sort((a, b) => descending
            ? CompareRecords(b.Record, a.Record, column)
            : CompareRecords(a.Record, b.Record, column));

        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].transform.SetSiblingIndex(i);
        }

        if (statusText != null)
        {
            string first = descending ? high : low;
            string next = descending ? low : high;
            statusText.text = $"{label}: {first} first. Double-click again for {next} first.";
        }
    }

    private static int CompareRecords(RunRecord a, RunRecord b, SortColumn column)
    {
        int comparison;
        switch (column)
        {
            case SortColumn.Level:
                comparison = a.levelReached.CompareTo(b.levelReached);
                if (comparison == 0)
                    comparison = a.durationSeconds.CompareTo(b.durationSeconds);
                break;
            case SortColumn.RunTime:
                comparison = a.durationSeconds.CompareTo(b.durationSeconds);
                break;
            case SortColumn.Enemies:
                comparison = a.enemiesDestroyed.CompareTo(b.enemiesDestroyed);
                break;
            default:
                comparison = GetEndedAt(a).CompareTo(GetEndedAt(b));
                break;
        }

        // Consistent ordering when two runs have the same selected value.
        if (comparison == 0)
            comparison = GetEndedAt(a).CompareTo(GetEndedAt(b));
        if (comparison == 0)
            comparison = string.CompareOrdinal(a.runId, b.runId);
        return comparison;
    }

    private static DateTimeOffset GetEndedAt(RunRecord record)
    {
        return DateTimeOffset.TryParse(record.endedAtUtc,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset endedAt)
            ? endedAt
            : DateTimeOffset.MinValue;
    }
}
