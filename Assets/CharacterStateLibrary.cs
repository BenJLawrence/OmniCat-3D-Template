using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterState : IState
{
    protected CharacterController controller;
    protected Rigidbody rb;
    public virtual void OnStateEnter<T>(StatefulObject<T> self) where T : IState
    {
        controller = self.GetComponent<CharacterController>();
        rb = controller.GetComponent<Rigidbody>();
    }
    public abstract void OnStateUpdate<T>(StatefulObject<T> self) where T: IState;
    public abstract void OnStateExit<T>(StatefulObject<T> self) where T: IState;
    public abstract void OnStateFixedUpdate<T>(StatefulObject<T> self) where T : IState;
}

public class CharacterStateLibrary
{
    public class MoveState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            controller = self.GetComponent<CharacterController>();
            rb = controller.GetComponent<Rigidbody>();
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            rb.AddRelativeForce(controller.movementDir * controller.moveSpeed * Time.deltaTime, ForceMode.Impulse);

            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            if (controller.movementDir == Vector3.zero)
            {
                controller.ChangeState(CharacterStates.Idle);
            }
        }
    }

    public class IdleState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            controller = self.GetComponent<CharacterController>();
            rb = controller.GetComponent<Rigidbody>();

            rb.velocity = Vector3.zero;
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {

        }

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            if (controller.movementDir != Vector3.zero)
            {
                controller.ChangeState(CharacterStates.Moving);
            }
        }
    }

    public class FallingState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            base.OnStateEnter(self);
            controller.onGrounded.AddListener(Transition);
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            rb.AddRelativeForce(controller.movementDir * controller.inAirMoveSpeed * Time.deltaTime, ForceMode.Force);
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            
        }

        private void Transition()
        {
            controller.ChangeState(CharacterStates.Idle);
        }
    }

    public class SprintState : CharacterState
    {
        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            
        }
    }

    public class JumpState : CharacterState
    {
        private float airTime = 0f;

        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            controller = self.GetComponent<CharacterController>();
            rb = controller.GetComponent<Rigidbody>();
            airTime = 0f;

            rb.AddForce((Vector3.up * controller.minJumpForce) + rb.velocity, ForceMode.Impulse);
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            if (airTime < controller.jumpDuration && controller.jumpKeyDown)
            {
                rb.AddForce(Vector3.up * controller.jumpModifier * Time.deltaTime, ForceMode.Impulse);
                airTime += Time.deltaTime;
            }
            else
            {
                controller.ChangeState(CharacterStates.Falling);
            }
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {

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

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
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

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            throw new NotImplementedException();
        }
    }
}
