using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
/*TODO
 * Dash
 * Grapple To/From
 * Wall run
 * Wall jump
 * Slide
 * Crouch
 * Mantle
 * 
 * Refactor to state machine format with override setups
 * Option for spherecast or raycast ground check
 * 
 * 
 * BUGS
 * Slope
 * Modifier for coyote time jumps
 * Push off from objects
 * 
 */

[RequireComponent(typeof(Rigidbody))]
public class FPSCharacterController : MonoBehaviour
{
    public enum MovementType
    {
        /// <summary>
        /// Movement is smooth and does not take time to reach some max speed value
        /// </summary>
        Instant,
        /// <summary>
        /// Movement initially begins slow and ramps up to some max value at which it is capped
        /// </summary>
        Acceleration
    }
    [Tooltip("Can be 'Instant' or 'Acceleration'\n\n" +
        "Using Instant, Movement is smooth and does not take time to reach some max speed value\n\n" +
        "Using Acceleration, Movement initially begins slow and ramps up to some max value at which it is capped")]
    public MovementType movementType;
    [Tooltip("Whether the player should stop immediately or slow to a stop when there is no input")]
    public bool instantStop = true;

    [Header("General")]
    public float moveSpeed = 100f;
    public float jumpModifier = 100f;
    [Tooltip("The maximum horizontal velocity the player can ever move at.")]
    public float maxSpeed = 5f;
    [Tooltip("Multiplier of moveSpeed when sprinting")]
    public float sprintMultiplier = 1.2f;
    [Tooltip("Allows sprinting in any direction")]
    public bool multiDirSprint = false;
    public float slopeCheckDistance = 1f;
    public float maxSlopeAngle = 60f;

    [Header("Ground Checks")]
    public Transform groundPoint;
    public float checkRadius = 1f;
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
    [Tooltip("The force applied to jumps past the normal first jump. If this is 0 the minJumpForce will be applied instead" +
        "Typically, this should be higher than the standard jump force to counteract your falling speed.")]
    public float multiJumpForce = 200f;
    [Tooltip("Whether the player can hold the jump button to jump longer on multi jumps.")]
    public bool extendMultiJumps = false;
    [Tooltip("Resets player's y velocity on landing so that they don't bounce when hitting the ground")]
    public bool lockOnLanding = false;

    [Header("Slopes")]
    [Tooltip("The downward force applied to keep the player on a slope. If you find your player bouncing on a slope, try applying more force here")]
    public float slopePush = 500f;

    private Rigidbody rb;
    private Vector3 movementDir;
    private bool isGrounded = true;
    private bool jumping = false;
    private float airTime = 0f;
    private bool falling = false;
    private Vector3 horizontalVelocityCheck;
    private int currentJumps = 0;
    private bool wasGrounded;
    private bool shouldJump = false;
    private bool sprinting = false;
    private RaycastHit slopeHit;
    private bool onSlope = false;
    private bool canJump = true;
    private float currentCoyoteTime = 0f;
    private float coyoteJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    public void Update()
    {
        GroundCheck();
        SlopeCheck();
        //Debug.Log(falling);
    }

    private void HandleJump()
    {
        if (airTime <= jumpDuration && shouldJump)
        {
            rb.AddForce(Vector3.up * jumpModifier * Time.deltaTime, ForceMode.Impulse);
            airTime += Time.deltaTime;
        }
        else
        {
            if (!isGrounded)
                falling = true;
        }

        if (falling)
        {
            rb.AddForce(Vector3.down * fallForce * Time.deltaTime, ForceMode.Force);
        }
    }

