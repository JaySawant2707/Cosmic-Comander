using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 5f;
    Vector2 platformVelocity;

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

    [Header("Double Jump")]
    [SerializeField] int maxJumps = 2;
    int jumpCount;

    float coyoteTimeCounter;
    float jumpBufferCounter;
    Rigidbody2D rb;
    PlayerInputHandler input;
    PlayerAnimationController anim;

    bool isFacingRight = true;
    public int FacingDirection => isFacingRight ? 1 : -1;

    bool isGrounded;
    bool wasGrounded;
    bool hasJumped;
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
        ApplyBetterGravity(input.JumpHeld);
    }

    public void ApplyBetterGravity(bool jumpHeld)
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
        }
        else if (rb.linearVelocity.y > 0 && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
        }
    }

    void HandleMovement()
    {
        Move(input.MoveInput.x);
    }

    void Move(float input)
    {
        rb.linearVelocity = new Vector2(
            input * speed + platformVelocity.x,
            rb.linearVelocity.y
        );
    }

    public void SetPlatformVelocity(Vector2 velocity)
    {
        platformVelocity = velocity;
    }

    void HandleTimers()
    {
        if (isGrounded && !hasJumped)
            coyoteTimeCounter = coyoteTime;
        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            // When coyote time expires and the player never jumped (walked off a platform),
            // forfeit the first jump slot so the double jump is still available mid-air.
            // Without this, jumpCount stays 0 forever in mid-air, and the air jump branch
            // (which requires jumpCount >= 1) would never fire — locking out the double jump.
            if (coyoteTimeCounter <= 0f && jumpCount == 0 && !isGrounded)
                jumpCount = 1;
        }

        if (input.JumpPressed)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    void HandleJump()
    {
        // --- FIRST JUMP ---
        // Only reachable when grounded or within the coyote time window.
        // Mid-air presses with expired coyote time cannot trigger this branch.
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && jumpCount < maxJumps)
        {
            Jump();
            anim.PlayJump();

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            input.JumpPressed = false;
            hasJumped = true;

            jumpCount = 1; // mark first jump as consumed
        }

        // --- DOUBLE JUMP (and any extra air jumps) ---
        // Requires jumpCount >= 1: either the player used their first jump normally,
        // OR coyote time expired and HandleTimers auto-advanced jumpCount to 1 (fell off platform).
        // Both cases correctly allow exactly one air jump.
        else if (input.JumpPressed && jumpCount >= 1 && jumpCount < maxJumps && !isGrounded)
        {
            Jump();
            anim.PlayJump();

            input.JumpPressed = false;
            hasJumped = true;
            jumpCount++;
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void HandleFlip()
    {
        float move = input.MoveInput.x;

        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();
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
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            jumpCount = 0;
            hasJumped = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
}