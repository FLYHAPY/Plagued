using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player2")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TrapDeath();
        }
    }
}