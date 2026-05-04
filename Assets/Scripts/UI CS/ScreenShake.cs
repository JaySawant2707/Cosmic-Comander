using UnityEngine;
using Cinemachine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        Instance = this;

        cam = GetComponent<CinemachineVirtualCamera>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f; // Start with no shake
    }

    public void Shake(float intensity, float time)
    {
        StartCoroutine(ShakeRoutine(intensity, time));
    }

    IEnumerator ShakeRoutine(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;

        yield return new WaitForSeconds(time);

        noise.m_AmplitudeGain = 0f;
    }
}