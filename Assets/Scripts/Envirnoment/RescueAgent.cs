using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RescueAgent : MonoBehaviour, IDamageable
{
    [SerializeField] FinalDoor doorToOpen;
    [SerializeField] GameObject agentVisual;
    [SerializeField] Sprite brokenPodSprite;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] int health = 4;

    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Rescue()
    {
        col.enabled = false;

        if (this.gameObject.TryGetComponent<SpriteRenderer>(out var sr))
            sr.sprite = brokenPodSprite;

        // Play animation / sound

        StartCoroutine(FadeOut(agentVisual.GetComponent<SpriteRenderer>(), fadeDuration));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Rescue();
        }
    }

    public IEnumerator FadeOut(SpriteRenderer sprite, float duration)
    {
        float time = 0f;
        Color startColor = sprite.color;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startColor.a, 0f, time / duration);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        agentVisual.SetActive(false);

        if (doorToOpen != null)
        {
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, 1);
            doorToOpen.OpenDoor(agentVisual);
        }
    }
}