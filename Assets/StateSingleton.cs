using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class CharacterState
{
    public string name;
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}

public class StateSingleton : MonoBehaviour
{
    public static StateSingleton Instance;
    public static MoveState MoveStateInstance;
    public static IdleState IdleStateInstance;
    public static CrouchState CrouchStateInstance;

    private List<CharacterState> stateList = new List<CharacterState>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        MoveStateInstance = new MoveState("Move");
        stateList.Add(MoveStateInstance);
        IdleStateInstance = new IdleState("Idlessssssssss");
        stateList.Add(IdleStateInstance);
        CrouchStateInstance = new CrouchState("Crouch");
    }

    public void Add(CharacterState instance)
    {
        stateList.Add(instance);
    }

    public class MoveState : CharacterState
    {
        public MoveState(string _name)
        {
            name = _name;
        }

        public override void OnStateEnter()
        {

        }

        public override void OnStateUpdate()
        {
            //Debug.Log("Moving in normal");
        }

        public override void OnStateExit()
        {

        }
    }

    public class IdleState : CharacterState
    {
        public IdleState(string _name)
        {
            name = _name;
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered");
        }

        public override void OnStateUpdate()
        {
            //Debug.Log("Idle");
        }

        public override void OnStateExit()
        {

        }
    }

    public class CrouchState : CharacterState
    {
        public CrouchState(string _name)
        {
            name = _name;
        }

        public override void OnStateEnter()
        {

        }

        public override void OnStateUpdate()
        {
            //Debug.Log("Idle");
        }

        public override void OnStateExit()
        {

        }
    }
}
