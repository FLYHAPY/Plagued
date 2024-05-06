using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosHealth : MonoBehaviour
{
    public float health = 100; // Initial health value
    public float maxHealth = 100;
    public Image healthImage;
    public List<GameObject> objectsToDelete; // List of objects to delete
    private HashSet<GameObject> deletedObjects; // Track deleted objects

    void Start()
    {
        deletedObjects = new HashSet<GameObject>();
    }

    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 20f, 0, Space.Self);

        foreach (GameObject obj in objectsToDelete)
        {
            if (obj == null && !deletedObjects.Contains(obj))
            {
                health -= 50; // Reduce health by 10 when an object is deleted
                deletedObjects.Add(obj); // Mark the object as deleted
                float fillAmount = CalculateFillAmount(health);
                healthImage.fillAmount = fillAmount;
            }
        }

        if (health <= 0)
        {
            // Handle game over or object destruction logic here
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }

    float CalculateFillAmount(float currentHealth)
    {
        return currentHealth / maxHealth;
    }
}
