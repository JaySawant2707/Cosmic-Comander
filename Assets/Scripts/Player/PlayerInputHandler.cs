using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; set; }

    // One-shot flag: set true on press, consumed (set false) by PlayerController after use.
    // Used for jump buffering — detects the moment the button is pressed.
    public bool JumpPressed { get; set; }

    // FIX 2: Continuous flag: true while jump button is physically held down, false on release.
    // Used by ApplyBetterGravity to enable variable jump height (short hop vs full jump).
    // Previously JumpPressed was serving both roles, which broke variable jump height entirely.
    public bool JumpHeld { get; private set; }

    public bool AttackPressed { get; set; }
    public bool ShootPressed { get; set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpPressed = true;  // one-shot, consumed by PlayerController
            JumpHeld = true;     // held state begins
        }

        if (context.canceled)
        {
            // FIX 2: Only JumpHeld goes false on release.
            // JumpPressed is intentionally NOT cleared here — PlayerController clears it
            // after consuming it, so the jump buffer window works correctly.
            JumpHeld = false;
        }
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