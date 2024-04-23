using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Door2 : MonoBehaviour
{
    public int numberofenemies;

    private void Awake()
    {
        numberofenemies = 0;
    }
    public void OnEnemySpawned()
    {
       print("Hello");
       numberofenemies += 1;
    }

    public void OnEnemyDestroyed()
    {
        print("Bye");
        numberofenemies -= 1;

        if (numberofenemies <= 0)
        {
            Destroy(gameObject);
        }
    }
}