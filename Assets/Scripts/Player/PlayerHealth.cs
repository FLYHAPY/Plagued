using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject respawnPoint;
    public Image health;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        float fillAmount = CalculateFillAmount(currentHealth);
        health.fillAmount = fillAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        Destroy(gameObject);
    }

    public void TrapDeath()
    {
        transform.position = respawnPoint.transform.position;
    }

    float CalculateFillAmount(float currentHealth)
    {
        return currentHealth / maxHealth;
    }
}
