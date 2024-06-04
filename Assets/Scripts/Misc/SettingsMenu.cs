using Unity.VisualScripting;
//Just for testing,my friends 
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//////////////////
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
public class SettingsMenu : MonoBehaviour
{
    public Button quitButton;
    public GameObject mainMenu;
    public GameObject options;
    Resolution[] resulutions;
    public Dropdown relsolutionDropdwon;
    public AudioMixer mixer;
    public Slider slider;
    public Slider sfxSlider;
    public GameObject background;
    public GameObject health;
    public GameObject healthBackground;
    public GameObject crossHair;
    public GameObject ak;
    public GameObject shot;
    public GameObject pistol;
    public GameObject bullet;
    public GameObject dash;

    void Start()
    {
        if (PlayerPrefs.HasKey("music") && mixer != null & slider != null && sfxSlider != null)
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
            SetSFXVolume();
        }

        resulutions = Screen.resolutions;
        relsolutionDropdwon.ClearOptions();


        List<string> options = new List<string>();

        int curentResulotionIndex = 0;
        for (int i = 0; i < resulutions.Length; i++)
        {
            string option = resulutions[i].width + "x" + resulutions[i].height;
            options.Add(option);

            if (resulutions[i].width == Screen.currentResolution.width && resulutions[i].height == Screen.currentResolution.height)
            {
                curentResulotionIndex = i;  
            }
        }

        relsolutionDropdwon.AddOptions(options);
        relsolutionDropdwon.value = curentResulotionIndex;
        relsolutionDropdwon.RefreshShownValue();
    }
    public void OptionsMenu()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        if (health)
        {
            background.SetActive(false);
            health.SetActive(false);
            healthBackground.SetActive(false);
            crossHair.SetActive(false);
            ak.SetActive(false);
            pistol.SetActive(false);
            shot.SetActive(false);
            bullet.SetActive(false);
            dash.SetActive(false);
        }
    }

    public void ReverseOptions()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);

        if (health)
        {
            background.SetActive(true);
            health.SetActive(true);
            healthBackground.SetActive(true);
            crossHair.SetActive(true);
            ak.SetActive(true);
            pistol.SetActive(true);
            shot.SetActive(true);
            bullet.SetActive(true);
            dash.SetActive(true);
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResulotion(int resultionIndex)
    {
        Resolution resulotion = Screen.resolutions[resultionIndex];
        Screen.SetResolution(resulotion.width, resulotion.height, Screen.fullScreen);
    }

    public void SetVolume()
    {
        float volume = slider.value;
        mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music", volume);
    }
    public void SetSFXVolume()
    {
        float volume1 = sfxSlider.value;
        mixer.SetFloat("sfxVolume", Mathf.Log10(volume1) * 20);
        PlayerPrefs.SetFloat("sfx", volume1);
    }
    public void LoadVolume()
    {
        slider.value = PlayerPrefs.GetFloat("music");
        sfxSlider.value = PlayerPrefs.GetFloat("sfx");
        SetVolume();
        SetSFXVolume();
    }
}