using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    private int damage;
    private bool hasHit;

    public void Launch(Vector2 direction, float speed, int damage)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;

        hasHit = true;
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}