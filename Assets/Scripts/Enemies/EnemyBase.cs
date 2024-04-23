using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public GameObject player;
    public float bulletcooldown;
    public int damage;
    public float health;
    public float speed;
    public Rigidbody rb;
    public GameObject door;


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(door != null)
        {
            door.GetComponent<Door2>().OnEnemyDestroyed();
        }
        Destroy(gameObject);
    }
}
