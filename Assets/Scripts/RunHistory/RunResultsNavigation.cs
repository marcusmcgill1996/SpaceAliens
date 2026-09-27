using UnityEngine;
using UnityEngine.SceneManagement;

// Carries only the selected record between scenes; never changes the active run.
public static class RunResultsNavigation
{
    private static RunRecord selectedRun;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSelection()
    {
        selectedRun = null;
    }

    public static void OpenSavedRun(RunRecord record)
    {
        if (record == null) return;
        selectedRun = record;
        SceneManager.LoadScene("GameOver");
    }

    public static RunRecord TakeSelectedRun()
    {
        RunRecord record = selectedRun;
        selectedRun = null;
        return record;
    }
}
