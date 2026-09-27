[System.Serializable]
public class PlayerStats
{
    // Ranks: how many times each stat has been upgraded this run. 1 = base.
    public int maxHealth = 1;
    public int damage = 1;
    public int moveSpeed = 1;
    public int fireRate = 1;
    public int projectileSpeed = 1;
    public int range = 1;
    public int pickupRadius = 1;

    public void ResetStats()
    {
        maxHealth = 1;
        damage = 1;
        moveSpeed = 1;
        fireRate = 1;
        projectileSpeed = 1;
        range = 1;
        pickupRadius = 1;
    }

    // base × (1 + growth × (rank − 1))
    public static float Scale(float baseValue, int rank, float growthPerRank)
    {
        return baseValue * (1f + growthPerRank * (rank - 1));
    }
}