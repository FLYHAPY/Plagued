using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDealDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player2")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(30);
        }
    }
}
