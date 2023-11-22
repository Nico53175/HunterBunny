using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IEntityEventSubscriber
{
    private LayerMask groundLayer;
    private float groundCheckDistance;
    private float speed;
    private float jumpForce;
    private float sprintMultiplier;
    private int playerLevel;

    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isWalking = false;

    Rigidbody rb;

    [SerializeField] private PlayerSO player;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private DamageSystem damageSystem;

    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();

    private void Awake()
    {
        if (healthSystem != null)
        {
            healthSystem.Initialize(player.health, player.level, player.healthCurve, this);
        }

        if (damageSystem != null)
        {
            damageSystem.Initialize(player.health, player.level, player.damageCurve, this);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = player.groundLayer;
        groundCheckDistance = player.groundCheckDistance;
        speed = player.speed;
        jumpForce = player.jumpForce;
        sprintMultiplier = player.sprintMultiplier;
        playerLevel = player.level;
    }

    private void Update()
    {
        CheckIfGrounded();
        Move();
        Jump();
    }

    public void SubscribeToLevelUp(UnityAction<int> callback)
    {
        OnLevelUp.AddListener(callback);
    }

    public void UnsubscribeFromLevelUp(UnityAction<int> callback)
    {
        OnLevelUp.RemoveListener(callback);
    }

    public void LevelUp()
    {
        playerLevel++;
        OnLevelUp.Invoke(playerLevel);
    }

    void Move()
    {
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the current speed based on whether the player is running or walking
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) && moveZ > 0) ? speed * sprintMultiplier : speed;

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
