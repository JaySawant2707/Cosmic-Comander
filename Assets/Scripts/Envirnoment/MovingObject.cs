using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    Vector3 targetPos;

    public float speed;
    public float waitDuration;
    public GameObject ways;
    public Transform[] waypoints;

    int pointIndex;
    int pointCount;
    int direction = 1;
    int speedMultiplier = 1;


    private void Awake()
    {
        waypoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            waypoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pointCount = waypoints.Length;
        pointIndex = 1;
        targetPos = waypoints[pointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var step = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if(transform.position == targetPos)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }

        if (pointIndex == 0)
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = waypoints[pointIndex].transform.position;
        StartCoroutine(WaitNextPoint());
    }

    IEnumerator WaitNextPoint()
    {
        speedMultiplier = 0;

        yield return new WaitForSeconds(waitDuration);

        speedMultiplier = 1;
    }
}
