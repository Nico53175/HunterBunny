
using UnityEngine;

[CreateAssetMenu(fileName = "DroneStateObserveSettings", menuName = "Drones/Drone Observe State Settings", order = 3)]
public class DroneStateObserveSO : ScriptableObject
{
    public float observationLength;
    public float observationFlightHeight;
    public float observationSpeed;
    public float observationRadius;
    public float rotationLerpSpeedObserve;
}