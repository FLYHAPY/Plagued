using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cameraHolder;
    public GameObject bossUI;
    void Start()
    {
        cameraHolder = GameObject.FindGameObjectWithTag("Camera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player2"))
        {
            cameraHolder.GetComponent<MoveCamera>().inCutcene = true;
            other.gameObject.GetComponent<PlayerHealth>().respawnPoint = gameObject;
            bossUI.gameObject.SetActive(true);
        }
    }
}
