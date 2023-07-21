using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    public void OnStateEnter<T>(StatefulObject<T> self) where T : IState;
    public void OnStateUpdate<T>(StatefulObject<T> self) where T : IState;
    public void OnStateExit<T>(StatefulObject<T> self) where T : IState;
}

public class State<T> : OmniEnum<State<T>, T> where T : IState
{
    public static implicit operator State<T>(T data)
    {
        var x = new State<T>();
        x.data = data;
        return x;
    }
    public void OverrideState(T newData)
    {
        data = newData;
    }
}


public class StatefulObject<T> : MonoBehaviour where T : IState
{
    public static StatefulObject<T> Instance;

    protected State<T> state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    protected virtual void Update()
    {
        HandleState(this);
    }

    public void ChangeState<ControllerType>(State<T> newState, StatefulObject<ControllerType> self) where ControllerType : IState
    {
        //call the exit on the current state
        state.data.OnStateExit(self);

        //change the state
        state = newState;

        //call enter on the new state in IStates
        newState.data.OnStateEnter(self);
    }

    protected virtual void HandleState<ControllerType>(StatefulObject<ControllerType> self) where ControllerType : IState
    {
        state.data.OnStateUpdate(self);
    }
}
