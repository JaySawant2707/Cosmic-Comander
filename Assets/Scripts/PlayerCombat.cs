using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [Header("Shooting")]
    [SerializeField] float fireRate = 5f; // bullets per second
    private float fireTimer;

    [Header("Audio")]
    [SerializeField] AudioManager audioManager;

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
            HandleShooting();
    }

    void HandleShooting()
    {
        if (input.ShootPressed)
        {
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = 1f / fireRate;
            }
        }
        else
        {
            fireTimer = 0f; // reset for responsive shooting
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        int direction = controller.FacingDirection;

        bullet.GetComponent<Bullet>().Initialize(direction);

        if (anim != null)
            anim.PlayShoot();

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.laserShoot);
    }
}