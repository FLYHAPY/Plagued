using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject respawnPoint;
    public Image health;
    public bool cheatOn;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !cheatOn)
        {
            cheatOn = true;
        }else if(Input.GetKeyDown(KeyCode.P) && cheatOn)
        {
            cheatOn= false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!cheatOn)
        {
            currentHealth -= damage;

            float fillAmount = CalculateFillAmount(currentHealth);
            health.fillAmount = fillAmount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        transform.position = respawnPoint.transform.position;
        currentHealth = maxHealth;
        float fillAmount = CalculateFillAmount(currentHealth);
        health.fillAmount = fillAmount;
        respawnPoint.GetComponent<SpawnTrigger>().triggered = false;
        respawnPoint.GetComponent<SpawnTrigger>().doorspawned = false;

        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("bullet");
        foreach (GameObject bullet in allBullets)
        {
            Destroy(bullet);
        }

        if (!respawnPoint.GetComponent<SpawnTrigger>().isWaves)
        {
            GameObject[] allEnemiesLeft = GameObject.FindGameObjectsWithTag("EnemyToSpawn");
            foreach (GameObject enemy in allEnemiesLeft)
            {
                enemy.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
        }else if (respawnPoint.GetComponent<SpawnTrigger>().isWaves)
        {
            GameObject[] allEnemiesLeft = GameObject.FindGameObjectsWithTag("EnemyToSpawn");
            foreach (GameObject enemy in allEnemiesLeft)
            {
                respawnPoint.GetComponent<SpawnTrigger>().timer = respawnPoint.GetComponent<SpawnTrigger>().maxTimer;
                respawnPoint.GetComponent<SpawnTrigger>().StopCoroutines();
                GameObject[] allEnemiesLeft2 = GameObject.FindGameObjectsWithTag("EnemyToSpawn");
                foreach (GameObject enemy2 in allEnemiesLeft2)
                {
                    Destroy(enemy2);
                }
                GameObject wavedoor = GameObject.FindGameObjectWithTag("WaveDoor");
                Destroy(wavedoor);
                respawnPoint.GetComponent<SpawnTrigger>().wavesStarted = false;
                respawnPoint.GetComponent<SpawnTrigger>().text.text = "";
            }
        }
    }

    public void TrapDeath()
    {
        if (cheatOn) 
        {
            transform.position = respawnPoint.transform.position;
        }
    }

    float CalculateFillAmount(float currentHealth)
    {
        return currentHealth / maxHealth;
    }
}
