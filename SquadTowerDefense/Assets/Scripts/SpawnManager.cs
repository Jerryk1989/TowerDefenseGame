using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnTarget; // The target location for spawning
    public float spawnInterval = 2.5f; // Time between spawns

    private float timer = 0f;

    public GameObject enemyPrefab;

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn
        if (timer >= spawnInterval)
        {
            // Spawn an enemy
            SpawnEnemy();

            // Reset the timer
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        // Get an enemy object from the object pool
        GameObject enemy = ObjectPool.Instance.GetObject(enemyPrefab);

        // Check if the enemy is not null and the spawn target is not null
        if (enemy != null && spawnTarget != null)
        {
            Debug.LogWarning($"Spawning enemy at {enemy.transform.position}");
            enemy.SetActive(true);
        }
    }
}