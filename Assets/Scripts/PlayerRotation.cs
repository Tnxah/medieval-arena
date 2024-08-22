using System.Collections;
using System.Collections.Generic;
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

    private const float gamepadRotationSmoothing = 500;

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

        direction = cameraPosition.right * inputRotation.x + cameraPosition.forward * inputRotation.y;

        if (direction.sqrMagnitude > 0.0f)
        {
            Quaternion newrotation = Quaternion.LookRotation(direction, Vector3.up);
            body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, newrotation, gamepadRotationSmoothing * Time.deltaTime);
            
        }
    }

    private void Update()
    {
        RotatePlayer();
    }

    private void OnDisable()
    {
        //if (photonView.IsMine)
        playerControls.Player.Rotate.performed -= ctx => Rotate(ctx);
    }
}
