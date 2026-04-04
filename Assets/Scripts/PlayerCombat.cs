using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform firePoint;

    [SerializeField] LayerMask enemyLayer;

    [SerializeField] int SlashDamage = 20;
    [SerializeField] float attackRange = 0.05f;

    [SerializeField] float attackRate = 2f;
    float nextAttackTime = 0f;

    [SerializeField] float ShootRate = 2f;
    float nextShootTime = 0f;

    [SerializeField] int ShootDamage = 20;
    [SerializeField] LineRenderer lineRenderer;
    
    PlayerInputHandler input;
    PlayerAnimationController anim;

    private void Start()
    {
        anim = GetComponent<PlayerAnimationController>();
        input = GetComponent<PlayerInputHandler>();
    }


    void Update()
    {
        if (input.AttackPressed)
        {
            Slash();
            input.AttackPressed = false; // reset
        }

        if (input.ShootPressed)
        {
            ShootRay();
            input.ShootPressed = false; // reset
        }
    }

    public void Slash()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            anim.PlaySlash();
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
        anim.PlayShoot();
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
