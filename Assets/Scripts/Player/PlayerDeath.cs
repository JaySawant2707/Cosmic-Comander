using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] float beforeRespawnCooldown = 0.5f;
    PlayerAnimationController anim;
    PlayerController playerController;
    PlayerHealth playerHealth;
    PlayerInputHandler playerInputHandler;
    Rigidbody2D rb;
    Vector2 checkPointPos;
    bool isAlive = true;
    public bool IsAlive => isAlive;

    private void Start()
    {
        anim = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        checkPointPos = transform.position;
        isAlive = true;
    }

    void Update()
    {
        if(!isAlive) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            Death();
        }
    }

    public void UpdateCheckpoint(Vector2 newCheckPoint)
    {
        checkPointPos = newCheckPoint;
    }

    public void Death()
    {
        isAlive = false;
        anim.PlayDeath();
        AudioManager.instance.PlaySFX("Death");
        playerController.enabled = false;
        playerInputHandler.enabled = false;

        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(beforeRespawnCooldown);

        Respawn();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Respawn()
    {
        isAlive = true;
        playerController.enabled = true;
        playerInputHandler.enabled = true;
        playerHealth.ResetHealth();
        transform.position = checkPointPos;
        anim.PlayRespawn();
        rb.simulated = true;
    }
}
