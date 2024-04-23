using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Guns : MonoBehaviour
{
    [Header("Gun Stats")]
    public int damage;
    public int magazineSize;
    public int bulletsPerTap;
    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float reloadTime;
    public float timeBetweenShots;  
    public float force;
    public float yes;
    private int bulletsLeft, bulletsShot;

    [Header("Bools")]
    public bool allowButtonHold, shooting, readyToShoot, reloading, rocketjumping;

    [Header("References")]
    public Camera cam;
    public Transform attackPoint;
    public RaycastHit hit;
    public LayerMask whatIsEnemy;
    public GameObject bulletHole;
    public Rigidbody rb;
    public PlayerMovement pm;


    private void Update()
    {
        StartCoroutine(MyInput());
    }
    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    IEnumerator MyInput()
    {
        if(allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting= Input.GetKeyDown(KeyCode.Mouse0);
        }

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0) 
        {
            bulletsShot = bulletsPerTap;
            for (int i = 0; i < bulletsPerTap; i++)
            {
                Shoot();
                yield return new WaitForSeconds(yes);
            }
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        Vector3 direction = cam.transform.forward + new Vector3(x, y, z);
        //Debug.Log("hit");

        if (Physics.Raycast(cam.transform.position, direction, out hit, range, whatIsEnemy))
        {
            Debug.DrawRay(cam.transform.position, direction * range, Color.red);
            //Debug.Log("Hit");
            if (hit.collider.CompareTag("Enemy"))
            {
                //call the function of the enemy
                Debug.Log("hit");
                //hit.collider.gameObject.GetComponent<DroneController>().TakeDamage(damage);
                //hit.collider.gameObject.GetComponent<OfficerController>().TakeDamage(damage);
                hit.collider.gameObject.SendMessage("TakeDamage", damage);

            }
            else if (hit.collider.CompareTag("cable"))
            {
                Destroy(hit.collider.gameObject);
            }
        }

        Instantiate(bulletHole, hit.point, Quaternion.Euler(0, 180, 0));

        bulletsLeft--;
        bulletsShot--;
        //Invoke("ResetShot", timeBetweenShooting);
        Invoke("ResetState", 0.5f);

        if(rocketjumping)
        {
            //Debug.Log(-cam.transform.forward.normalized);
            rb.AddForce(-cam.transform.forward.normalized * force, ForceMode.Impulse);
            pm.rocketJumping = true;
        }

        if (bulletsShot == 0)
        {
            Invoke("ResetShot", timeBetweenShooting);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void ResetState()
    {
        pm.rocketJumping = false;
    }
}
