using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to apply to the player

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamage(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FlameThrower"))
        {
            Destroy(other.gameObject);
        }
    }

    private void ApplyDamage(GameObject player)
    {
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); // Assuming the player has a PlayerHealth component
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("Player does not have a PlayerHealth component!");
            }
        }
    }
}
