using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [Header("Shooting")]
    [SerializeField] float fireRate = 5f; // bullets per second
    private float fireTimer;
    bool isShooting;

    PlayerInputHandler input;
    PlayerAnimationController anim;
    PlayerController controller;
    PlayerDeath playerDeath;

    private void Start()
    {
        anim = GetComponent<PlayerAnimationController>();
        input = GetComponent<PlayerInputHandler>();
        controller = GetComponent<PlayerController>();
        playerDeath = GetComponent<PlayerDeath>();
    }

    void Update()
    {
        if (playerDeath.IsAlive)
        {
            HandleShooting();
            if (anim) anim.PlayShoot(isShooting);
        }
    }

    void HandleShooting()
    {
        if (input.ShootPressed)
        {
            isShooting = true;
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = 1f / fireRate;
            }
        }
        else
        {
            isShooting = false;
            fireTimer = 0f; // reset for responsive shooting
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        int direction = controller.FacingDirection;

        bullet.GetComponent<Bullet>().Initialize(direction, gameObject);

        AudioManager.instance.PlaySFX("PlayerShoot");
    }
}