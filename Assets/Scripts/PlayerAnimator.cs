using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerManager manager;

    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
    }
    private void Update()
    {
        MovementControl(manager.playerMovement.GetVelocity, manager.playerMovement.IsGrounded, manager.playerRotation.Body);
    }

    private void Start()
    {
        manager.playerCombat.OnAttack += Attack;
    }

    private void MovementControl(Vector3 velocity, bool isGrounded, Transform body)
    {
        var direction = body.InverseTransformDirection(velocity);
        animator.SetFloat("velocityX", direction.x);
        animator.SetFloat("velocityZ", direction.z);
        animator.SetBool("isFalling", !isGrounded);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void OnDisable()
    {
        manager.playerCombat.OnAttack -= Attack;
    }
}
