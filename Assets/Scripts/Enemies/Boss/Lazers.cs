using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Lazers : MonoBehaviour
{
    public float raycastDistance = 10f;
    public int damage;
    public bool isInCoolDown;
    public float timer;
    public float maxTimer;
    public LineRenderer lineRenderer;
    public Vector3 endPoint;

    void Update()
    {
        // Cast a ray from the object's position in the forward direction
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            // If the ray hits something, draw the hit point and normal
            //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            //Debug.DrawRay(hit.point, hit.normal, Color.green);
            endPoint = hit.point;
            if (hit.collider.gameObject.CompareTag("Player2") && !isInCoolDown)
            {
                // Damage the player
                PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damage);
                isInCoolDown = true;
            }
        }
        else
        {
            isInCoolDown = false;
            timer = 0;
            // If the ray doesn't hit anything, draw the full raycast
            //Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.blue);
            endPoint = transform.position + transform.forward * raycastDistance;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);

        if (isInCoolDown)
        {
            timer += Time.deltaTime;
        }

        if(timer >= maxTimer) 
        {
            isInCoolDown = false;
            timer = 0;
        }
    }
}
