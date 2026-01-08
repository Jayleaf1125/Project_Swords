using UnityEngine;

// Credits: Samson
public class StateMachine<T> where T : MonoBehaviour
{
    public T Context { get; private set; }
    public State<T> CurrentState { get; private set; }
    public bool HasTransitionedThisFrame { get; private set; } = false;

    public StateMachine(T context)
    {
        Context = context;
    }

    /// <summary>
    /// Changes the current state to the new state.
    /// If the new state is the same as the current state, it will not change unless forceChangeToSameState is true.
    /// </summary>
    public void ChangeState(State<T> newState, bool forceChangeToSameState = false)
    {
        if (HasTransitionedThisFrame)
            return;

        if (newState == null)
            return;

        if (CurrentState == newState && !forceChangeToSameState)
            return;

        HasTransitionedThisFrame = true;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Update()
    {
        HasTransitionedThisFrame = false;
        CurrentState?.Update();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }

    public void ExitCurrentState()
    {
        CurrentState?.Exit();
        CurrentState = null;
    }

    public void Destroy()
    {
        CurrentState?.Destroy();
    }
}

