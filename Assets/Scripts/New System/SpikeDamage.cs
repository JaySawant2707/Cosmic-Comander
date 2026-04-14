using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float damageCooldown = 0.5f;

    float lastDamageTime;
    bool isInside = false;
    GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            isInside = true;
            TryDealDamage(player);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInside = false;
        }
    }
 
    void Update()
    {
        if (isInside && Time.time >= lastDamageTime + damageCooldown)
        {
            TryDealDamage(player);
        }
    }

    void TryDealDamage(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}