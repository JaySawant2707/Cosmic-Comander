using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int health = 3;

    [Header("VFX")]
    [SerializeField] GameObject explosionPrefab;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Spawn explosion
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}