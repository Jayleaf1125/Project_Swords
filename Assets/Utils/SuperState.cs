using UnityEngine;


// Credits: Samson
public abstract class SuperState<T> : State<T> where T : MonoBehaviour
{
    public StateMachine<T> SubStateMachine { get; private set; }
    public abstract State<T> InitialSubState { get; }

    private protected override void OnInit()
    {
        SubStateMachine = new StateMachine<T>(_context);
        InitializeSubStates();
    }

    private protected abstract void InitializeSubStates();

    public override void Destroy()
    {
        SubStateMachine.Destroy();
        base.Destroy();
    }

    public override void Enter()
    {
        base.Enter();
        SubStateMachine.ChangeState(InitialSubState, true);
    }

    public override void Exit()
    {
        SubStateMachine.ExitCurrentState();
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        State<T> transitionState = GetSubStateTransition();
        if (transitionState != null && !SubStateMachine.HasTransitionedThisFrame)
        {
            SubStateMachine.ChangeState(transitionState);
            return;
        }
        SubStateMachine.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        SubStateMachine.FixedUpdate();
    }

    /// <summary>
    /// Polls for a sub-state transition. If a sub-state transition is returned, the SubStateMachine will handle the transition.
    /// </summary>
    private protected virtual State<T> GetSubStateTransition() => null;
}

