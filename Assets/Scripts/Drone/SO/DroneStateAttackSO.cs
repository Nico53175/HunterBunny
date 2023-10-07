using UnityEngine;

[CreateAssetMenu(fileName = "DroneStateAttackSettings", menuName = "Drones/Drone Attack State Settings", order = 2)]
public class DroneStateAttackSO : ScriptableObject
{
    public float rayDisplayDuration;
    public float rotationLerpSpeedAttack;
    public Color rayColor;
}