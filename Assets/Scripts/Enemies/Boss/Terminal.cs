using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    private bool collided;
    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        if (collided)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                text.text = "";
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player2"))
        {
            collided = true;
            text.text = "Press Q to destroy the terminal";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player2"))
        {
            collided = false;
            text.text = "";
        }
    }

}
