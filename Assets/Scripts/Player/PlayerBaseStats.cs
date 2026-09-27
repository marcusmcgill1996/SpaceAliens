using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "Game Data/Player Base Stats")]
public class PlayerBaseStats : ScriptableObject
{
    [Header("Base values (rank 1)")]
    public int maxHealth = 100;
    public int damage = 10;
    public float moveSpeed = 5f;
    public float fireRate = 1f;          // shots per second
    public float projectileSpeed = 5f;
    public float range = 6f;
    public float pickupRadius = 1.5f;

    [Header("Growth per rank above 1 (0.1 = +10%)")]
    public float maxHealthGrowth = 0.1f;
    public float damageGrowth = 0.1f;
    public float moveSpeedGrowth = 0.1f;
    public float fireRateGrowth = 0.1f;
    public float projectileSpeedGrowth = 0.1f;
    public float rangeGrowth = 0.1f;
    public float pickupRadiusGrowth = 0.1f;
}