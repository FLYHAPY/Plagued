using UnityEngine;
using UnityEngine.AI;

public class JuggernautController : EnemyBase
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int numBullets = 12;
    public float shootCooldown = 2.5f;
    public NavMeshAgent agent;

    private float lastShootTime;

    void Update()
    {
        agent.SetDestination(player.transform.position);
        if (CanSeePlayer() && Time.time - lastShootTime >= shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out hit))
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
        Vector3 direction = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector3 spreadDirection = Quaternion.Euler(0, Random.Range(-15f, 15f), 0) * direction;
            bullet.GetComponent<Rigidbody>().velocity = spreadDirection * bulletSpeed;
            bullet.GetComponent<Bullets>().damage = damage;
            Destroy(bullet, 3f);
        }
    }
}
