using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player2"))
        {
            Debug.Log(collision.gameObject);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (!collision.gameObject.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }
    }
}
