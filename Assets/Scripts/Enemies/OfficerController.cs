using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class OfficerController : EnemyBase
{
    public GameObject cam;
    public GameObject[] hidingSpots;
    public GameObject target;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float shootCooldown = 1f;
    public NavMeshAgent agent;
    public Transform firePoint;

    private float lastShootTime;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Camera");
        player = GameObject.FindGameObjectWithTag("Player2");
        door = GameObject.FindGameObjectWithTag("door");
        if(door != null)
        {
            door.GetComponent<Door2>().OnEnemySpawned();
        }
    }

    void Update()
    {
        if (!CanThePlayerSee())
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            Hide();
        }
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

    bool CanThePlayerSee()
    {
        RaycastHit hit;
        // Debug.DrawRay(transform.position, direction * Vector3.Distance(transform.position, player.transform.position), Color.red);
        Vector3 direction = cam.transform.forward;
        if (Physics.Raycast(player.transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }


    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Debug.Log(direction);
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        bullet.GetComponent<Bullets>().damage = damage;

        Debug.Log("Shooting");

        Destroy(bullet, 3f); 
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGeo = hidingSpots[0];

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDir = hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidepos = hidingSpots[i].transform.position + hideDir.normalized * 5;
            if (Vector3.Distance(transform.position, hidepos) < dist)
            {
                chosenSpot = hidepos;
                chosenDir = hideDir;
                chosenGeo = hidingSpots[i];
                dist = Vector3.Distance(transform.position, hidepos);
            }
        }

        Collider hideCol = chosenGeo.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 100.0f;
        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized * 5);
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }
}
