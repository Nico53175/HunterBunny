using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float runMultiplier = 1.5f;

    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isWalking = false;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckIfGrounded();
        Move();
        Jump();
    }

    void Move()
    {
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the current speed based on whether the player is running or walking
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) && moveZ > 0) ? speed * runMultiplier : speed;

        // Determine the forward movement direction based on the camera's forward direction
        Vector3 forwardMovement = Camera.main.transform.forward * moveZ;
        forwardMovement.y = 0; // Ensure the movement is horizontal
        forwardMovement.Normalize(); // Normalize to ensure consistent speed regardless of the camera's pitch

        // Set the boolean flags based on the movement direction and speed
        isWalking = moveZ != 0 && !Input.GetKey(KeyCode.LeftShift);
        isRunning = moveZ != 0 && Input.GetKey(KeyCode.LeftShift);

        // Apply the calculated velocity to the Rigidbody
        rb.velocity = new Vector3(forwardMovement.x * currentSpeed, rb.velocity.y, forwardMovement.z * currentSpeed);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }

    void CheckIfGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;
        isGrounded = Physics.Raycast(ray, out hit, groundCheckDistance, groundLayer) ? true : false;
    }
}
