using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputHandler input;
    PlayerMotor motor;
    PlayerAnimationController anim;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float radius = 0.2f;
    public LayerMask groundLayer;

    [Header("Jump Feel")]
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private bool isFacingRight = true;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        CheckGround();
        HandleTimers();
        HandleJump();
        HandleFlip();
    }

    void FixedUpdate()
    {
        HandleMovement();
        motor.ApplyBetterGravity(input.JumpPressed);
    }

    void HandleMovement()
    {
        motor.Move(input.MoveInput.x);
    }

    void HandleTimers()
    {
        // Coyote time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        if (input.JumpPressed)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            motor.Jump();
            anim.PlayJump();

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            input.JumpPressed = false;
        }
    }

    void HandleFlip()
    {
        float move = input.MoveInput.x;

        // Only flip if actually moving (prevents jitter)
        if (move > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (move < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            radius,
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
}