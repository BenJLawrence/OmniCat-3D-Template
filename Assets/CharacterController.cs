using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStates : State<CharacterState>
{
    public static readonly State<CharacterState> Moving = new CharacterStateLibrary.MoveState();
    public static readonly State<CharacterState> Idle = new CharacterStateLibrary.IdleState();
    public static readonly State<CharacterState> Falling = new CharacterStateLibrary.FallingState();
    public static readonly State<CharacterState> Sprinting = new CharacterStateLibrary.SprintState();
    public static readonly State<CharacterState> Jumping = new CharacterStateLibrary.JumpState();
    public static readonly State<CharacterState> OnSlope = new CharacterStateLibrary.SlopeState();
}

public class CharacterController : StatefulObject<CharacterState>
{
    private Vector3 movementDir;

    private void Start()
    {
        state = CharacterStates.Idle;
        ChangeState(CharacterStates.Moving, this);
    }

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDir = context.ReadValue<Vector3>();
        ChangeState(CharacterStates.Moving, this);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
    #endregion
}
