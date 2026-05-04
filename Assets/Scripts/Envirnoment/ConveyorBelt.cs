using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector2 direction = Vector2.right;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.position += speed * Time.deltaTime * direction.normalized;
            }
        }
    }
}