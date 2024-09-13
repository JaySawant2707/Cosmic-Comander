using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
    [SerializeField] GameObject creditPanel;

    public void Credits()
    {
        creditPanel.SetActive(true);

    }

    public void BackToM_M()
    {
        creditPanel.SetActive(false);

    }
}
