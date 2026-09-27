using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invulnerabilityTime = 0.5f;

    private int currentHealth;
    private float invulnerableUntil;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => GameManager.Instance.MaxHealth;

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.ContactDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        // Still invulnerable from the last hit.
        if (Time.time < invulnerableUntil) return;

        currentHealth -= amount;
        invulnerableUntil = Time.time + invulnerabilityTime;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.EndRun();
    }
}
