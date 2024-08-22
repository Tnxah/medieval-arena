using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerControls playerControls;

    [SerializeField]
    private Transform cameraPosition;

    private float rotateDirection;
    [SerializeField, Range(10f, 100f)]
    private float rotationSpeed;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        //if (photonView.IsMine)
        {
            playerControls = playerManager.playerControls;


            playerControls.Player.Look.performed += ctx => rotateDirection = ctx.ReadValue<float>();
            playerControls.Player.Look.canceled += ctx => rotateDirection = 0;
        }
    }

    private void Update()
    {
        if (rotateDirection != 0)
        { 
            cameraPosition.RotateAround(transform.position, Vector3.up, rotateDirection * rotationSpeed * Time.deltaTime);
        }

        if (Camera.main.transform.position != cameraPosition.position)
        {
            Camera.main.transform.position = cameraPosition.position;
            Camera.main.transform.LookAt(transform);
        }
    }

    private void OnDisable()
    {
        playerControls.Player.Look.performed -= ctx => rotateDirection = ctx.ReadValue<float>();
        playerControls.Player.Look.performed -= ctx => rotateDirection = 0;
    }
}
