using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GroundState : State<PlayerController>
{
    public float Speed { get; set; } = 10f;
    public float JumpForce { get; set; } = 5f;

    bool hasRunningActivated = false;

    private protected override void OnEnter()
    {
        _context.CurrentState = GetType().Name;
        InputManager.Instance.Jump += Jumping;
        InputManager.Instance.Run += SetIsRunning;
    }

    private protected override void OnExit()
    {
        InputManager.Instance.Jump -= Jumping;
        InputManager.Instance.Run -= SetIsRunning;
    }

    private protected override void OnFixedUpdate()
    {
        Movement();
    }

    private protected override void OnUpdate()
    {
        Running();
    }

    private protected override State<PlayerController> GetTransition()
    {
        if (!_context.IsGrounded)
        {
            return _context.AirState;
        }

        return null;
    }

    void Movement()
    {
        Vector2 vm = InputManager.Instance.MoveDirection;
        Debug.Log(vm);
        vm.y = 0f;
        _context.Rb.linearVelocityX = vm.x * Speed;
    }

    void Jumping()
    {
        if (_context.IsGrounded)
        {
            _context.Rb.linearVelocity += (Vector2.up * JumpForce);
            _context.IsGrounded = false;
        }
    }

    void SetIsRunning(bool val)
    {
        _context.IsRunning = val;
    }

    void Running()
    {
        const float SPEED_MULTIPLIER = 2f;

        Debug.Log($"Speed: {Speed}");

        if (_context.IsRunning && !hasRunningActivated)
        {
            hasRunningActivated = true;
            Speed *= SPEED_MULTIPLIER;
            _context.Sr.color = Color.red;
        }

        if (!_context.IsRunning && hasRunningActivated)
        {
            hasRunningActivated = false;
            Speed /= SPEED_MULTIPLIER;
            _context.Sr.color = Color.white;
        }
    }

}
