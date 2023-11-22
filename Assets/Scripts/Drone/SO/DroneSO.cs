using UnityEngine;

[CreateAssetMenu(fileName = "Drone Settings", menuName = "Entities/Basic Drone Settings", order = 2)]
public class DroneSO : ScriptableObject
{
    public string droneName;
    public int droneLevel;
    public float droneHealth;
    public float droneDamage;

    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
}
