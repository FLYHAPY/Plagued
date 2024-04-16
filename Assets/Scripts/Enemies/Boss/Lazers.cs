using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Lazers : MonoBehaviour
{
    public float raycastDistance = 10f;
    public int damage;

    void Update()
    {
        // Cast a ray from the object's position in the forward direction
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            // If the ray hits something, draw the hit point and normal
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            if(hit.collider.tag == "Player2")
            {
                // Damage the player
                Debug.Log("yes");
                PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damage);
                
            }
        }
        else
        {
            // If the ray doesn't hit anything, draw the full raycast
            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.blue);
        }
    }
}
