using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;
    [Header("VFX")]
    [SerializeField] GameObject explosionPrefab;

    private int direction = 1;
    private GameObject owner;

    void Awake()
    {
        if (!gameObject.GetComponent<Rigidbody2D>())
            gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public void Initialize(int dir, GameObject ownerObj)
    {
        direction = dir;
        owner = ownerObj;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore self (owner)
        if (collision.gameObject == owner)
            return;

        var damageable = collision.GetComponent<IDamageable>();
        damageable?.TakeDamage(1);

        // Spawn explosion
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}