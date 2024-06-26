using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawning : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player2"))
        {
            other.gameObject.GetComponent<PlayerHealth>().respawnPoint = gameObject;
        }
    }
}
