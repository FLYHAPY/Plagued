using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoosHealth : MonoBehaviour
{
    public int health = 100; // Initial health value
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
                health -= 100; // Reduce health by 10 when an object is deleted
                deletedObjects.Add(obj); // Mark the object as deleted
            }
        }

        if (health <= 0)
        {
            // Handle game over or object destruction logic here
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }
}
