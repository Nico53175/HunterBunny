using UnityEngine;


[CreateAssetMenu(fileName = "EnemyIdleSettings", menuName = "Enemies/Enemy Idle State Settings", order = 1)]
public class EnemyIdleStateSO : ScriptableObject
{
    public float visionRadius;
    public float idleRadius;
    public float speed;
}
