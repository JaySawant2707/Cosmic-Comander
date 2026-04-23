using UnityEngine;
using TMPro;

public class DataFragmentManager : MonoBehaviour
{
    //public static DataFragmentManager instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI fragmentText;

    private int currentFragments = 0;

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void Start()
    {
        UpdateUI();
    }

    public void AddFragment(int amount = 1)
    {
        currentFragments += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (fragmentText != null)
        {
            fragmentText.text = currentFragments.ToString();
        }
    }
}