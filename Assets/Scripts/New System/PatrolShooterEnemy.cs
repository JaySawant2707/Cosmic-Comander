using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PatrolShooterEnemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float waitTime = 1f;

    [Header("Combat")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float detectionRange = 8f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask obstacleLayer;

    Animator animator;
    Transform player;
    private Transform currentTarget;
    private bool isWaiting;
    private bool isAttacking;
    private int facingDirection = 1;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentTarget = pointA;
        UpdateFacing();
    }

    void Update()
    {
        if (player == null) return;

        if (CanSeePlayer())
        {
            HandleAttack();
        }
        else
        {
            HandlePatrol();
        }
        animator.SetBool("IsShooting", isAttacking);
    }

    // ---------------- PATROL ----------------
    void HandlePatrol()
    {
        if (isAttacking || isWaiting) return;

        animator.SetBool("IsRunning", true);

        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            StartCoroutine(WaitAndSwitch());
        }
    }

    IEnumerator WaitAndSwitch()
    {
        isWaiting = true;
        animator.SetBool("IsRunning", false);

        yield return new WaitForSeconds(waitTime);

        currentTarget = currentTarget == pointA ? pointB : pointA;

        UpdateFacing();
        isWaiting = false;
    }

    // ---------------- ATTACK ----------------
    void HandleAttack()
    {
        isAttacking = true;

        animator.SetBool("IsRunning", false);
    }

    // Called from Animation Event
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(facingDirection, gameObject);
    }

    // ---------------- DETECTION ----------------
    bool CanSeePlayer()
    {
        Vector2 direction = Vector2.right * facingDirection;

        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.position,
            direction,
            detectionRange,
            playerLayer | obstacleLayer
        );

        if (hit.collider != null)
        {
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                return true;
            }
        }

        isAttacking = false;
        return false;
    }

    // ---------------- FACING ----------------
    void UpdateFacing()
    {
        float dir = currentTarget.position.x - transform.position.x;

        if (dir > 0)
            facingDirection = 1;
        else
            facingDirection = -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        int dir = transform.localScale.x > 0 ? 1 : -1;

        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.right * dir * detectionRange
        );
    }
}