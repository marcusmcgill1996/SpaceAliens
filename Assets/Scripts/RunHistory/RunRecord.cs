using System;

[Serializable]
public class RunRecord
{
    // Identifies this run so it cannot be saved twice.
    public string runId;

    // Stored in UTC; converted to local time when displayed.
    public string endedAtUtc;

    // Progress reached.
    public int levelReached;

    // Run results.
    public double durationSeconds;
    public int enemiesDestroyed;

    // Final stat ranks.
    public int maxHealth;
    public int damage;
    public int moveSpeed;
    public int fireRate;
    public int projectileSpeed;
    public int range;
    public int pickupRadius;

    // A separate copy for the reusable stat display.
    public PlayerStats CreateDisplayStats()
    {
        return new PlayerStats
        {
            maxHealth = this.maxHealth,
            damage = this.damage,
            moveSpeed = this.moveSpeed,
            fireRate = this.fireRate,
            projectileSpeed = this.projectileSpeed,
            range = this.range,
            pickupRadius = this.pickupRadius
        };
    }
}
