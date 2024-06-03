using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveCamera : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPosition;
    public GameObject cutceneCamera;
    public GameObject normalCamera;
    public GameObject cameraPos;
    public GameObject boss;

    [Header("Cutcene")]
    public bool inCutcene;
    public Transform[] targetPositions; // Array to store multiple target positions
    public float speed = 5f; // Adjust the speed in the Inspector
    public int currentIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (!inCutcene)
        {
            transform.position = cameraPosition.position;
            normalCamera.SetActive(true);
            cutceneCamera.SetActive(false);
            StopAllCoroutines();
        }
        else
        {
            normalCamera.SetActive(false);
            cutceneCamera.SetActive(true);
            transform.LookAt(boss.transform);

            if(currentIndex < targetPositions.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPositions[currentIndex].position, speed * Time.deltaTime);
                if (transform.position == targetPositions[currentIndex].position)
                {
                    // Move to the next target position in the array
                    currentIndex = (currentIndex + 1);
                }
            }
            
            
            if (currentIndex >= targetPositions.Length) 
            {
                inCutcene = false;
                currentIndex = 0;
            }
        }
    }
}   
