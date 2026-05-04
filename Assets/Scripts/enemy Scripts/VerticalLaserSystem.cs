using System.Collections;
using UnityEngine;

public class VerticalLaserSystem : MonoBehaviour
{
    [SerializeField] private Transform[] laserPoints;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private float warningTime = 0.6f;
    [SerializeField] private float laserDuration = 0.5f;
    [SerializeField] private float gap = 0.2f;

    Transform laserParent;

    public void Init(Transform parent)
    {
        laserParent = parent;
    }

    public IEnumerator RunSweep()
    {
        for (int i = laserPoints.Length - 1; i >= 0; i--) // Right → Left
        {
            Transform point = laserPoints[i];

            GameObject warn = Instantiate(warningPrefab, point.position, Quaternion.identity, laserParent);
            AudioManager.instance.PlaySFX("LaserCharge");
            StartCoroutine(Blink(warn));
            
            yield return new WaitForSeconds(warningTime);

            Destroy(warn);

            GameObject laser = Instantiate(laserPrefab, point.position, Quaternion.identity, laserParent);
            AudioManager.instance.PlaySFX("LaserFire");

            ScreenShake.Instance.Shake(1.5f, 0.15f);

            yield return new WaitForSeconds(laserDuration);
            Destroy(laser);

            yield return new WaitForSeconds(gap);
        }
    }

    IEnumerator Blink(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}