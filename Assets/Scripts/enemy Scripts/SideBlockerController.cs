using System.Collections;
using UnityEngine;

public class SideBlockerController : MonoBehaviour
{
    [SerializeField] private GameObject leftBlocker;
    [SerializeField] private GameObject rightBlocker;

    [SerializeField] private GameObject leftWarning;
    [SerializeField] private GameObject rightWarning;

    [SerializeField] private float warningTime = 0.8f;
    [SerializeField] private float blinkInterval = 0.1f;

    public IEnumerator ActivateWithWarning()
    {
        // Show warnings
        leftWarning.SetActive(true);
        rightWarning.SetActive(true);

        // Blink effect
        yield return StartCoroutine(BlinkWarnings());

        // Hide warnings
        leftWarning.SetActive(false);
        rightWarning.SetActive(false);

        // Activate actual blockers
        leftBlocker.SetActive(true);
        rightBlocker.SetActive(true);
    }

    IEnumerator BlinkWarnings()
    {
        float timer = 0f;

        while (timer < warningTime)
        {
            leftWarning.SetActive(!leftWarning.activeSelf);
            rightWarning.SetActive(!rightWarning.activeSelf);

            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval;
        }

        // Ensure visible before firing
        leftWarning.SetActive(true);
        rightWarning.SetActive(true);

        yield return new WaitForSeconds(0.1f);
    }

    public void Deactivate()
    {
        leftBlocker.SetActive(false);
        rightBlocker.SetActive(false);

        leftWarning.SetActive(false);
        rightWarning.SetActive(false);
    }
}