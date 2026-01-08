using UnityEngine;

public class GroundState : State<PlayerController>
{
    public float Speed { get; set; } = 10f;
    public float JumpForce { get; set; } = 5f;

    private protected override void OnEnter()
    {
        _context.CurrentState = GetType().Name;
        InputManager.Instance.Jump += Jumping;
    }

    private protected override void OnExit()
    {
        InputManager.Instance.Jump -= Jumping;
    }

    private protected override void OnFixedUpdate()
    {
        Movement();
    }

    private protected override void OnUpdate()
    {

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

}
