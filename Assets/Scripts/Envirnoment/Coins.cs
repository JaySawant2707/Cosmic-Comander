using UnityEngine;

public class Coins : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX("CoinCollect");
            Destroy(gameObject);
        }
    }
}
