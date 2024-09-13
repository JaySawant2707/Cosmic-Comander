using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Player_Input PI;

    private bool canPress = false;

    // Start is called before the first frame update
    void Start()
    {
        PI = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Input>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canPress)
        PI.Jump();

        canPress = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canPress = true;
    }
}
