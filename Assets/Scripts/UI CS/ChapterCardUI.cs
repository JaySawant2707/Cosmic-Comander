using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChapterCardUI : MonoBehaviour
{
    [Header("Setup")]
    public int chapterIndex;
    public string sceneName;

    [Header("UI")]
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI taglineText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        bool unlocked = ChapterManager.instance.IsChapterUnlocked(chapterIndex);

        playButton.interactable = unlocked;
        lockIcon.SetActive(!unlocked);
    }

    public void OnPlayClicked()
    {
        if (!ChapterManager.instance.IsChapterUnlocked(chapterIndex))
            return;

        SceneManager.LoadScene(sceneName);
    }
}