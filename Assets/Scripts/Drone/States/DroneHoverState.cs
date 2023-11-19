using UnityEngine;

public class DroneHoverState : IDroneState
{
    private DroneStateManager drone;
    private DroneStateHoverSO droneHoverSettings;

    // Local Variables
    private float currentSpeed = 0;
    private Vector3 targetPosition;

    // Hover Settings
    private float visionRadius;
    private float hoverRadius;
    private float flightHeight;
    private float minSpeed;
    private float maxSpeed;
    private float rotationLerpSpeedHover;

    // Timer
    private float updateFrequency = 3;
    private float nextUpdateTime = 0;

    // Transform
    private Transform droneTransform;
    private Transform playerTransform;

    public DroneHoverState(DroneStateManager drone)
    {
        this.drone = drone;
        droneHoverSettings = drone.droneHoverSettings;

        // Set Drone Settings from SO
        visionRadius = droneHoverSettings.visionRadius;
        hoverRadius = droneHoverSettings.hoverRadius;
        flightHeight = droneHoverSettings.flightHeight;
        minSpeed = droneHoverSettings.minSpeed;
        maxSpeed = droneHoverSettings.maxSpeed;
        rotationLerpSpeedHover = droneHoverSettings.rotationLerpSpeedHover;

        // Get needed Components
        playerTransform = drone.playerTransform;
        droneTransform = drone.GetComponent<Transform>();

        // Set Variables 
        currentSpeed = 2;
        drone.visionRadiusCollider.radius = visionRadius;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        // Drone Settings
        Vector3 dronePosition = droneTransform.position;
        Quaternion droneRotation = droneTransform.rotation;

        // Player Settings
        Vector3 playerPosition = playerTransform.position;
        Quaternion playerRotation = playerTransform.rotation;

        if (Time.time > nextUpdateTime)
        {
            // Choose a random point within the hoverRadius around the player
            Vector2 randomCirclePoint = Random.insideUnitCircle * hoverRadius;
            targetPosition = playerPosition + new Vector3(randomCirclePoint.x, flightHeight, randomCirclePoint.y);

            // Choose a random speed between minSpeed and maxSpeed
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            if (Vector3.Distance(dronePosition, playerPosition) > hoverRadius)
            {
                currentSpeed = currentSpeed * 2.5f;
            }
            nextUpdateTime = Time.time + updateFrequency;
        }
        // Move towards the target position at the current speed
        drone.transform.position = Vector3.MoveTowards(dronePosition, targetPosition, currentSpeed * Time.deltaTime);

        // Lerp the drone's rotation to match the player's rotation
        drone.transform.rotation = Quaternion.Lerp(droneRotation, playerRotation, rotationLerpSpeedHover * Time.deltaTime); 
    }

    public void Exit()
    {
        // Cleanup or actions needed when exiting the hover state (if any)
    }
}