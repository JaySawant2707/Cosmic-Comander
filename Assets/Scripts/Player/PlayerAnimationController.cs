using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    Rigidbody2D rb;

    PlayerController controller;
    PlayerInputHandler input;

    void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateAirState();
    }

    void UpdateMovement()
    {
        float speed = Mathf.Abs(input.MoveInput.x);
        animator.SetFloat("Speed", speed);
    }

    void UpdateAirState()
    {
        animator.SetBool("IsGrounded", controller.IsGrounded);

        if (!controller.IsGrounded)
        {
            PlayJump();
        }
    }

    // Called from other scripts
    public void PlayShoot(bool state)
    {
        animator.SetBool("IsShooting", state);
    }
    public void PlayJump()
    {
        animator.SetTrigger("Jump");
    }
    public void PlaySlash()
    {
        animator.SetTrigger("Slash");
    }
    public void PlayShoot()
    {
        animator.SetTrigger("Shoot");
    }
    public void PlayHurt()
    {
        ResetAllTriggers();
        animator.SetTrigger("Hurt");
    }
    public void PlayDeath()
    {
        ResetAllTriggers();
        animator.SetTrigger("Death");
    }
    public void PlayRespawn()
    {
        ResetAllTriggers();
        animator.SetTrigger("Respawned");
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Slash");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("Death");
        animator.ResetTrigger("Respawned");
    }
}