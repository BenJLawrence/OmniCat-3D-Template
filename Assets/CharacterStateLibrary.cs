using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterState : IState
{
    public abstract void OnStateEnter<T>(StatefulObject<T> self) where T : IState;
    public abstract void OnStateUpdate<T>(StatefulObject<T> self) where T: IState;
    public abstract void OnStateExit<T>(StatefulObject<T> self) where T: IState;
}

public class CharacterStateLibrary
{
    public class MoveState : CharacterState
    {
        private CharacterController controller;
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            controller = self.GetComponent<CharacterController>();
            Debug.Log(controller.name, controller.gameObject);
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }

    public class IdleState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            Debug.Log("Exited Idle");
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }

    public class FallingState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }

    public class SprintState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }

    public class JumpState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }

    public class SlopeState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }

    public class CrouchState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }
}
