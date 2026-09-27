using UnityEngine;
using TMPro;

// Shows stat RANKS (how many times each stat was upgraded). Used on Game Over,
// and later on the level-up screen.
public class PlayerStatsDisplay : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text speedText;
    public TMP_Text fireRateText;
    public TMP_Text projectileSpeedText;
    public TMP_Text rangeText;          // optional: leave empty until the panel has a slot
    public TMP_Text pickupRadiusText;   // optional

    public bool refreshOnEnable = true;
    private PlayerStats displayedStats;

    void OnEnable()
    {
        if (refreshOnEnable) RefreshStats();
    }

    public void RefreshStats()
    {
        PlayerStats stats = displayedStats;
        if (stats == null && GameManager.Instance != null)
            stats = GameManager.Instance.playerStats;

        if (stats != null) RefreshStats(stats);
    }

    public void RefreshStats(PlayerStats stats)
    {
        if (stats == null) return;
        displayedStats = stats;

        SetText(healthText, "Max Health", stats.maxHealth);
        SetText(damageText, "Damage", stats.damage);
        SetText(speedText, "Move Speed", stats.moveSpeed);
        SetText(fireRateText, "Fire Rate", stats.fireRate);
        SetText(projectileSpeedText, "Projectile Speed", stats.projectileSpeed);
        SetText(rangeText, "Range", stats.range);
        SetText(pickupRadiusText, "Pickup Radius", stats.pickupRadius);
    }

    private static void SetText(TMP_Text text, string label, int rank)
    {
        if (text != null) text.text = label + ": " + rank;
    }
}
