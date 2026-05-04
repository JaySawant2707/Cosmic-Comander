using UnityEngine;

public class BossWeakPoint : MonoBehaviour, IDamageable
{
    [SerializeField] private AstraBoss boss;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private GameObject dieVFX;
    public int MaxHealth => maxHealth;
    private int currentHealth;
    public System.Action<int> OnHealthChanged;
    public System.Action OnBossDied;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int dmg)
    {
        if (!boss.CanTakeDamage()) return;

        currentHealth -= dmg;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Defeated!");

        if (dieVFX != null)
        {
            Instantiate(dieVFX, transform.position, Quaternion.identity);
        }

        // BUG 3 FIX: Tell the boss state machine to stop immediately.
        // Without this, StunnedPhase finishes its wait and re-enters AttackPhase
        // even though the boss is already dead.
        boss.OnBossDefeated();

        boss.OpenDoors(true);
        OnBossDied?.Invoke();
        this.gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
}