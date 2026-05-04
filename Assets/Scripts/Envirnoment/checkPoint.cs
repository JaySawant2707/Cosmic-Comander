using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class checkPoint : MonoBehaviour
{
    private static readonly int ActiveHash = Animator.StringToHash("active");
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
            animator.SetBool(ActiveHash, true);
            PD.UpdateCheckpoint(respawn.transform.position);
        }
    }
}
