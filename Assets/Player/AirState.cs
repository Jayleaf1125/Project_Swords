using UnityEngine;

public class AirState : State<PlayerController>
{
    private protected override void OnEnter()
    {
        _context.CurrentState = GetType().Name;
    }

    private protected override void OnExit()
    {

    }

    private protected override void OnFixedUpdate()
    {
        Vector2 vm = InputManager.Instance.MoveDirection;
        vm.y = 0f;
        _context.Rb.linearVelocityX = vm.x * _context.GroundState.Speed;
    }

    private protected override void OnUpdate()
    {

    }

    private protected override State<PlayerController> GetTransition()
    {
        if (_context.IsGrounded)
        {
            return _context.GroundState;
        }

        return null;
    }
}
