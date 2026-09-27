using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Scene names live in one place so no other script has to spell them.
    public const string StartMenuSceneName = "StartMenu";
    public const string GameSceneName = "Game";
    public const string GameOverSceneName = "GameOver";
    public const string RunHistorySceneName = "RunHistory";

    public static GameManager Instance;

    public PlayerBaseStats playerBaseStats;
    public PlayerStats playerStats = new PlayerStats();

    public int enemiesDestroyed;
    public int playerLevel = 1;

    // The finished run, captured once when the run ends. Game Over reads this.
    public RunRecord CompletedRun { get; private set; }

    // ---------- Run timer (from Space Rouge) ----------
    // Uses Unity's scaled clock, so it stops while Time.timeScale is 0 (pause, level-up).
    private double completedCombatSeconds;
    private double combatStartedAt;
    private bool isTimingCombat;

    public double RunDurationSeconds => completedCombatSeconds +
        (isTimingCombat ? Time.timeAsDouble - combatStartedAt : 0d);

    public string GetFormattedRunDuration()
    {
        long totalSeconds = (long)RunDurationSeconds;
        return $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
    }

    // ---------- Final stat values: base scaled by rank ----------
    public int MaxHealth => Mathf.RoundToInt(PlayerStats.Scale(
        playerBaseStats.maxHealth, playerStats.maxHealth, playerBaseStats.maxHealthGrowth));

    public int Damage => Mathf.RoundToInt(PlayerStats.Scale(
        playerBaseStats.damage, playerStats.damage, playerBaseStats.damageGrowth));

    public float MoveSpeed => PlayerStats.Scale(
        playerBaseStats.moveSpeed, playerStats.moveSpeed, playerBaseStats.moveSpeedGrowth);

    public float FireRate => PlayerStats.Scale(
        playerBaseStats.fireRate, playerStats.fireRate, playerBaseStats.fireRateGrowth);

    public float ProjectileSpeed => PlayerStats.Scale(
        playerBaseStats.projectileSpeed, playerStats.projectileSpeed, playerBaseStats.projectileSpeedGrowth);

    public float Range => PlayerStats.Scale(
        playerBaseStats.range, playerStats.range, playerBaseStats.rangeGrowth);

    public float PickupRadius => PlayerStats.Scale(
        playerBaseStats.pickupRadius, playerStats.pickupRadius, playerBaseStats.pickupRadiusGrowth);

    // ---------- Lifecycle ----------
    private void Awake()
    {
        // If another GameManager already exists, delete this one.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // The run clock starts whenever the Game scene loads.
        if (scene.name == GameSceneName)
        {
            BeginCombatTimer();
        }
    }

    // ---------- Run control ----------
    public void ResetRun()
    {
        playerStats.ResetStats();
        enemiesDestroyed = 0;
        playerLevel = 1;

        completedCombatSeconds = 0d;
        combatStartedAt = 0d;
        isTimingCombat = false;

        CompletedRun = null;
    }

    // Called when the player dies or quits from the pause menu.
    public void EndRun()
    {
        CaptureCompletedRun();

        // Never carry a frozen clock into the next scene.
        Time.timeScale = 1f;

        SceneManager.LoadScene(GameOverSceneName);
    }

    public void BeginCombatTimer()
    {
        if (isTimingCombat) return;

        combatStartedAt = Time.timeAsDouble;
        isTimingCombat = true;
    }

    public void StopCombatTimer()
    {
        if (!isTimingCombat) return;

        completedCombatSeconds += Time.timeAsDouble - combatStartedAt;
        isTimingCombat = false;
    }

    public void CaptureCompletedRun()
    {
        // Keep the original snapshot if it was already captured.
        if (CompletedRun != null) return;

        StopCombatTimer();

        CompletedRun = new RunRecord
        {
            runId = System.Guid.NewGuid().ToString(),
            endedAtUtc = System.DateTime.UtcNow.ToString("O"),

            levelReached = playerLevel,
            durationSeconds = RunDurationSeconds,
            enemiesDestroyed = this.enemiesDestroyed,

            maxHealth = playerStats.maxHealth,
            damage = playerStats.damage,
            moveSpeed = playerStats.moveSpeed,
            fireRate = playerStats.fireRate,
            projectileSpeed = playerStats.projectileSpeed,
            range = playerStats.range,
            pickupRadius = playerStats.pickupRadius
        };
    }
}
