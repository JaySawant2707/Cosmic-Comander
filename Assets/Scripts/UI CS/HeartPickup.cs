using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] int healAmount = 1;
    [SerializeField] GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.TryGetComponent<PlayerHealth>(out var playerHealth)) return;

        // Only pickup if not full health
        if (playerHealth.CanHeal())
        {
            playerHealth.Heal(healAmount);

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            AudioManager.instance.PlaySFX("Pickup");
            Destroy(gameObject);
        }
        // else: do nothing (no pickup)
    }
}