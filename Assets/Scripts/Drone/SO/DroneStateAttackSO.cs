using UnityEngine;

[CreateAssetMenu(fileName = "DroneStateAttackSettings", menuName = "Drones/Drone Attack State Settings", order = 2)]
public class DroneStateAttackSO : ScriptableObject
{
    public int reloadTimer;
    public float rotationLerpSpeedAttack;
    public Color rayColor;
}