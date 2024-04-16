using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnGroup
{
    public string groupName; // Name of the group
    public List<EnemySpawnInfo> enemySpawnInfos; // List of enemy prefabs and spawn positions for this group
    public List<GameObject> doorPrefabs; // List of door prefabs for this group
}

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
}

public class SpawningEnemies : MonoBehaviour
{
    public List<EnemySpawnGroup> enemySpawnGroups; // List of enemy spawn groups
    public int maxEnemies = 10; // Maximum number of enemies
    private int enemyCount = 0; // Current number of enemies

    void Start()
    {
        StartCoroutine(SpawnEnemySequence());
    }

    IEnumerator SpawnEnemySequence()
    {
        foreach (EnemySpawnGroup group in enemySpawnGroups)
        {
            foreach (EnemySpawnInfo spawnInfo in group.enemySpawnInfos)
            {
                // Spawn enemy for this group
                SpawnEnemy(spawnInfo.enemyPrefab, spawnInfo.spawnPosition);

                // Increase enemy count
                enemyCount++;

                yield return new WaitForSeconds(0.1f);
            }

            // Spawn doors for this group
            foreach (GameObject doorPrefab in group.doorPrefabs)
            {
                SpawnDoor(doorPrefab, Vector3.zero); // Adjust spawn position as needed
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnDoor(GameObject doorPrefab, Vector3 spawnPosition)
    {
        // Adjust spawn position for door if needed
        // Example: Vector3 doorSpawnPosition = spawnPosition + new Vector3(0, 1, 0);
        Instantiate(doorPrefab, spawnPosition, Quaternion.identity);
    }
}
