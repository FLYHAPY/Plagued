using System.Collections;
using UnityEngine;

public class SniperController : EnemyBase
{
    public GameObject cam;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootCooldown = 5f;
    public Transform firePoint;
    [SerializeField] private float lastShootTime;
    public float rotationSpeed;
    public float jumpForce = 10f;
    public bool grounded;
    public float rotationDuration;
    public bool shoot;
    public float height = 2;
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player2");
        door = GameObject.FindGameObjectWithTag("door");
        rb = GetComponent<Rigidbody>();
        if (door != null)
        {
            door.GetComponent<Door2>().OnEnemySpawned();
        }
    }

    void Update()
    {
        if (grounded) 
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }


        if (CanSeePlayer() && !shoot && grounded)
        {
            shoot = true;
            Shoot();
            shoot = true;
        }
        grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.1f, layerMask);

        if (shoot)
        {
            lastShootTime += Time.deltaTime;
        }

        if(lastShootTime >= shootCooldown)
        {
            shoot = false;
            lastShootTime = 0;
        }
    }

    private void FixedUpdate()
    {
        if (grounded && CanThePlayerSee())
        {
            shoot = true;
            lastShootTime = 0;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            StartCoroutine(Rotate360Degrees());
            JumpAction();
        }
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
        soundEffect.Play();
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

        Destroy(bullet, 3f);
    }


    void JumpAction()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
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
                Debug.Log("true");
                return true;
            }
        }
        return false;
    }

    IEnumerator Rotate360Degrees()
    {
        float elapsedTime = 0.0f;
        float initialYRotation = transform.eulerAngles.y;
        float targetYRotation = initialYRotation + 360.0f;

        while (elapsedTime < rotationDuration)
        {
            float currentYRotation = Mathf.Lerp(initialYRotation, targetYRotation, elapsedTime / rotationDuration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentYRotation, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Shoot();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetYRotation, transform.eulerAngles.z);
    }
}

