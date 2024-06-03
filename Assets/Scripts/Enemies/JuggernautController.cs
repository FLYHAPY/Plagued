using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class JuggernautController : EnemyBase
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int numBullets = 12;
    public float burstDelay = 0.1f;
    public float shootCooldown = 2.5f;
    public float shootGrenadeCooldown;
    public NavMeshAgent agent;
    public Transform firePoint;

    private float lastShootTime;

    public GameObject grenadePrefab;

    public float grenadeSpeed = 10f;
    public float speedIncreaseFactor;

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
        if (CanSeePlayer() && Time.time - lastShootTime >= shootCooldown && health > 20)
        {
            ShootBurst();
            lastShootTime = Time.time;
        }

        if (health <= 20 && CanSeePlayer() && Time.time - lastShootTime >= shootGrenadeCooldown)
        {

            StopAllCoroutines();
            ShootGrenade();
            lastShootTime = Time.time;
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Player2"))
            {
                return true;
            }
        }
        return false;
    }

    void ShootBurst()
    {
        StartCoroutine(BurstRoutine());
    }

    IEnumerator BurstRoutine()
    {
        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector3 direction = (player.transform.position - firePoint.position).normalized;
            bullet.transform.LookAt(player.transform.position);  // Ensure the bullet is facing the player
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            bullet.GetComponent<Bullets>().damage = damage;

            Destroy(bullet, 3f);

            yield return new WaitForSeconds(burstDelay);
        }
    }

    public void ShootGrenade()
    {
        // Calculate direction from grenade launcher to player
        Vector3 direction = player.transform.position - firePoint.position;

        // Calculate the time it takes for the grenade to reach the player in the horizontal plane
        float timeToReachPlayer = direction.magnitude / grenadeSpeed;

        // Calculate the vertical displacement due to gravity during the time to reach the player
        float verticalDisplacement = 0.5f * -Physics.gravity.y * timeToReachPlayer * timeToReachPlayer;

        // Adjust the player's position considering the vertical displacement
        Vector3 adjustedPlayerPosition = player.transform.position + new Vector3(0f, verticalDisplacement, 0f);

        // Recalculate direction from grenade launcher to adjusted player position
        direction = adjustedPlayerPosition - firePoint.position;

        // Calculate the initial velocity required to reach the adjusted player position
        Vector3 initialVelocity = direction / timeToReachPlayer;

        // Create the grenade and apply initial velocity
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.velocity = initialVelocity;

        // Apply gravity to the grenade
        rb.useGravity = true;
    }
}
