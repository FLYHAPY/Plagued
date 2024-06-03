using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Enter the name of the specific scene where you want to stop the music
        string sceneToStopMusic = "MainMenu";

        if (currentScene.name == sceneToStopMusic)
        {
            // Add logic to stop the music here
            // For example, you can use GetComponent to get the AudioSource and then call Stop()
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
    }
}