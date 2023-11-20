using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    private float enemyDamage;
    private int enemyLevel;
    private AnimationCurve enemyDamageCurve;

    public void Initialize(float damage, int level, AnimationCurve damageCurve)
    {
        enemyDamage = damage;
        enemyLevel = level;
        enemyDamageCurve = damageCurve;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetDamageAtLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDamageAtLevel()
    {
        return enemyDamage * enemyDamageCurve.Evaluate(enemyLevel);
    }
}
