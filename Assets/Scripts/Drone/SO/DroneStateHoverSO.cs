using UnityEngine;

[CreateAssetMenu(fileName = "DroneStateHoverSettings", menuName = "Drones/Drone Hover State Settings", order = 1)]
public class DroneStateHoverSO : ScriptableObject
{
    public string droneName;
    public int droneUpgradeLvl;

    public float visionRadius;
    public float hoverRadius;
    public float flightHeight;
    public float minSpeed;
    public float maxSpeed;
    public float rotationLerpSpeedHover;
}
