using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}