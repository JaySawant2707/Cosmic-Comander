using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthSliderUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] public GameObject bossHealthPanel;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private BossWeakPoint bossWeakPoint;

    // BUG 4 FIX: Track the running smooth coroutine so we can cancel and restart it
    // when damage happens faster than the animation finishes
    private Coroutine smoothCoroutine;

    void OnEnable()
    {
        bossWeakPoint.OnHealthChanged += SetHealthSlider;
        bossWeakPoint.OnBossDied += DisableHealthPanel;
    }

    void Start()
    {
        bossHealthPanel.SetActive(false);
        slider.minValue = 0;
        slider.maxValue = bossWeakPoint.MaxHealth;
        slider.value = bossWeakPoint.MaxHealth;
    }

    public void ResetSlider()
    {
        if (smoothCoroutine != null)
        {
            StopCoroutine(smoothCoroutine);
            smoothCoroutine = null;
        }
        slider.value = bossWeakPoint.MaxHealth;
    }

    // BUG 4 FIX: The original used Mathf.Lerp inside an event callback (called once),
    // which only performed a single lerp step — the slider barely moved.
    // Now we start a coroutine that interpolates every frame until it reaches the target.
    public void SetHealthSlider(int targetValue)
    {
        if (smoothCoroutine != null)
            StopCoroutine(smoothCoroutine);

        smoothCoroutine = StartCoroutine(SmoothToTarget(targetValue));
    }

    IEnumerator SmoothToTarget(int targetValue)
    {
        while (Mathf.Abs(slider.value - targetValue) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);
            yield return null;
        }

        slider.value = targetValue;
        smoothCoroutine = null;
    }

    void DisableHealthPanel()
    {
        if (smoothCoroutine != null)
        {
            StopCoroutine(smoothCoroutine);
            smoothCoroutine = null;
        }
        bossHealthPanel.SetActive(false);
    }

    void OnDisable()
    {
        bossWeakPoint.OnHealthChanged -= SetHealthSlider;
        bossWeakPoint.OnBossDied -= DisableHealthPanel;
    }
}