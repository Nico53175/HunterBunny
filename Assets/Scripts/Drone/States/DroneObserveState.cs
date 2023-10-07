using TMPro;
using UnityEngine;

public class DroneObserveState : IDroneState
{
    private DroneStateManager drone;
    private DroneStateObserveSO droneObserveSettings;
    private float observeStartTime;
    private float nextUpdateTime;
    private float updateFrequency = 3f;
    private Vector3 targetPosition;

    private float observationLength;
    private float observationFlightHeight;
    private float observationSpeed;
    private float observationRadius;
    private float rotationLerpSpeedObserve;
    public DroneObserveState(DroneStateManager drone)
    {
        this.drone = drone;
        this.droneObserveSettings = drone.droneObserveSettings;

        observationLength = droneObserveSettings.observationLength;
        observationFlightHeight = droneObserveSettings.observationFlightHeight;
        observationSpeed = droneObserveSettings.observationSpeed;
        observationRadius = droneObserveSettings.observationRadius;
        rotationLerpSpeedObserve = droneObserveSettings.rotationLerpSpeedObserve;
    }

    public void Enter()
    {
        observeStartTime = Time.time;
        drone.visionRadiusCollider.radius = observationRadius;
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
        Vector3 playerPosition = drone.playerTransform.position;
        Vector3 dronePosition = drone.transform.position;
        Quaternion droneRotation = drone.transform.rotation;
        Quaternion playerRotation = drone.playerTransform.rotation;

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
