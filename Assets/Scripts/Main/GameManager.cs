using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerBaseStats playerBaseStats;
    public PlayerStats playerStats = new PlayerStats();

    public int enemiesDestroyed;

    // Final values: base scaled by rank. Read these, not the raw numbers.
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
    }

    public void ResetRun()
    {
        playerStats.ResetStats();
        enemiesDestroyed = 0;
    }
}