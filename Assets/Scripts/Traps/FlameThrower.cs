using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to apply
    public ParticleSystem flameParticles; // Reference to the particle system
    private bool playerInside = false; // Flag to track if the player is inside the trigger area
    public bool canDealDamage = false; // Flag to track if the player is inside the trigger area

    private void Start()
    {
        flameParticles = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(FireFlameThrower());
    }

    private System.Collections.IEnumerator FireFlameThrower()
    {
        while (true)
        {
            // Activate the particle system
            flameParticles?.Play();
            canDealDamage = true;

            yield return new WaitForSeconds(2f); // Fire duration

            // Stop the particle system
            flameParticles?.Stop();
            canDealDamage = false;

            yield return new WaitForSeconds(5f); // Wait for 5 seconds before restarting the flamethrower
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            playerInside = true;
            // Apply damage only if the particle system is playing
            if (flameParticles != null && flameParticles.isPlaying && canDealDamage)
            {
                ApplyDamageToPlayer(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            playerInside = false;
        }
    }

    private void ApplyDamageToPlayer(GameObject player)
    {
        if (flameParticles != null && flameParticles.isPlaying)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Damage Applied: " + damageAmount + " to the player.");
            }
            else
            {
                Debug.LogWarning("Player does not have a PlayerHealth component!");
            }
        }
        else
        {
            Debug.LogWarning("FlameThrower particle system is not playing!");
        }
    }
}
