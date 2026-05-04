using System.Collections;
using UnityEngine;

public class HorizontalLaser : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float chargeTime = 0.6f;
    [SerializeField] private float duration = 0.6f;
    Transform laserParent;

    public void Init(Transform parent)
    {
        laserParent = parent;
    }

    public IEnumerator Fire()
    {
        // 🔴 WARNING LINE
        GameObject warn = Instantiate(
            warningPrefab,
            firePoint.position,
            Quaternion.Euler(0, 0, 90f), // horizontal warning
            laserParent
        );
        AudioManager.instance.PlaySFX("LaserCharge");

        StartCoroutine(Blink(warn));

        // Optional: make it blink
        yield return new WaitForSeconds(chargeTime);

        Destroy(warn);

        // ⚡ FIRE LASER
        GameObject laser = Instantiate(
            laserPrefab,
            firePoint.position,
            Quaternion.Euler(0, 0, 90f),
            laserParent
        );

        ScreenShake.Instance.Shake(2f, 0.2f);
        AudioManager.instance.PlaySFX("LaserFire");

        yield return new WaitForSeconds(duration);

        Destroy(laser);
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