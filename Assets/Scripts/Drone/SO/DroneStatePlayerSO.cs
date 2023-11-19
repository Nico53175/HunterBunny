using UnityEngine;

[CreateAssetMenu(fileName = "DroneStatePlayerSettings", menuName = "Drones/Drone Player State Settings", order = 4)]
public class DroneStatePlayerSO : ScriptableObject
{
    public float startSpeed;
    public float maxSpeed;
    public float acceleration;
    public float tiltIntensity;
    public float tiltSpeed;
    public float rotationSpeed;
}
