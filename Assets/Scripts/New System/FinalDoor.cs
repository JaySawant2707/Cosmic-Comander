using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class FinalDoor : MonoBehaviour
{
    [SerializeField] Transform agentSpawnPoint;
    [SerializeField] float delayBeforeLoad = 0.2f;

    Collider2D col;
    Animator anim;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        col.enabled = false;
    }

    public void OpenDoor(GameObject agent)
    {
        agent.transform.position = agentSpawnPoint.position;
        agent.SetActive(true);

        col.enabled = true; // Allow player to trigger level transition
        anim.SetTrigger("OpenDoor");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(OpenNextLevel());
        }
    }

    IEnumerator OpenNextLevel()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }
}
