using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Entities/Basic Player Settings", order = 1)]
public class PlayerSO : ScriptableObject
{
    public string playerName;
    public int level;

    public DamageType damageType;
    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
    public float health;
    public float damage;

    public float speed;
    public float sprintMultiplier;

    public float jumpForce;
    public LayerMask groundLayer;
    public float groundCheckDistance;
}
