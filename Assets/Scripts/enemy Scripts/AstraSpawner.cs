using System.Collections;
using UnityEngine;

public class AstraSpawner : MonoBehaviour
{
    [SerializeField] private GameObject astraPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GateController gateController;
    [SerializeField] private BossHealthSliderUI bossUI;

    [Header("Spawn Effects")]
    [SerializeField] private GameObject spawnVFX;
    [SerializeField] private float spawnDelay = 1f;

    public void StartSpawnSequence()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        // Close gates
        gateController.OpenDoors(false);

        // Small pause (dramatic)
        yield return new WaitForSeconds(0.5f);

        // Spawn effect
        if (spawnVFX != null)
        {
            Instantiate(spawnVFX, spawnPoint.position, Quaternion.identity);
        }

        AudioManager.instance.PlaySFX("AstraSpawn");

        bossUI.bossHealthPanel.SetActive(true);

        yield return new WaitForSeconds(spawnDelay);

        // Activate Astra
        astraPrefab.SetActive(true);
        astraPrefab.GetComponent<AstraBoss>().StartBossFight();
    }

    public void ResetEverything()
    {
        // Reset gates
        gateController.OpenDoors(true);

        // Reset health first (while the object is still active so ResetHealth fires correctly)
        astraPrefab.GetComponent<BossWeakPoint>().ResetHealth();

        // BUG 5 FIX: ResetEverythingAndDisable already calls gameObject.SetActive(false)
        // at the end, so the extra astraPrefab.SetActive(false) line below it was redundant.
        // Removed to keep the intent clear.

        astraPrefab.GetComponent<AstraBoss>().ResetEverythingAndDisable();
        // Reset UI
        bossUI.ResetSlider();
        bossUI.bossHealthPanel.SetActive(false);
    }
}