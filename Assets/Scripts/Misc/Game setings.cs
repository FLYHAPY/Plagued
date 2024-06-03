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
public class Gamesetings : MonoBehaviour
{
    public Button quitButton;
    public GameObject mainMenu;
    public GameObject options;
    Resolution[] resulutions;
    public Dropdown relsolutionDropdwon;
    public AudioMixer mixer;
    public Slider slider;
    public Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
