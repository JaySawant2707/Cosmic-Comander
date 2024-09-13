using UnityEngine;

public class Unlock_Board : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject Door;
    public Animator anim;
    Animator animator;
    public bool isOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        anim = Door.GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Triggered");
            audioManager.PlaySFX(audioManager.DoorOpen);
            anim.SetBool("Open", true);
            isOpen = true;
        }
    }
}
