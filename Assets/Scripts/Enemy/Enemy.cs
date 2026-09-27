using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int contactDamage = 10;

    private int currentHealth;

    public int ContactDamage => contactDamage;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.enemiesDestroyed++;
        Destroy(gameObject);
    }
}