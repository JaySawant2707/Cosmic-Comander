using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public Animator animator;
    AudioManager audioManager;
    private Vector2 checkPointPos;
    Rigidbody2D rb;


    private void Start()
    {
        checkPointPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
        rb.velocity = new Vector2(0, 0);
        animator.SetTrigger("Death");
        audioManager.PlaySFX(audioManager.Death);
        rb.simulated = false;

        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.4f);

        transform.position = checkPointPos;
        animator.SetTrigger("Respawned");
        rb.simulated = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
