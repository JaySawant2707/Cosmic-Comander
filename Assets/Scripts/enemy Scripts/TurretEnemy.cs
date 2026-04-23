using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TurretEnemy : MonoBehaviour
{
    public enum FacingDirection
    {
        Left, Right
    }
    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [Header("Settings")]
    [SerializeField] float detectionRange = 10f;
    [SerializeField] FacingDirection facingDirection = FacingDirection.Left;
    [SerializeField] LayerMask obstacleLayer; // walls/ground
    [SerializeField] LayerMask playerLayer;

    int facingDir;
    Transform player;
    Vector2 direction;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        facingDir = facingDirection == FacingDirection.Left ? 1 : -1;
        Flip(facingDir);
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
        direction = Vector2.left * facingDir;

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
        bullet.GetComponent<Bullet>().Initialize(-facingDir, gameObject);

        AudioManager.instance.PlaySFX("EnemyShoot");
    }

    private void Flip(int direction)
    {
        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = Vector3.left * facingDir;
        Gizmos.DrawLine(transform.position, transform.position + direction * detectionRange);
    }
}