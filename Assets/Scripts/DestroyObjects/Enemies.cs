using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Destroy all enemies in the scene with the "Enemy" tag
            DestroyEnemies();
        }
    }

    // Function to destroy all enemies in the scene
    void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
