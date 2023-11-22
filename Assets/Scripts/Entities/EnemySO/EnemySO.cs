using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Settings", menuName = "Entities/Basic Enemy Settings", order = 3)]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int level;
    public float health;
    public float damage;
    public float speed;
    public DamageType damageType;

    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
}
