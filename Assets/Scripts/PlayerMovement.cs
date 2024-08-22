using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerControls playerControls;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private const int groundDrag = 6;
    private const int airDrag = 0;

    [Header("Movement")]
    [SerializeField]
    private float speed;

    private Vector2 moveInput;

    [Header("Ground Check")]
    [SerializeField]
    private float playerHeight;

    [SerializeField]
    private LayerMask groundLayer;
    private bool isGrounded = true;

    public Vector3 GetVelocity => rb.velocity;
    public bool IsGrounded => isGrounded;

    [SerializeField]
    private Transform cameraPosition;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();

        //if (photonView.IsMine)
        {
            playerControls = playerManager.playerControls;


            playerControls.Player.Move.performed += ctx => Move(ctx);
            playerControls.Player.Move.canceled += ctx => Stop();
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Stop() 
    {
        moveInput = Vector2.zero;
    }

    private void MovePlayer()
    {
        if (moveInput == Vector2.zero && rb.velocity.magnitude == 0f)
            return;

        moveDirection = cameraPosition.right * moveInput.x + cameraPosition.forward * moveInput.y;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    private void SpeedLimit()
    {
        if (rb.velocity.magnitude < speed)
            return;

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -speed, speed));
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position + new Vector3(0, playerHeight * 0.5f, 0), Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);
    }

    private void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void Update()
    {
        GroundCheck();
        ControlDrag();
    }

    public void FixedUpdate()
    {
        MovePlayer();
        SpeedLimit();
    }

    private void OnDisable()
    {
        //if (photonView.IsMine)
            playerControls.Player.Move.performed -= ctx => Move(ctx);
            playerControls.Player.Move.canceled -= ctx => Stop();
    }
}
