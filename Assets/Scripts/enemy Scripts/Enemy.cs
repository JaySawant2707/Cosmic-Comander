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
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
        audioManager.PlaySFX(audioManager.Blast);

        isDead = true;

        animator.SetBool("isDead", true);

        Destroy(gameObject, disappearTime);
        
    }

}
