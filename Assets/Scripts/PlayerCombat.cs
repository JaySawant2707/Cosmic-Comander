using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public Transform firePoint;
    
    public LayerMask enemyLayer;

    public int SlashDamage = 20;
    public float attackRange = 0.05f;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public float ShootRate = 2f;
    float nextShootTime = 0f;

    public int ShootDamage = 20;
    public LineRenderer lineRenderer;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Slash();
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {           
            ShootRay();
        }
    }

    public void Slash()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            animator.SetTrigger("Slash");
            audioManager.PlaySFX(audioManager.Slash);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(SlashDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    public void ShootRay()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + 1f / ShootRate;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        animator.SetTrigger("Shoot");
        audioManager.PlaySFX(audioManager.laserShoot);

        yield return new WaitForSeconds(0.5f);


        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(ShootDamage);
            }

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);

        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.2f);

        lineRenderer.enabled = false;
    }

}
