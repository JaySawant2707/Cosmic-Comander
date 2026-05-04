using System.Collections;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private VerticalDoor[] doors;

    [SerializeField] private Vector3 leftClosedPos;
    [SerializeField] private Vector3 rightClosedPos;

    [SerializeField] private float speed = 5f;

    public void OpenDoors(bool state)
    {
        foreach (var door in doors)
        {
            if (state)
                door.OpenDoor();
            else
                door.CloseDoor();
        }
    }
}