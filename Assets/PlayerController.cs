using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
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
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float currentSpeed = speed;

        isRunning = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runMultiplier;
            isRunning = true;
        }

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);

        if (moveDirection.magnitude > 0.1f)
        {
            if (isRunning)
            {
                isWalking = false;
            }
            else
            {
                isWalking = true;
                isRunning = false;
            }
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);
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
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, 0.6f, groundLayer);
    }
}