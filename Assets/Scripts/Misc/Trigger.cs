using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cameraHolder;
    void Start()
    {
        cameraHolder = GameObject.FindGameObjectWithTag("Camera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player2")
        {
            cameraHolder.GetComponent<MoveCamera>().inCutcene = true;
        }
    }
}
