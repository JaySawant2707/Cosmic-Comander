using UnityEngine;

public class droneSpawner : MonoBehaviour
{ 
    public float DRange;
    public float fireRate;
    public GameObject drone;
    public GameObject dronePos;
    [SerializeField] private bool spawnOnTop = false;

    private bool canSpawnDrones;
    private float nextFireTime;
    private Transform player;
    AudioManager audioManager;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        
        if (distanceFromPlayer < DRange && nextFireTime < Time.time && canSpawnDrones)
        {
            animator.SetTrigger("spawn");
            audioManager.PlaySFX(audioManager.Shoot);
            Instantiate(drone, dronePos.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }

        if (!spawnOnTop)
        {
            if (transform.position.y < player.transform.position.y)
            {
                canSpawnDrones = false;
            }
            else
            {
                canSpawnDrones = true;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DRange);

    }
}
