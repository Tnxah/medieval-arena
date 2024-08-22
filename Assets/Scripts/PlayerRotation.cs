using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerControls playerControls;

    [SerializeField]
    private Transform cameraPosition;

    [SerializeField]
    private Transform body;

    private Vector2 inputRotation;
    private Vector3 direction;

    private const float rotationSmoothing = 500;

    private PlayerInput pi;

    public Transform Body => body;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        //if (photonView.IsMine)
        {
            playerControls = playerManager.playerControls;

            playerControls.Player.Rotate.performed += ctx => Rotate(ctx);
        }
    }

    private void Rotate(InputAction.CallbackContext context) 
    {
        inputRotation = context.ReadValue<Vector2>();
    }

    private void RotatePlayer()
    {
        if (inputRotation == Vector2.zero)
            return;

        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            direction = cameraPosition.right * inputRotation.x + cameraPosition.forward * inputRotation.y;

        } else if (pi.currentControlScheme.Equals("Keyboard and Mouse"))
        {
            Ray ray = Camera.main.ScreenPointToRay(inputRotation);
            Plane groundPlane = new Plane(Vector3.up, body.position);

            if (groundPlane.Raycast(ray, out var rayDistance))
            {
                direction = ray.GetPoint(rayDistance) - body.position;
            }
        }

        if (direction.sqrMagnitude > 0.0f)
        {
            Quaternion newrotation = Quaternion.LookRotation(direction, Vector3.up);
            body.rotation = Quaternion.RotateTowards(body.rotation, newrotation, rotationSmoothing * Time.deltaTime);
        }
    }

    private void Update()
    {
        RotatePlayer();
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        this.pi = pi;
    }

    private void OnDisable()
    {
        //if (photonView.IsMine)
        playerControls.Player.Rotate.performed -= ctx => Rotate(ctx);
    }
}
