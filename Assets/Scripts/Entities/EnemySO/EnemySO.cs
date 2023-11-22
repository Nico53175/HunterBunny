using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Settings", menuName = "Entities/Basic Enemy Settings", order = 3)]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int enemyLevel;
    public float enemyHealth;
    public float enemyDamage;
    public float enemySpeed;
    public DamageType enemyDamageType;

    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
}
