using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public Vector3[] spawnPoints; // Array of Vector2 spawn points for this enemy type
    [HideInInspector]
    public bool isSpawned; // Flag to track if enemies for this type have been spawned
}

public class SpawnTrigger : MonoBehaviour
{
    public GameObject TypeOfDoor;

    public bool triggered;

    private bool doorspawned = false;

    public bool isWaves;
    public bool wavesStarted;

    public Vector3 doorspawnposition;

    public EnemySpawnInfo[] enemiesToSpawn; // Array of enemy prefabs and their counts

    public float timer;
    public float maxTimer;


    private void Update()
    {
        //normal
        if (triggered == true && doorspawned == false && !isWaves)
        {
            Instantiate(TypeOfDoor, doorspawnposition, Quaternion.identity);
            SpawnEnemies();
            doorspawned = true;
        }

        //waves
        if (triggered == true && doorspawned == false && isWaves)
        {
            Instantiate(TypeOfDoor, doorspawnposition, Quaternion.identity);
            doorspawned = true;
            StartCoroutine(SpawnEnemiesWithDelay());
            wavesStarted = true;
        }

        if(wavesStarted == true)
        {
            timer += Time.deltaTime;
        }
        if(timer >= maxTimer)
        {
            timer = 0;
            StopAllCoroutines();
            GameObject[] allEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemiesLeft)
            {
                Destroy(enemy);
            }
            GameObject wavedoor = GameObject.FindGameObjectWithTag("WaveDoor");
            Destroy(wavedoor);
            wavesStarted = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player2"))
        {
            triggered = true;
        }
    }
    //normal spawn
    void SpawnEnemies()
    {
        foreach (var enemyInfo in enemiesToSpawn)
        {
            foreach (Vector3 spawnPoint in enemyInfo.spawnPoints)
            {
                Instantiate(enemyInfo.enemyPrefab, spawnPoint, Quaternion.identity);
            }
        }
    }
    //waves spawner
    IEnumerator SpawnEnemiesWithDelay()
    {
        while (true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(2f);
        }
    }
}