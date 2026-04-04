using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    public Vector2 Move { get; private set; }

    private float lastJumpPressedTime = float.NegativeInfinity;

    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
            moveAction.action.performed += OnMove;
            moveAction.action.canceled += OnMove;
        }

        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJump;
        }
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.performed -= OnMove;
            moveAction.action.canceled -= OnMove;
            moveAction.action.Disable();
        }

        if (jumpAction != null)
        {
            jumpAction.action.performed -= OnJump;
            jumpAction.action.Disable();
        }
    }

    public bool HasBufferedJump(float jumpBufferTime)
    {
        return Time.time - lastJumpPressedTime <= jumpBufferTime;
    }

    public void ConsumeBufferedJump()
    {
        lastJumpPressedTime = float.NegativeInfinity;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            lastJumpPressedTime = Time.time;
        }
    }
}
