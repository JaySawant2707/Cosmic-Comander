using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; set; }
    public bool JumpPressed { get; set; }
    public bool AttackPressed { get; set; }
    public bool ShootPressed { get; set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            JumpPressed = true;

        if (context.canceled)
            JumpPressed = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            AttackPressed = true;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
            ShootPressed = true;

        if (context.canceled)
            ShootPressed = false;
    }
}