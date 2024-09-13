using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShoot : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float shootingRange;
    public float fireRate;
    public GameObject bullet;
    public GameObject bulletPos;

    private Animator anim;
    private float nextFireTime;
    private Transform player;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange) 
        {
            anim.SetBool("isRunning", true);
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer < shootingRange && nextFireTime < Time.time)
        {
            anim.SetBool("isRunning", false);
            audioManager.PlaySFX(audioManager.Shoot);
            Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);

    }
}
