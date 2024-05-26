using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Weapon_Switcher : MonoBehaviour
{
    public Image assultRifle;
    public Image shotgun;
    public Image pistol;
    public GameManager gameManager;
    public TextMeshProUGUI bulletText;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    // Update is called once per frame
    public int currentWeapon = 0;

    void Update()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (gameManager.state != State2.Paused)
        {
            if (scrollWheelInput > 0f)
            {
                SwitchWeapon(1);
            }
            else if (scrollWheelInput < 0f)
            {
                SwitchWeapon(-1);
            }
        }

        switch (currentWeapon)
        {
            case 0:
                shotgun.gameObject.SetActive(true);
                assultRifle.gameObject.SetActive(false);
                pistol.gameObject.SetActive(false);
                bulletText.text = transform.GetChild(0).gameObject.GetComponent<Guns>().bulletsLeft.ToString() + "/" + transform.GetChild(0).gameObject.GetComponent<Guns>().magazineSize.ToString();
                break;
            case 1:
                shotgun.gameObject.SetActive(false);
                assultRifle.gameObject.SetActive(false);
                pistol.gameObject.SetActive(true);
                bulletText.text = transform.GetChild(1).gameObject.GetComponent<Guns>().bulletsLeft.ToString() + "/" + transform.GetChild(1).gameObject.GetComponent<Guns>().magazineSize.ToString();
                break;
            case 2:
                shotgun.gameObject.SetActive(false);
                assultRifle.gameObject.SetActive(true);
                pistol.gameObject.SetActive(false);
                bulletText.text = transform.GetChild(2).gameObject.GetComponent<Guns>().bulletsLeft.ToString() + "/" + transform.GetChild(2).gameObject.GetComponent<Guns>().magazineSize.ToString();
                break;
        }
    }

    void SwitchWeapon(int direction)
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
        
        currentWeapon += direction;
        
        if (currentWeapon < 0)
        {
            currentWeapon = transform.childCount - 1;
        }
        else if (currentWeapon >= transform.childCount)
        {
            currentWeapon = 0;
        }
        
        transform.GetChild(currentWeapon).gameObject.SetActive(true);
    }
}