    private void HandleMovement()
    {
        if (new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude > maxSpeed)
        {
            horizontalVelocityCheck = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized * maxSpeed;
            horizontalVelocityCheck.y = rb.velocity.y;
            rb.velocity = horizontalVelocityCheck;
        }

        if (movementType == MovementType.Instant)
        {
            if (isGrounded)
            {
                if (movementDir != Vector3.zero)
                {
                    if (onSlope)
                    {
                        if (multiDirSprint)
                        {
                            rb.AddRelativeForce(GetSlopeMoveDir() * (sprinting ? moveSpeed * sprintMultiplier : moveSpeed) * Time.deltaTime, ForceMode.Impulse);
                        }
                        else
                        {
                            rb.AddRelativeForce(GetSlopeMoveDir() * (sprinting && movementDir.z > 0 ? moveSpeed * sprintMultiplier : moveSpeed) * Time.deltaTime, ForceMode.Impulse);
                        }
                    }
                    else
                    {
                        if (multiDirSprint)
                            rb.AddRelativeForce(movementDir * (sprinting ? moveSpeed * sprintMultiplier : moveSpeed) * Time.deltaTime, ForceMode.Impulse);
                        else
                            rb.AddRelativeForce(movementDir * (sprinting && movementDir.z > 0 ? moveSpeed * sprintMultiplier : moveSpeed) * Time.deltaTime, ForceMode.Impulse);
                    }
                }
                else
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }

                //On the ground we horizontal velocity every frame so that we are moving with a constant force
                //rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            else
            {
                if (movementDir != Vector3.zero)
                {
                    rb.AddRelativeForce(movementDir * inAirMoveSpeed * Time.deltaTime, ForceMode.Force);
                }
                else
                {
                    float newX = rb.velocity.x * Time.deltaTime;
                    float newZ = rb.velocity.z * Time.deltaTime;
                    rb.velocity = new Vector3(newX, rb.velocity.y, newZ);
                }
            }
        }
        else
        {
            if (movementDir != Vector3.zero && rb.velocity.magnitude < maxSpeed)
            {
                rb.AddRelativeForce(movementDir * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                if (movementDir != Vector3.zero)
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                rb.velocity *= Time.deltaTime;
            }
        }

    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundPoint.position, checkRadius, groundLayer);

        //checks if we are grounded this frame after we were not last frame
        if (isGrounded && !wasGrounded)
        {
            onGrounded.Invoke();
            jumping = false;
            currentJumps = 0;
            falling = false;
            airTime = 0f;
            currentCoyoteTime = 0f;
            canJump = true;
            if (lockOnLanding)
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

        if (!isGrounded && !jumping)
        {
            currentCoyoteTime += Time.deltaTime;
            if (currentCoyoteTime <= coyoteTime)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }

        wasGrounded = isGrounded;
    }

    private void SlopeCheck()
    {
        if (Physics.Raycast(groundPoint.position, Vector3.down, out slopeHit, slopeCheckDistance))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            onSlope = angle < maxSlopeAngle && angle != 0;
        }
        else onSlope = false;

        if (onSlope)
        {
            //this is to keep us from bobbing on the slope
            if (rb.velocity.y > 0f)
            {
                rb.AddForce(Vector3.down * (sprinting ? slopePush * sprintMultiplier : slopePush), ForceMode.Force);
            }
        }

        //rb.useGravity = !onSlope;
    }

    private Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(movementDir, slopeHit.normal);
    }

    public void OnDrawGizmosSelected()
    {
        if (groundPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundPoint.position, checkRadius);
        }
    }

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDir = context.ReadValue<Vector3>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprinting = true;
        }

        if (context.canceled)
        {
            sprinting = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //when we press jump, if we are jumping only then do we increase our jump count. This is so that the first jump does not count for our total
            if (jumping || falling && currentCoyoteTime > coyoteTime)
            {
                currentJumps++;
                if (currentJumps <= jumpAmount)
                {
                    //important to clear our current y velocity before the extra jump otherwise you can stack the velocity for multiple jumps
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    Debug.Log("Called");
                    rb.AddForce(Vector3.up * (multiJumpForce == 0f ? minJumpForce : multiJumpForce), ForceMode.Impulse);
                    shouldJump = extendMultiJumps;
                }
            }
            
            if (canJump)
            {
                Debug.Log("Called normal");
                rb.AddForce((Vector3.up * (falling && coyoteModifier != 0 ? minJumpForce * coyoteModifier : minJumpForce)) + rb.velocity, ForceMode.Impulse);
                jumping = true;
                canJump = false;
                shouldJump = extendJumps;
            }
        }

        if (context.canceled)
        {
            falling = true;
            shouldJump = false;
        }
    }
    #endregion
}
