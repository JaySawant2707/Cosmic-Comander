using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private Enemy enemy;
    private EnemyShooting ES;
    private GameObject player;

    public GameObject PointA;
    public GameObject PointB;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;

    public float speed;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ES = GetComponent<EnemyShooting>();
        enemy = GetComponent<Enemy>(); 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = PointB.transform;
        animator.SetBool("isRunning", true);
    }
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        Vector2 point = currentPoint.position - transform.position;

        if (enemy.isDead == false)
        {
            if (distance > ES.Range)
            {
                if (currentPoint == PointB.transform)
                {
                    rb.velocity = new Vector2(-speed, 0);
                }
                else
                {
                    rb.velocity = new Vector2(speed, 0);
                }

                if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
                {
                    Flip();
                    currentPoint = PointA.transform;
                }
                if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
                {
                    Flip();
                    currentPoint = PointB.transform;
                }
            }
            
        }
        
    }

    private void Flip()
    {
        Vector3 localscale = transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);

    }
}
