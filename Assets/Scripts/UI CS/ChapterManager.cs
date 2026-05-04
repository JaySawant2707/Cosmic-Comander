using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance;

    private const string KEY = "UnlockedChapter";

    public int UnlockedChapter { get; private set; } = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadProgress()
    {
        UnlockedChapter = PlayerPrefs.GetInt(KEY, 1);
    }

    public void UnlockNextChapter(int completedChapter)
    {
        int next = completedChapter + 1;

        if (next > UnlockedChapter)
        {
            UnlockedChapter = next;
            PlayerPrefs.SetInt(KEY, UnlockedChapter);
        }
    }

    public bool IsChapterUnlocked(int chapterIndex)
    {
        return chapterIndex <= UnlockedChapter;
    }
}