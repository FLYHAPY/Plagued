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
    public NavMeshAgent agent;
    public Transform firePoint;

    private float lastShootTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player2");
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        if (CanSeePlayer() && Time.time - lastShootTime >= shootCooldown)
        {
            ShootBurst();
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
            Vector3 direction = (player.transform.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            bullet.GetComponent<Bullets>().damage = damage;
            Destroy(bullet, 3f);

            yield return new WaitForSeconds(burstDelay);
        }
    }
}
