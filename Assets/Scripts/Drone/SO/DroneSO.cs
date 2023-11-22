using UnityEngine;

[CreateAssetMenu(fileName = "Drone Settings", menuName = "Entities/Basic Drone Settings", order = 2)]
public class DroneSO : ScriptableObject
{
    public string droneName;
    public int level;
    public float health;
    public float damage;

    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
}
