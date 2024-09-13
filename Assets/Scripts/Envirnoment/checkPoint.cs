using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    public GameObject respawn;
    Animator animator;
    PlayerDeath PD;
    public bool isActive = false;

    private void Start()
    {
       
        PD = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("active", true);
            PD.UpdateCheckpoint(respawn.transform.position);
        }
    }
}
