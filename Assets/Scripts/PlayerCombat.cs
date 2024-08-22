using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerControls playerControls;

    public event Action OnAttack;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        //if (photonView.IsMine)
        {
            playerControls = playerManager.playerControls;

            playerControls.Player.Attack.performed += ctx => Attack();
        }
    }

    private void Attack()
    {
        OnAttack?.Invoke();
    }

    private void OnDisable()
    {
        playerControls.Player.Attack.performed -= ctx => Attack();
    }
}
