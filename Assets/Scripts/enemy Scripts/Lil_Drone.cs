using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lil_Drone : MonoBehaviour
{
    public float speed;
    public float loseRange;
    [SerializeField] private float blastRange = 3f;
    float distanceFromPlayer;
    private Transform player;
    private bool lost = false;

    Animator animator;
    Rigidbody2D rb;
    PlayerDeath PD;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PD = player.GetComponent<PlayerDeath>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        
        if (distanceFromPlayer > loseRange)
        {
           
            
                transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            
            
        }
        else if (!lost)
        {
            lost = true;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
            
        }
        if (lost) loseRange = distanceFromPlayer + 1f;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, loseRange);
        Gizmos.DrawWireSphere(transform.position, blastRange);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            StartCoroutine(DroneExplosion());
            
        }

        if (collision.gameObject.CompareTag("Platform"))
        StartCoroutine(DroneExplosion());

        
    }

    IEnumerator DroneExplosion()
    {
        audioManager.PlaySFX(audioManager.Blast);
        animator.SetBool("Exploded", true);
        yield return new WaitForSeconds(0.5f);
        if (distanceFromPlayer < blastRange)
        {
            PD.Death();
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
