using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Entities/Basic Player Settings", order = 1)]
public class PlayerSO : ScriptableObject
{
    public string playerName;
    public int playerLevel;
    public float playerHealth;
    public float playerDamage;
    public float playerSpeed;
    public float playerSprintMultiplier;
    public float playerJumpForce;
    public LayerMask groundLayer;
    public float groundCheckDistance;
    public DamageType playerDamageType;

    public AnimationCurve healthCurve;
    public AnimationCurve damageCurve;
}
