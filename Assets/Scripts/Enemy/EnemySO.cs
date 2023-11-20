using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Settings", menuName = "Enemies/Basic Enemy Settings", order = 1)]
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
