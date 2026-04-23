using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    public float disappearTime = 0f;
    public bool isDead = false;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        AudioManager.instance.PlaySFX("Blast");

        isDead = true;

        animator.SetBool("isDead", true);

        Destroy(gameObject, disappearTime);
        
    }

}
