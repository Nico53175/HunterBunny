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
        
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        Vector3 forwardDirection = transform.forward;
        float speedInForwardDirection = Vector3.Dot(velocity, forwardDirection);

        Debug.Log(speedInForwardDirection);
        animator.SetFloat("moveSpeed", Mathf.Abs(speedInForwardDirection));
        animator.SetBool("isGrounded", playerMovement.isGrounded);
        animator.SetBool("isRunning", playerMovement.isRunning);
        animator.SetBool("isWalking", playerMovement.isWalking);
    }
}
