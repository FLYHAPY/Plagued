using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage to apply
    public ParticleSystem flameParticles; // Reference to the particle system
    private bool playerInside = false; // Flag to track if the player is inside the trigger area
    private bool canDealDamage = false; // Flag to track if the FlameThrower can deal damage
    private float damageInterval = 1f; // Interval between each application of damage (1 second)
    private float lastDamageTime; // Time when the last damage was applied

    private void Start()
    {
        flameParticles = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(FireFlameThrower());
        lastDamageTime = -damageInterval; // Initialize the last damage time to ensure immediate damage application
    }

    private System.Collections.IEnumerator FireFlameThrower()
    {
        while (true)
        {
            // Activate the particle system
            flameParticles?.Play();
            canDealDamage = true;

            yield return new WaitForSeconds(2f); // Fire duration

            // Deactivate the particle system
            flameParticles?.Stop();
            canDealDamage = false;

            yield return new WaitForSeconds(5f); // Wait for 5 seconds before restarting the flamethrower
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            float currentTime = Time.time;
            float timeSinceLastDamage = currentTime - lastDamageTime;
            Debug.Log("Current Time: " + currentTime + ", Last Damage Time: " + lastDamageTime + ", Time Since Last Damage: " + timeSinceLastDamage);

            if (timeSinceLastDamage > damageInterval)
            {
                // Apply damage only if enough time has passed since the last damage
                if (canDealDamage)
                {
                    ApplyDamageToPlayer(other.gameObject);
                    lastDamageTime = currentTime; // Update the time of the last damage application
                }
            }
        }
    }



    private void ApplyDamageToPlayer(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TrapDeath();
            Debug.Log("Damage Applied: " + damageAmount + " to the player.");
        }
        else
        {
            Debug.LogWarning("Player does not have a PlayerHealth component!");
        }
    }
}