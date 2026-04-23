using UnityEngine;

public class DataFragmentPickup : MonoBehaviour
{
    public int value = 1;
    [SerializeField] GameObject pickupEffect;

    DataFragmentManager dataFragmentManager;

    void Start()
    {
        dataFragmentManager = FindFirstObjectByType<DataFragmentManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        dataFragmentManager.AddFragment(value);

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        AudioManager.instance.PlaySFX("Pickup");
        Destroy(gameObject);
    }
}