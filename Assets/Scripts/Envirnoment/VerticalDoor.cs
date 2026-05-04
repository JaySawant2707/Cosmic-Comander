using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VerticalDoor : MonoBehaviour
{
    private static readonly int OpenHash = Animator.StringToHash("Open");
    Animator animator;
    [SerializeField] private bool startOpen = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.SetBool(OpenHash, startOpen);
    }

    public void OpenDoor()
    {
        animator.SetBool(OpenHash, true);
    }

    public void CloseDoor()
    {
        animator.SetBool(OpenHash, false);
    }
}
