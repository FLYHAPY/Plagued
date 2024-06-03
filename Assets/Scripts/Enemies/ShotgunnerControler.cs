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
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        soundEffect.Play();
        // Loop to create each bullet
        for (int i = 0; i < numBullets; i++)
        {
            // Calculate the spread for each bullet
            float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector3 spreadDirection = Quaternion.Euler(0, angle, 0) * direction;

            // Instantiate and shoot the bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(spreadDirection));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            bullet.GetComponent<Bullets>().damage = damage;
            rb.velocity = spreadDirection * bulletSpeed;
        }
    }

    IEnumerator EnableCollisionAfterDelay(Collider bulletCollider)
    {
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed
        Physics.IgnoreCollision(bulletCollider, GetComponent<Collider>(), false);
    }



}
