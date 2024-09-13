using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_Platform : MonoBehaviour
{
    public float BreakTime = 2f;
    public float BreakAnimTime = 2f;
    public float reappearTime = 2f;

    Animator animator;
    SpriteRenderer sr;
    BoxCollider2D boxCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BreakBox();
        }
    }

    public void BreakBox()
    {
        StartCoroutine(BreakablePlatform());
    }

    IEnumerator BreakablePlatform()
    {
        yield return new WaitForSeconds(BreakTime);

        animator.SetBool("Break", true);

        yield return new WaitForSeconds(BreakAnimTime);

        sr.enabled = false;
        boxCollider.enabled = false;
        animator.SetBool("Break", false);

        yield return new WaitForSeconds(reappearTime);

        sr.enabled = true;
        boxCollider.enabled = true;
    }
}
