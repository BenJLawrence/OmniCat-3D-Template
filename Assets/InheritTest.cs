using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritTest : StateSingleton
{
    private void Start()
    {
        MoveStateInstance = new CustomMove("CustomMove");
    }

    public class CustomMove : MoveState
    {
        public CustomMove(string _name) : base(_name)
        {

        }

        public override void OnStateUpdate()
        {
            Debug.Log("Custom Move");
        }
    }

}
