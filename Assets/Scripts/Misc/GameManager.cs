using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum State
{
    Paused,
    Playing
}

public class GameManager : MonoBehaviour
{
    public GameObject panel;
    public GameObject resumeButton;
    public GameObject pauseButton;
    public GameObject sureButton;
    public GameObject noButton;
    public State state;
    public GameObject paused;
    public GameObject areYouSure;
    public DialogueSystem dialogueSystem;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && state == State.Playing && !dialogueSystem.inDialogue)
        {
            Activate();
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && state == State.Paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Deactivate();
        }
    }

    public void Deactivate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Playing;
        paused.SetActive(false);
        panel.SetActive(false);
        resumeButton.SetActive(false);
        pauseButton.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Activate()
    {
        state = State.Paused;
        paused.SetActive(true);
        Time.timeScale = 0f;
        panel.SetActive(true); 
        resumeButton.SetActive(true);
        pauseButton.SetActive(true);
    }

    public void ExitConfirmation()
    {
        paused.SetActive(false);
        areYouSure.SetActive(true);
        panel.SetActive(true);
        resumeButton.SetActive(false);
        pauseButton.SetActive(false);
        sureButton.SetActive(true);
        noButton.SetActive(true);
    }

    public void Sure()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void No()
    {
        areYouSure.SetActive(false);
        paused.SetActive(true);
        panel.SetActive(true);
        resumeButton.SetActive(true);
        pauseButton.SetActive(true);
        sureButton.SetActive(false);
        noButton.SetActive(false);
    }
}
