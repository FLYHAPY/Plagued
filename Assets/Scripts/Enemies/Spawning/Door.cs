using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Door2 : MonoBehaviour
{
    public int numberofenemies;
    public GameObject text;

    private void Awake()
    {
        numberofenemies = 0;
        text = GameObject.FindGameObjectWithTag("objective");
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
            text.GetComponent<TextMeshProUGUI>().text = "";
            Destroy(gameObject);
        }
    }
}