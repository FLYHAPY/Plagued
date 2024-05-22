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

    void Start()
    {
        if (PlayerPrefs.HasKey("music") && mixer != null & slider != null && sfxSlider != null)
        {
            //LoadVolume();
        }
        else
        {
            //SetVolume();
            //SetSFXVolume();
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
    }

    public void ReverseOptions()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
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

    /*public void SetVolume()
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
    }*/
}