using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnMargin = 2f;   // how far past the screen corner

    private float timer;

    private void Update()
    {
        if (enemyPrefab == null || player == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * GetSpawnDistance();
        Vector3 spawnPosition = player.position + (Vector3)offset;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private float GetSpawnDistance()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // Distance from the screen centre to a corner, plus a margin.
        return Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight) + spawnMargin;
    }
}