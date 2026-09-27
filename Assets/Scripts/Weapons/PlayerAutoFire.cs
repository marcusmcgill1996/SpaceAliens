using UnityEngine;

public class PlayerAutoFire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private float cooldown;

    private void Update()
    {
        if (bulletPrefab == null) return;

        cooldown -= Time.deltaTime;
        if (cooldown > 0f) return;

        Transform target = FindNearestEnemy(GameManager.Instance.Range);
        if (target == null) return;

        Vector2 direction = ((Vector2)(target.position - transform.position)).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Launch(direction, GameManager.Instance.ProjectileSpeed, GameManager.Instance.Damage);

        cooldown = 1f / GameManager.Instance.FireRate;
    }

    private Transform FindNearestEnemy(float range)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float nearestDistance = range;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}