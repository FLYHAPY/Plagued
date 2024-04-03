using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningEnemies : MonoBehaviour
{
    public GameObject Enemy;
    public List<Vector3> spawnPositions; // List to hold the spawn positions
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        // Ensure there are available spawn positions and enemy count is less than desired
        while (spawnPositions.Count > 0 && enemyCount < 10)
        {
            // Get the next spawn position from the list
            Vector3 spawnPosition = spawnPositions[0];
            spawnPositions.RemoveAt(0); // Remove the used spawn position

            // Instantiate enemy at the specified position
            Instantiate(Enemy, spawnPosition, Quaternion.identity);

            // Increase enemy count
            enemyCount++;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
