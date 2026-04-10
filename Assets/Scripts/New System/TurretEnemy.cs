using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform player;

    [Header("Settings")]
    [SerializeField] float detectionRange = 10f;
    [SerializeField] int facingDirection = 1;
    [SerializeField] LayerMask obstacleLayer; // walls/ground
    [SerializeField] LayerMask playerLayer;

    Vector2 direction;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        if (CanSeePlayer())
        {
            animator.SetBool("IsShooting", true);
        }
        else
        {
            animator.SetBool("IsShooting", false);
        }
    }

    bool CanSeePlayer()
    {
        direction = Vector2.right * facingDirection;

        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.position,
            direction,
            detectionRange,
            obstacleLayer | playerLayer
        );

        if (hit.collider != null)
        {
            // Check if first thing hit is player
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                return true;
            }
        }

        return false;
    }

    // This method will called in Animation Event
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(facingDirection, gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = Vector3.right * facingDirection;

        Gizmos.DrawLine(transform.position, transform.position + direction * detectionRange);

    }
}