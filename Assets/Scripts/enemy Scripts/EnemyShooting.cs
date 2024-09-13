using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyShooting : MonoBehaviour
{
    public Enemy enemy;
    private Animator animator;
    private GameObject player;

    public GameObject Bullet;
    public Transform BulletPOS;

    public float Range = 5f;
    public float timer = 1;
    public float ShootAnimDelay;

    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    
    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (enemy.isDead == false)
        {
            if (distance < Range)
            {
                timer += Time.deltaTime;

                if (timer > 2)
                {
                    timer = 0;
                    animator.SetTrigger("Attack");
                    
                    StartCoroutine(Shoot());
                }
            }
        }
        

        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(ShootAnimDelay);

        audioManager.PlaySFX(audioManager.Shoot);

        Instantiate(Bullet, BulletPOS.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Range);

    }
}
