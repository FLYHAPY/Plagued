using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
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
    public GameObject exitDoor;
    public GameObject entranceDoor;

    public bool triggered;

    public bool doorspawned = false;

    public bool isWaves;
    public bool wavesStarted;

    public Vector3 exitDoorSpawnPosition;
    public Vector3 entranceDoorSpawnPosition;

    public EnemySpawnInfo[] enemiesToSpawn; // Array of enemy prefabs and their counts

    public float timer;
    public float maxTimer;

    public TextMeshProUGUI text;

    private void Start()
    {
        timer = maxTimer;   
    }


    private void Update()
    {
        //normal
        if (triggered == true && doorspawned == false && !isWaves)
        {
            text.text = "Defeat all enemies";
            Instantiate(exitDoor, exitDoorSpawnPosition, Quaternion.Euler(new Vector3(0, -90, 0)));
            Instantiate(entranceDoor, entranceDoorSpawnPosition, Quaternion.Euler(new Vector3(0, -90, 0)));
            SpawnEnemies();
            doorspawned = true;
        }

        //waves
        if (triggered == true && doorspawned == false && isWaves)
        {
            Instantiate(exitDoor, exitDoorSpawnPosition, Quaternion.identity);
            Instantiate(entranceDoor, entranceDoorSpawnPosition, Quaternion.identity);
            doorspawned = true;
            StartCoroutine(SpawnEnemiesWithDelay());
            wavesStarted = true;
        }

        //cheats
        if(triggered)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                //normal
                if (!isWaves)
                {
                    GameObject[] allEnemiesLeft = GameObject.FindGameObjectsWithTag("EnemyToSpawn");
                    foreach (GameObject enemy in allEnemiesLeft)
                    {
                        enemy.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
                    }
                }
                if (isWaves)
                {
                    GameObject[] allBullets = GameObject.FindGameObjectsWithTag("bullet");
                    foreach (GameObject bullet in allBullets)
                    {
                        Destroy(bullet);
                    }

                    GameObject[] allEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in allEnemiesLeft)
                    {
                        timer = maxTimer;
                        StopAllCoroutines();
                        GameObject[] allEnemiesLeft2 = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach (GameObject enemy2 in allEnemiesLeft2)
                        {
                            Destroy(enemy2);
                        }
                        GameObject wavedoor = GameObject.FindGameObjectWithTag("WaveDoor");
                        Destroy(wavedoor);
                        wavesStarted = false;
                        text.text = "";
                    }
                }
            }
        }

        if(wavesStarted == true)
        {
            text.text = "Survive for:" + (int)timer;
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            timer = maxTimer;
            StopAllCoroutines();
            GameObject[] allEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemiesLeft)
            {
                Destroy(enemy);
            }
            GameObject wavedoor = GameObject.FindGameObjectWithTag("WaveDoor");
            Destroy(wavedoor);
            wavesStarted = false;
            text.text = "";

            GameObject[] allBullets = GameObject.FindGameObjectsWithTag("bullet");
            foreach (GameObject bullet in allBullets)
            {
                Destroy(bullet);
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player2"))
        {
            triggered = true;
            collision.gameObject.GetComponent<PlayerHealth>().respawnPoint = gameObject;
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
            yield return new WaitForSeconds(10f);
        }
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }
}