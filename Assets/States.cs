//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharacterState : MonoBehaviour
//{
//    //public static CharacterState Instance;

//    //public CharacterState()
//    //{
//    //    Init();
//    //}

//    //public virtual void Init()
//    //{

//    //}

//    public virtual void OnStateEnter()
//    {

//    }
//    public virtual void OnStateUpdate()
//    {

//    }
//    public virtual void OnStateExit()
//    {

//    }
//}

//public class MoveState : CharacterState
//{
//    public static MoveState Instance;

//    public MoveState()
//    {
//        Init();
//    }

//    public virtual void Init()
//    {
//        Instance = this;
//    }

//    public override void OnStateEnter()
//    {

//    }

//    public override void OnStateUpdate()
//    {
//        Debug.Log("Moving in normal");
//    }

//    public override void OnStateExit()
//    {

//    }
//}

//public class CustomMoveState : MoveState
//{
//    public override void Init()
//    {
//        Instance = this;
//    }

//    public override void OnStateUpdate()
//    {
//        Debug.Log("Moving in custom");
//    }
//}

//public class IdleState : CharacterState
//{
//    public static IdleState Instance;

//    public override void OnStateEnter()
//    {

//    }

//    public override void OnStateUpdate()
//    {
//        Debug.Log("Idle");
//    }

//    public override void OnStateExit()
//    {

//    }
//}