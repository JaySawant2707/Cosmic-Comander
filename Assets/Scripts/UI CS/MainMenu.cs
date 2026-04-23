using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        AudioManager.instance.PlayMusic("MainMenuTheme");
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
