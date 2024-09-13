using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject message;
    public Unlock_Board board;

    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!board.isOpen)
        { message.SetActive(true); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!board.isOpen)
        { message.SetActive(false); }
    }
}
