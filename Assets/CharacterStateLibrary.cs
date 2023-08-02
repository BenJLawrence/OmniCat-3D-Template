using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterState : IState
{
    protected CharacterController controller;
    protected Rigidbody rb;
    public virtual void OnStateStart<T>(StatefulObject<T> self) where T : IState
    {
        controller = self.GetComponent<CharacterController>();
        rb = controller.GetComponent<Rigidbody>();
    }
    public virtual void OnStateEnter<T>(StatefulObject<T> self) where T : IState
    {
        
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
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            if (controller.movementDir == Vector3.zero)
            {
                controller.ChangeState(CharacterStates.Idle);
            }

            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
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

    public class AirJumpingState : CharacterState
    {
        private float airTime = 0f;

        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            base.OnStateEnter(self);
            airTime = 0f;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce((Vector3.up * controller.multiJumpForce), ForceMode.Impulse);
            controller.currentJumpAmount++;
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

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            
        }
    }

    public class FallingState : CharacterState
    {
        private Vector3 horizontalVelocityCheck;
        private float reduction;

        public override void OnStateStart<T>(StatefulObject<T> self)
        {
            base.OnStateStart(self);

            controller.onAirJump.AddListener(DoAirJump);
            controller.onGrounded.AddListener(HandleGrounded);
        }

        public override void OnStateEnter<T>(StatefulObject<T> self)
        {
            base.OnStateEnter(self);
        }

        public override void OnStateExit<T>(StatefulObject<T> self)
        {
            
        }

        public override void OnStateFixedUpdate<T>(StatefulObject<T> self)
        {
            if (controller.movementDir != Vector3.zero)
            {
                rb.AddRelativeForce(controller.movementDir * controller.inAirMoveSpeed * Time.deltaTime, ForceMode.Force);
                reduction = controller.inAirMoveSpeed;

            }
            else if (controller.instantAirStop)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            else
            {
                //slow down over by time by multiplying with small numbers
                reduction *= controller.slowDown;
                rb.AddRelativeForce(controller.movementDir * reduction * Time.deltaTime, ForceMode.Force);
            }

            //handles the extra downward force when falling
            rb.AddForce(Vector3.down * controller.fallForce * Time.deltaTime, ForceMode.Force);
        }

        public override void OnStateUpdate<T>(StatefulObject<T> self)
        {
            //Velocity cap since when adding our in air force we could theoretically ramp speed forever
            if (new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude > controller.maxInAirSpeed)
            {
                horizontalVelocityCheck = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized * controller.maxInAirSpeed;
                horizontalVelocityCheck.y = rb.velocity.y;
                rb.velocity = horizontalVelocityCheck;
            }
        }

        protected void DoAirJump()
        {
            controller.ChangeState(CharacterStates.AirJump);
        }

        //Called when the player hits the ground
        public void HandleGrounded()
        {
            controller.currentJumpAmount = 0;
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
            Debug.Log("entered jump");
            controller = self.GetComponent<CharacterController>();
            rb = controller.GetComponent<Rigidbody>();
            airTime = 0f;
            
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce((Vector3.up * controller.minJumpForce), ForceMode.Impulse);
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
