using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibleTime = 1f;

    PlayerAnimationController anim;
    PlayerDeath playerDeath;

    int currentHealth;
    bool isInvincible = false;

    void Start()
    {
        anim = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth;
        playerDeath = GetComponent<PlayerDeath>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || !playerDeath.IsAlive) return;

        currentHealth -= damage;

        anim.PlayHurt();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 5f, 5f);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    void Die()
    {
        playerDeath.Death();
    }

    System.Collections.IEnumerator Invincibility()
    {
        isInvincible = true;

        // Optional: flash effect
        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}