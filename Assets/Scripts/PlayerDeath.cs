using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    PlayerAnimationController anim;
    Rigidbody2D rb;
    Vector2 checkPointPos;

    private void Start()
    {
        anim = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();
        checkPointPos = transform.position;
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
        rb.linearVelocity = new Vector2(0, 0);
        anim.PlayDeath();
        audioManager.PlaySFX(audioManager.Death);
        rb.simulated = false;

        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.4f);

        transform.position = checkPointPos;
        anim.PlayRespawn();
        rb.simulated = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
