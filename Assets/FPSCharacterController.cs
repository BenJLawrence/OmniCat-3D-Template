using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FPSCharacterController : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float jumpModifier = 100f;
    public Transform groundPoint;
    public float checkRadius = 1f;
    public LayerMask groundLayer;
    public float inAirMoveSpeed = 100f;

    private Rigidbody rb;
    private Vector3 movementDir;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        HandleMovement();
    }

    public void Update()
    {
        GroundCheck();
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpModifier, ForceMode.Force);
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundPoint.position, checkRadius, groundLayer);
        Debug.Log(isGrounded);
    }

    private void HandleMovement()
    {
        if (isGrounded)
        {
            if (movementDir != Vector3.zero)
            {
                rb.AddRelativeForce(movementDir * moveSpeed * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                rb.velocity *= Time.deltaTime;
            }

            rb.velocity = Vector3.zero;
        }
        else
        {
            if (movementDir != Vector3.zero)
            {
                rb.AddRelativeForce(movementDir * inAirMoveSpeed * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                float newX = rb.velocity.x * Time.deltaTime;
                float newZ = rb.velocity.z * Time.deltaTime;
                rb.velocity = new Vector3(newX, rb.velocity.y, newZ);
            }

            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleJump();
        }
    }
    #endregion
}
