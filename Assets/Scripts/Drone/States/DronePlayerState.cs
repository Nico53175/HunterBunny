using UnityEngine;

public class DronePlayerState : IDroneState
{
    private DroneStateManager drone;
    private DroneStatePlayerSO dronePlayerSettings;

    // Camera
    private Camera mainCamera;
    private Camera droneCamera;

    // Player Settings
    private CameraFollowPlayer cameraFollowPlayerScript;
    private PlayerController playerControllerScript;

    // Drone Settings
    private float startSpeed;
    private float maxSpeed;
    private float acceleration;
    private float tiltIntensity;
    private float tiltSpeed;
    private float rotationSpeed;

    // Local Variables
    private float currentSpeed;

    // Transform
    private Transform droneTransform;
    public DronePlayerState(DroneStateManager drone)
    {
        this.drone = drone;
        dronePlayerSettings = drone.dronePlayerSettings;

        // Set Drone Settings from SO
        startSpeed = dronePlayerSettings.startSpeed;
        maxSpeed = dronePlayerSettings.maxSpeed;
        acceleration = dronePlayerSettings.acceleration;
        tiltIntensity = dronePlayerSettings.tiltIntensity;
        tiltSpeed = dronePlayerSettings.tiltSpeed;
        rotationSpeed = dronePlayerSettings.rotationSpeed;

        // Get needed Components
        droneTransform = drone.GetComponent<Transform>();
        droneCamera = drone.GetComponentInChildren<Camera>();
        cameraFollowPlayerScript = drone.playerTransform.GetComponent<CameraFollowPlayer>();
        playerControllerScript = drone.playerTransform.GetComponent<PlayerController>();

        // Set Variables 
        currentSpeed = startSpeed;
    }

    public void Enter()
    {
        cameraFollowPlayerScript.enabled = false;
        playerControllerScript.enabled = false;
        mainCamera = Camera.main;
        mainCamera.enabled = false;
        droneCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Execute()
    {
        DroneMovement();
    }

    public void Exit()
    {
        mainCamera.enabled = true;
        droneCamera.enabled = false;

        cameraFollowPlayerScript.enabled = true;
        playerControllerScript.enabled = true;
    }

    private void DroneMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = Input.GetKey(KeyCode.Space) ? 1 : 0; // Ascend when space is pressed
        float moveDown = Input.GetKey(KeyCode.LeftShift) ? -1 : 0; // Decend when shift is pressed
        float verticalSpeed = startSpeed / 2;

        if (currentSpeed < maxSpeed)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }

        // Calculate horizontal movement
        Vector3 horizontalMovement = droneTransform.right * moveHorizontal + droneTransform.forward * moveVertical;
        horizontalMovement.y = 0; // Ensure horizontal movement is parallel to the ground
        horizontalMovement.Normalize();

        // Apply horizontal movement
        droneTransform.position += horizontalMovement * currentSpeed * Time.deltaTime;

        // Apply vertical movement
        droneTransform.position += Vector3.up * (moveUp + moveDown) * verticalSpeed * Time.deltaTime;

        // Apply tilt based on horizontal movement only
        ApplyTilt(moveHorizontal, moveVertical);

        // Rotate based on mouse input
        RotateCamera();
    }
    private void ApplyTilt(float moveHorizontal, float moveVertical)
    {
        float tiltAroundZ = moveHorizontal * -tiltIntensity; // Roll tilt for horizontal movement
        float tiltAroundX = moveVertical * tiltIntensity; // Pitch tilt for forward/backward movement

        Quaternion targetRotation = Quaternion.Euler(tiltAroundX, droneTransform.eulerAngles.y, tiltAroundZ);
        droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }
    private void RotateCamera()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        Quaternion yawRotation = Quaternion.Euler(0, horizontalInput * rotationSpeed, 0);
        Quaternion pitchRotation = Quaternion.Euler(-verticalInput * rotationSpeed, 0, 0);

        droneTransform.rotation *= yawRotation * pitchRotation;
    }

}
