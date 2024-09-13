using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectLevels : MonoBehaviour
{
    AudioManager audioManager;

    public Button[] buttons;
    public GameObject AllLevels;

    private void Start()
    {

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        audioManager.Music.clip = audioManager.MMbackground;
        audioManager.Music.Play();
    }
    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++) 
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelID)
    {

        string levelName = "Level" + levelID;
        SceneManager.LoadSceneAsync(levelName);
        

    }

    public void ButtonsToArray()
    {
        int childCount = AllLevels.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0;i < childCount;i++)
        {
            buttons[i] = AllLevels.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

    

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
