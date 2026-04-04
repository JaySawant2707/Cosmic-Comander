using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player_Input : MonoBehaviour
{
    private const float GroundedRadius = 0.182f;

    private AudioManager audioManager;
    private PlatformerInputHandler inputHandler;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator anim;

    [Header("Movement")]
    [FormerlySerializedAs("speed")]
    [SerializeField] private float moveSpeed = 8f;
    [FormerlySerializedAs("jumpForce")]
    [FormerlySerializedAs("jumpForceKeyB")]
    [SerializeField] private float jumpVelocity = 12f;

    [Header("Jump Feel")]
    [FormerlySerializedAs("CayoteTime")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Ground Check")]
    [FormerlySerializedAs("m_GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [FormerlySerializedAs("m_WhatIsGround")]
    [SerializeField] private LayerMask whatIsGround;

    [Header("State")]
    [FormerlySerializedAs("m_Grounded")]
    [SerializeField] private bool isGrounded;
    [FormerlySerializedAs("FacingRight")]
    [SerializeField] private bool facingRight = true;

    [Header("Events")]
    public UnityEvent OnLandEvent;

    private float coyoteTimer;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        inputHandler = GetComponent<PlatformerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        if (audioManager != null)
        {
            audioManager.Music.clip = audioManager.Lvbackground;
            audioManager.Music.Play();
        }
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();
        ApplyHorizontalMovement();
        HandleJump();
    }

    private void Update()
    {
        float moveInput = inputHandler != null ? inputHandler.Move.x : 0f;

        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetBool("IsJumping", !isGrounded);

        if (moveInput > 0.01f && !facingRight)
        {
            Flip();
        }
        else if (moveInput < -0.01f && facingRight)
        {
            Flip();
        }
    }

    private void UpdateGroundedState()
    {
        bool wasGrounded = isGrounded;
        isGrounded = IsGroundedNow();

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            if (!wasGrounded)
            {
                OnLandEvent.Invoke();
            }
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
    }

    private bool IsGroundedNow()
    {
        if (groundCheck != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] != null && hits[i].gameObject != gameObject)
                {
                    return true;
                }
            }

            return false;
        }

        if (playerCollider != null)
        {
            return playerCollider.IsTouchingLayers(whatIsGround);
        }

        return false;
    }

    private void ApplyHorizontalMovement()
    {
        float moveInput = inputHandler != null ? inputHandler.Move.x : 0f;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void HandleJump()
    {
        if (inputHandler == null)
        {
            return;
        }

        bool hasBufferedJump = inputHandler.HasBufferedJump(jumpBufferTime);
        bool canUseCoyote = coyoteTimer > 0f;

        if (hasBufferedJump && canUseCoyote)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpVelocity;
            rb.linearVelocity = velocity;

            isGrounded = false;
            coyoteTimer = 0f;
            inputHandler.ConsumeBufferedJump();
            anim.SetBool("IsJumping", true);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, GroundedRadius);
    }
}
