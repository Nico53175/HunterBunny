using UnityEngine;

public class DroneObserveState : IDroneState
{
    private DroneStateManager drone;
    private DroneStateObserveSO droneObserveSettings;

    // Local Variables
    private float observeStartTime;
    private float nextUpdateTime;
    private float updateFrequency = 3f;
    private Vector3 targetPosition;

    // Observation Settings
    private float observationLength;
    private float observationFlightHeight;
    private float observationSpeed;
    private float observationRadius;
    private float rotationLerpSpeedObserve;

    // Transform
    private Transform droneTransform;
    private Transform playerTransform;

    public DroneObserveState(DroneStateManager drone) 
    {
        this.drone = drone;
        this.droneObserveSettings = drone.droneObserveSettings;

        // Set Drone Settings from SO
        observationLength = droneObserveSettings.observationLength;
        observationFlightHeight = droneObserveSettings.observationFlightHeight;
        observationSpeed = droneObserveSettings.observationSpeed;
        observationRadius = droneObserveSettings.observationRadius;
        rotationLerpSpeedObserve = droneObserveSettings.rotationLerpSpeedObserve;

        // Get needed Components
        playerTransform = drone.playerTransform;
        droneTransform = drone.GetComponent<Transform>();

        // Set Variables 
        drone.visionRadiusCollider.radius = observationRadius;
    }

    public void Enter()
    {
        observeStartTime = Time.time;
    }

    public void Execute()
    {
        if (Time.time - observeStartTime > observationLength)
        {
            return;
        }
        RotateToObserve();
    }

    public void Exit()
    {
        // Cleanup or actions needed when exiting the observe state (if any)
    }

    private void RotateToObserve()
    {
        // Player Settings
        Vector3 playerPosition = playerTransform.position;
        Quaternion playerRotation = playerTransform.rotation;

        // Drone Settings
        Vector3 dronePosition = droneTransform.position;
        Quaternion droneRotation = droneTransform.rotation;

        if (Time.time > nextUpdateTime)
        {
            targetPosition = playerPosition + new Vector3(0, observationFlightHeight, 0);
            nextUpdateTime = Time.time + updateFrequency;            
        }
        // Move towards the target position at the current speed
        drone.transform.position = Vector3.MoveTowards(dronePosition, targetPosition, observationSpeed * Time.deltaTime);

        // Lerp the drone's rotation to match the player's rotation
        drone.transform.rotation = Quaternion.Lerp(droneRotation, playerRotation, rotationLerpSpeedObserve * Time.deltaTime);

    }
}
