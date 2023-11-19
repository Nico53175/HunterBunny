using UnityEngine;

public class DronePlayerState : IDroneState
{
    private DroneStateManager drone;
    private DroneStatePlayerSO dronePlayerSettings;

    // Camera
    private Camera mainCamera;
    private Camera droneCamera;

    // Player Settings
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
        droneCamera = drone.GetComponent<Camera>();

        // Set Variables 
        currentSpeed = startSpeed;
    }

    public void Enter()
    {
        mainCamera = Camera.main;
        mainCamera.enabled = false;
        droneCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Execute()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (currentSpeed < maxSpeed)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }

        // Calculate forward and strafing movement
        Vector3 forwardMovement = droneTransform.forward * currentSpeed * Time.deltaTime * moveVertical;
        Vector3 strafingMovement = droneTransform.right * currentSpeed * Time.deltaTime * moveHorizontal;

        // Apply movement
        droneTransform.position += forwardMovement + strafingMovement;

        // Apply tilt based on movement
        ApplyTilt(moveHorizontal, moveVertical);

        // Rotate based on mouse input
        RotateCamera();
    }

    public void Exit()
    {
        mainCamera.enabled = true;
        droneCamera.enabled = false;
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
