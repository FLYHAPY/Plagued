using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ShotgunnerController : EnemyBase
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float shootCooldown = 1f;
    public NavMeshAgent agent;
    public Transform firePoint;

    private float lastShootTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player2");
        door = GameObject.FindGameObjectWithTag("door");
        if (door != null)
        {
            door.GetComponent<Door2>().OnEnemySpawned();
        }
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        if (CanSeePlayer() && Time.time - lastShootTime >= shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        };
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * Vector3.Distance(transform.position, player.transform.position), Color.red);
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Player2"))
            {
                return true;
            }
        }
        return false;
    }


    void Shoot()
    {
        int numBullets = 6; // Number of bullets to shoot
        float spreadAngle = 30f; // Spread angle for the shotgun effect

        // Calculate the direction towards the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < numBullets; i++)
        {
            // Calculate a random angle within the spread angle
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);

            // Rotate the direction vector by the random angle around the up axis
            Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
            Vector3 spreadDirection = rotation * direction;

            // Instantiate a bullet and set its velocity
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Temporarily disable collision detection for the bullet
            if(bullet != null)
            {
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>(), true);

                bullet.GetComponent<Rigidbody>().velocity = spreadDirection * bulletSpeed;
                bullet.GetComponent<Bullets>().damage = damage;

                // Destroy the bullet after some time
                Destroy(bullet, 3f);

                // Re-enable collision detection after a short delay
                StartCoroutine(EnableCollisionAfterDelay(bullet.GetComponent<Collider>()));
            }
        }

        Debug.Log("Shooting shot");
    }

    IEnumerator EnableCollisionAfterDelay(Collider bulletCollider)
    {
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed
        Physics.IgnoreCollision(bulletCollider, GetComponent<Collider>(), false);
    }



}
