using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        audioManager.Music.clip = audioManager.MMbackground;
        audioManager.Music.Play();
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("All_Levels");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
