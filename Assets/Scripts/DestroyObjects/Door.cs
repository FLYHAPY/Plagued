using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Check if there are enemies in the scene
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                // Destroy the door if there are no enemies
                Destroy(gameObject);
            }
            else
            {
                // Optionally, provide feedback that the door cannot be destroyed
                Debug.Log("Cannot destroy the door. Enemies are present in the map.");
            }
        }
    }
}
