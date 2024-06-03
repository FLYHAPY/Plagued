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
    public bool saw;
    public float sawTimer;
    public float maxSawTimer;

    private float lastShootTime;

    private void Start()
    {
        hidingSpots = World.Instance.GetHidingSpots();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player2");
        door = GameObject.FindGameObjectWithTag("door");
        if(door != null)
        {
            door.GetComponent<Door2>().OnEnemySpawned();
        }
    }

    void Update()
    {
        /*if (!CanThePlayerSee() && saw == false)
        {
            agent.SetDestination(player.transform.position);
        }
        else if(saw == true)
        {
            Hide();
        }*/
        if (CanSeePlayer() && Time.time - lastShootTime >= shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        };

        agent.SetDestination(player.transform.position);

        if (saw)
        {
            sawTimer += Time.deltaTime;
        }

        if(sawTimer >= maxSawTimer)
        {
            saw = false;
            sawTimer = 0f;
        }
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * Vector3.Distance(transform.position, player.transform.position), Color.red);
        if (Physics.Raycast(transform.position, direction * 100, out hit, Mathf.Infinity, layerMask))
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
        Vector3 direction = cam.transform.forward;
        Debug.DrawRay(cam.transform.position, direction * 10000000, Color.blue);
        if (Physics.SphereCast(cam.transform.position, 1.5f, cam.transform.forward, out hit, 100000, layerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                saw = true;
                Debug.Log("true");
                return true;
            }
        }
        return false;
    }


    void Shoot()
    {
        // Calculate the predicted position based on player's current velocity
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        float distance = Vector3.Distance(firePoint.position, player.transform.position);
        float travelTime = distance / bulletSpeed;
        Vector3 predictedPosition = player.transform.position + playerVelocity * travelTime;

        // Instantiate the bullet and set its velocity towards the predicted position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (predictedPosition - firePoint.position).normalized;
        bullet.transform.LookAt(predictedPosition);  // Ensure the bullet is facing the predicted position
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        bullet.GetComponent<Bullets>().damage = damage;
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGeo = hidingSpots[0];

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDir = hidingSpots[i].transform.position - player.transform.position;
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
