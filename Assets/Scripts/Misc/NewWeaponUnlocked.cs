using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewWeaponUnlocked : MonoBehaviour
{
    public TextMeshProUGUI text;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "New Weapon Unlocked";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 3)
        {
            text.text = "";
            Destroy(gameObject);
        }
    }
}
