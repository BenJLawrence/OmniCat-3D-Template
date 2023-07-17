using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : OmniEnum<State, CharacterState>
{
    public readonly static OmniEnum<State, CharacterState> Moving = StateSingleton.MoveStateInstance;

    public readonly static OmniEnum<State, CharacterState> Idle = StateSingleton.IdleStateInstance;
}

[RequireComponent(typeof(StateSingleton))]
public class CharacterController : MonoBehaviour
{
    private OmniEnum<State, CharacterState> state;

    void Start()
    {
        state = State.Idle;
        State.AddField(new OmniEnum<State, CharacterState>("Crouch", StateSingleton.CrouchStateInstance));
    }

    void Update()
    {
        HandleState();
    }

    public void ChangeState(State newState)
    {
        //call the exit on the current state
        state.data.OnStateExit();

        //change the state
        state = newState;

        //call enter on the new state in IStates
        newState.data.OnStateEnter();
    }

    private void HandleState()
    {
        //Debug.Log(state.stateData);
        state.data.OnStateUpdate();

        if (state == State.Idle)
        {
            
        }
        else if (state == State.Moving)
        {
            
        }
    }
}
