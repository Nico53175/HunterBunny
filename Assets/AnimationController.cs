using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerMovement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerController>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        float moveSpeed = GetComponent<Rigidbody>().velocity.magnitude;

        animator.SetFloat("moveSpeed", moveSpeed);
        animator.SetBool("isGrounded", playerMovement.isGrounded);
        animator.SetBool("isRunning", playerMovement.isRunning);
        animator.SetBool("isWalking", playerMovement.isWalking);
    }
}
