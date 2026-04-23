using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibleTime = 1f;
    [SerializeField] float knockbackForce = 5f;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    PlayerAnimationController anim;
    PlayerDeath playerDeath;

    int currentHealth;
    bool isInvincible = false;

    void Start()
    {
        anim = GetComponent<PlayerAnimationController>();
        currentHealth = maxHealth;
        playerDeath = GetComponent<PlayerDeath>();

        UpdateUI();
    }

    public bool CanHeal()
    {
        return currentHealth < maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || !playerDeath.IsAlive) return;

        currentHealth -= damage;

        anim.PlayHurt();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * knockbackForce, knockbackForce);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility());
        }

        UpdateUI();
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

    private void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

}