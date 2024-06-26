using UnityEngine;
using UnityEngine.AI;

public class DroneController : EnemyBase
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
        if (Time.time - lastShootTime >= shootCooldown)
        {
            if (CanSeePlayer())
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        Debug.DrawRay(transform.position, direction * Vector3.Distance(transform.position, player.transform.position), Color.red);

        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("Player2"))
            {
                return true;
            }
        }
        return false;
    }


    void Shoot()
    {
        soundEffect.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        bullet.transform.LookAt(player.transform.position);
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        bullet.GetComponent<Bullets>().damage = damage;

        //Debug.Log("Shooting(");

    }
}