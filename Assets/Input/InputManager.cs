using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputActions;

public class InputManager : Singleton<InputManager>, IPlayerActions
{
    public InputActions inputActions { get; private set; }

    #region Events
    public event Action<Vector2> Move = delegate { };
    public event Action Jump = delegate { };
    #endregion

    public Vector2 MoveDirection => inputActions.Player.Move.ReadValue<Vector2>();
    //public Vector2 AimDirection => inputActions.Player.Aim.ReadValue<Vector2>();


    private protected override void Awake()
    {
        base.Awake();
        CreatePlayerActions();
    }

    void CreatePlayerActions()
    {
        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump.Invoke();
    }


}
