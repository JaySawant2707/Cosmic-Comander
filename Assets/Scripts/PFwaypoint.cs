using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFwaypoint : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speeed = 5f;

    private int currentWaypointIndex = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) 
            { 
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speeed);
    }
}
