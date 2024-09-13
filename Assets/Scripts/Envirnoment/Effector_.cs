using UnityEngine;

public class Effector_ : MonoBehaviour
{
    AreaEffector2D effector;

    private void Start()
    {
        effector = GetComponent<AreaEffector2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            effector.enabled = false;
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            effector.enabled = true;
        }
    }
}
