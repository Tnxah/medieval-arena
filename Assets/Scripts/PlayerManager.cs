using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerRotation playerRotation;
    public PlayerCamera playerCamera;
    public PlayerCombat playerCombat;

    public PlayerControls playerControls { private set; get; }

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }
}
