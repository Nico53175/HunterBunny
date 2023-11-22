using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private Rigidbody rb;

    private void Start()
    {        
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateAnimations();               
    }

    void UpdateAnimations()
    {   
        Vector3 velocity = rb.velocity;
        Vector3 forwardDirection = transform.forward;
        float speedInForwardDirection = Vector3.Dot(velocity, forwardDirection);
        animator.SetFloat("moveSpeed", Mathf.Abs(speedInForwardDirection));
        animator.SetBool("isGrounded", playerController.isGrounded);
        animator.SetBool("isRunning", playerController.isRunning);
        animator.SetBool("isWalking", playerController.isWalking);
    }
}
