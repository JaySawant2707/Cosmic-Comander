using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 0.5f;

    private float lastDamageTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            TryDealDamage(collision.gameObject);
        }
    }

    void TryDealDamage(GameObject target)
    {
        var damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}