using UnityEngine;

public class BossTriggerZone : MonoBehaviour
{
    [SerializeField] private AstraSpawner spawner;

    public bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;
            spawner.StartSpawnSequence();

            //this.gameObject.SetActive(false); // Disable trigger after activation
        }
    }

    public void ResetTrigger()
    {
        triggered = false;
        //this.gameObject.SetActive(true); // Re-enable trigger if needed

        spawner.ResetEverything();
    }
}