using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] List<Transform> points = new List<Transform>();
    [SerializeField] float speed = 2f;
    [SerializeField] float waitTime = 0.5f;

    [Header("Movement Mode")]
    [SerializeField] bool loop = true; // if false → ping pong

    [Header("State")]
    [SerializeField] bool isMoving = true;

    Vector3 lastPosition;
    public Vector2 PlatformVelocity { get; private set; }
    private PlayerController playerController;

    int currentIndex = 0;
    int direction = 1; // for ping-pong
    bool isWaiting = false;

    // ---------------- UPDATE ----------------
    void Update()
    {
        if (!isMoving || points.Count == 0 || isWaiting)
        {
            PlatformVelocity = Vector2.zero;
            return;
        }

        lastPosition = transform.position;

        MovePlatform();

        PlatformVelocity = (transform.position - lastPosition) / Time.deltaTime;
    }

    void MovePlatform()
    {
        Transform target = points[currentIndex];

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            StartCoroutine(WaitAndNext());
        }
    }

    System.Collections.IEnumerator WaitAndNext()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        UpdateIndex();

        isWaiting = false;
    }

    void UpdateIndex()
    {
        if (loop)
        {
            currentIndex = (currentIndex + 1) % points.Count;
        }
        else
        {
            currentIndex += direction;

            if (currentIndex >= points.Count)
            {
                currentIndex = points.Count - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }
        }
    }

    // ---------------- CONTROL ----------------

    public void SetMoving(bool value)
    {
        isMoving = value;
    }

    public void ToggleMovement()
    {
        isMoving = !isMoving;
    }

    // ---------------- PLAYER PARENTING ----------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController != null)
        {
            playerController.SetPlatformVelocity(PlatformVelocity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController != null)
        {
            playerController.SetPlatformVelocity(Vector2.zero);
            playerController = null;
        }
    }

    // ---------------- DEBUG ----------------

    private void OnDrawGizmos()
    {
        if (points == null || points.Count == 0) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i] != null)
            {
                Gizmos.DrawSphere(points[i].position, 0.1f);

                if (i < points.Count - 1)
                {
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                }
            }
        }
    }
}