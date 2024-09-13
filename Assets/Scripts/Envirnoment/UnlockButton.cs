using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockButton : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject Door;
    public Animator anim;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        anim = Door.GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spriteRenderer.enabled)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
            {
                audioManager.PlaySFX(audioManager.DoorOpen);
                anim.SetBool("Open", true);
                spriteRenderer.enabled = false;
            }
        }
        
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!spriteRenderer.enabled)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
            {
                audioManager.PlaySFX(audioManager.DoorOpen);
                anim.SetBool("Open", false);
                spriteRenderer.enabled = true;
            }
        }
       
    }

}
