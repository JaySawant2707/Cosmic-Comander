using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Player_Input : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] public float speed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpForceKeyB;

    [SerializeField] public float CayoteTime = 0.3f;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private LayerMask m_WhatIsGround;

    public Rigidbody2D rb;
    public Animator anim;
    public bool FacingRight = true;

    [SerializeField] private bool m_Grounded;
    const float k_GroundedRadius = .182f;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        anim.enabled = true;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        audioManager.Music.clip = audioManager.Lvbackground;
        audioManager.Music.Play();

    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        float movX = SimpleInput.GetAxisRaw("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(movX));

        rb.velocity = new Vector2(movX * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (m_Grounded)
            {
                anim.SetBool("IsJumping", true);
                rb.AddForce(Vector2.up * jumpForceKeyB);
            }
        }

        if (!m_Grounded)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        // If the input is moving the player right and the player is facing left...
        if (movX > 0 && !FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (movX < 0 && FacingRight)
        {
            // ... flip the player.
            Flip();
        }

    }

    public void Jump()
    {

        if (m_Grounded)
        {
            m_Grounded = false;
            anim.SetBool("IsJumping", true);
            rb.AddForce(Vector2.up * jumpForce);
        }

    }

    public void OnLanding()
    {
        anim.SetBool("IsJumping", false);

    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
    }
}
