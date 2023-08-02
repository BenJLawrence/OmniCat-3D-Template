using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CharacterStates : State<CharacterState>
{
    public static readonly State<CharacterState> Moving = new CharacterStateLibrary.MoveState();
    [DefaultState] public static readonly State<CharacterState> Idle = new CharacterStateLibrary.IdleState();
    public static readonly State<CharacterState> Falling = new CharacterStateLibrary.FallingState();
    public static readonly State<CharacterState> Sprinting = new CharacterStateLibrary.SprintState();
    public static readonly State<CharacterState> Jumping = new CharacterStateLibrary.JumpState();
    public static readonly State<CharacterState> OnSlope = new CharacterStateLibrary.SlopeState();
    public static readonly State<CharacterState> AirJump = new CharacterStateLibrary.AirJumpingState();
}

/// <summary>
/// In the case of Both, the ray and sphere must return 
/// </summary>
public enum GroundCheckType
{
    Sphere,
    Raycast,
    Both,
    Either,
}

public class CharacterController : StatefulObject<CharacterState>
{
    [Header("General")]
    public float moveSpeed = 100f;
    public float jumpModifier = 100f;
    [Tooltip("The maximum in air horizontal velocity the player can ever move at.")]
    public float maxInAirSpeed = 5f;
    [Tooltip("Multiplier of moveSpeed when sprinting")]
    public float sprintMultiplier = 1.2f;
    [Tooltip("Allows sprinting in any direction")]
    public bool multiDirSprint = false;
    public float slopeCheckDistance = 1f;
    public float maxSlopeAngle = 60f;

    [Header("Ground Checks")]
    public GroundCheckType groundCheckType;
    public Transform groundPoint;
    public float checkRadius = 1f;
    public float checkDistance = 1f;
    public LayerMask groundLayer;
    public UnityEvent onGrounded = new UnityEvent();

    [Header("In Air and Jumps")]
    [Tooltip("The amount of time the player can jump after having been grounded")]
    public float coyoteTime = .2f;
    public float coyoteModifier = 1.2f;
    [Tooltip("Whether the player can hold jump to increase jump height")]
    public bool extendJumps = true;
    public bool multipleJumps = true;
    public int jumpAmount = 2;
    public float inAirMoveSpeed = 100f;
    public float jumpDuration = .7f;
    public float fallForce = 100f;
    public float minJumpForce = 100f;
    public bool instantAirStop = false;
    [Range(0.001f, 0.999f)]
    [Tooltip("Used to slow down in air movement towards zero when there is no input. The smaller the number, the faster velocity will approach zero")]
    public float slowDown = .2f;
    [Tooltip("The force applied to jumps past the normal first jump. If this is 0 the minJumpForce will be applied instead" +
        "Typically, this should be higher than the standard jump force to counteract your falling speed.")]
    public float multiJumpForce = 200f;
    [Tooltip("Whether the player can hold the jump button to jump longer on multi jumps.")]
    public bool extendMultiJumps = false;
    [Tooltip("Resets player's y velocity on landing so that they don't bounce when hitting the ground")]
    public bool lockOnLanding = false;

    internal Vector3 movementDir;
    internal bool isGrounded = true;
    private RaycastHit groundHit;
    private bool wasGrounded;
    internal bool jumpKeyDown = false;
    internal bool canJump = true;
    internal int currentJumpAmount = 0;
    internal UnityEvent onAirJump = new UnityEvent();

    private void Start()
    {
       
    }

    protected override void Update()
    {
        base.Update();
        GroundCheck();
        //UnityEngine.Debug.Log(state);
        //Debug.Log(currentJumpAmount);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void GroundCheck()
    {
        switch (groundCheckType)
        {
            case GroundCheckType.Raycast:
                isGrounded = Physics.Raycast(groundPoint.position, Vector3.down, out groundHit, checkDistance, groundLayer);
                break;
            case GroundCheckType.Sphere:
                isGrounded = Physics.CheckSphere(groundPoint.position, checkRadius, groundLayer);
                break;
            case GroundCheckType.Both:
                isGrounded = Physics.Raycast(groundPoint.position, Vector3.down, out groundHit, checkDistance, groundLayer) &
                    Physics.CheckSphere(groundPoint.position, checkRadius, groundLayer);
                break;
            case GroundCheckType.Either:
                isGrounded = Physics.Raycast(groundPoint.position, Vector3.down, out groundHit, checkDistance, groundLayer) |
                    Physics.CheckSphere(groundPoint.position, checkRadius, groundLayer);
                break;
        }

        //checks if we are grounded this frame after we were not last frame
        if (isGrounded && !wasGrounded)
        {
            onGrounded.Invoke();
            currentJumpAmount = 0;
        }

        wasGrounded = isGrounded;
    }


    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDir = context.ReadValue<Vector3>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isGrounded && currentJumpAmount < jumpAmount)
        {
            Debug.Log("Called air jump");
            jumpKeyDown = true;
            onAirJump.Invoke();
        }
            
        if (context.performed && isGrounded)
        {
            jumpKeyDown = true;
            ChangeState(CharacterStates.Jumping);
        }

        if (context.canceled)
        {
            jumpKeyDown = false;
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (groundPoint != null)
        {
            Gizmos.color = Color.green;
            switch (groundCheckType)
            {
                case GroundCheckType.Raycast:
                    Gizmos.DrawRay(groundPoint.position, Vector3.down * checkDistance);
                    break;
                case GroundCheckType.Sphere:
                    Gizmos.DrawWireSphere(groundPoint.position, checkRadius);
                    break;
                case GroundCheckType.Both:
                case GroundCheckType.Either:
                    Gizmos.DrawWireSphere(groundPoint.position, checkRadius);
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(groundPoint.position, Vector3.down * checkDistance);
                    break;
            }
        }
    }
}
