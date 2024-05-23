using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionEffect;
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 spawnDirection = collision.contacts[0].normal;

        Instantiate(explosionEffect, collision.contacts[0].point, Quaternion.LookRotation(spawnDirection));

        Destroy(gameObject);
    }
}
