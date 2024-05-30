using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waipoints : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] waypoints;
    int CurrentWp;

    float threshold = 3;
    float rotationSpeed = 1;
    float speed = 5;
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, waypoints[CurrentWp].transform.position) < threshold)
            CurrentWp++;

        if (CurrentWp >= waypoints.Length)
            CurrentWp = 0;

        Vector3 dir = waypoints[CurrentWp].transform.position - this.transform.position;
        dir.y = 0;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookDir, rotationSpeed * Time.deltaTime);
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
