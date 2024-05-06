using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportations : MonoBehaviour
{
    public GameObject otherPortal;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player2")
        {
            collision.gameObject.transform.position = otherPortal.transform.position;
        }
    }
}
