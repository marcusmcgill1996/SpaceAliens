using System;
using System.IO;
using UnityEngine;

public static class RunHistoryStorage
{
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "run-history.json");

    public static RunHistoryData Load()
    {
        // Before the first save, there is no file.
        if (!File.Exists(SavePath))
        {
            return new RunHistoryData();
        }

        string json = File.ReadAllText(SavePath);
        RunHistoryData history =
            JsonUtility.FromJson<RunHistoryData>(json);

        // Refuse to overwrite a file we cannot understand.
        if (history == null || history.runs == null)
        {
            throw new InvalidDataException("The run history file is invalid.");
        }

        if (history.schemaVersion != 1)
        {
            throw new InvalidDataException(
                "This run history uses an unsupported save format.");
        }

        return history;
    }

    public static bool TrySave(RunRecord record, out string message)
    {
        if (record == null || string.IsNullOrEmpty(record.runId))
        {
            message = "There is no completed run to save.";
            return false;
        }

        try
        {
            RunHistoryData history = Load();

            foreach (RunRecord savedRun in history.runs)
            {
                if (savedRun != null && savedRun.runId == record.runId)
                {
                    message = "This run is already saved.";
                    return false;
                }
            }

            // Newest saved runs appear first.
            history.runs.Insert(0, record);

            string json = JsonUtility.ToJson(history, true);

            Directory.CreateDirectory(Application.persistentDataPath);

            // Finish writing a temporary file before replacing the history.
            string temporaryPath = SavePath + ".tmp";
            File.WriteAllText(temporaryPath, json);

            if (File.Exists(SavePath))
            {
                File.Replace(temporaryPath, SavePath, null);
            }
            else
            {
                File.Move(temporaryPath, SavePath);
            }

            message = "Run saved!";
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            message = "Could not save the run. Check the Console.";
            return false;
        }
    }
}