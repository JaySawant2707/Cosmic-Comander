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

        if (!controller.IsGrounded && rb.linearVelocity.y > 0.1f)
        {
            animator.Play("Jump");
        }
        else if (!controller.IsGrounded && rb.linearVelocity.y < -0.1f)
        {
            //animator.Play("Fall");
        }
    }

    // Called from other scripts
    public void PlayJump() => animator.SetTrigger("Jump");
    public void PlaySlash() => animator.SetTrigger("Slash");
    public void PlayShoot() => animator.SetTrigger("Shoot");
    public void PlayDeath() => animator.SetTrigger("Death");
    public void PlayRespawn() => animator.SetTrigger("Respawned");
}