using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 5f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float radius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    float coyoteTimeCounter;
    float jumpBufferCounter;
    Rigidbody2D rb;
    PlayerInputHandler input;
    PlayerAnimationController anim;

    bool isFacingRight = true;
    public int FacingDirection => isFacingRight ? 1 : -1;
    bool isGrounded;
    public bool IsGrounded => isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputHandler>();
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
        ApplyBetterGravity(input.JumpPressed);
    }

    public void ApplyBetterGravity(bool jumpHeld)
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void HandleMovement()
    {
        Move(input.MoveInput.x);
    }

    void Move(float input)
    {
        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
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
            Jump();
            anim.PlayJump();

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            input.JumpPressed = false;
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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