using System;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    private float damage;
    private int level;
    private AnimationCurve damageCurve;
    private float currentDamage;

    public void Initialize(float damage, int level, AnimationCurve damageCurve, PlayerController playerController, EnemyController enemyController)
    {
        this.damage = damage;
        this.level = level;
        this.damageCurve = damageCurve;

        if (playerController != null)
        {
            playerController.SubscribeToLevelUp(OnLevelUp);
        }
        if (enemyController != null)
        {
            enemyController.SubscribeToLevelUp(OnLevelUp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentDamage = GetDamageAtLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDamageAtLevel()
    {
        return damage * damageCurve.Evaluate(level);
    }

    public void OnLevelUp(int newLevel)
    {
        level = newLevel;
        currentDamage = GetDamageAtLevel();
    }
    private void OnDestroy()
    {
        // Unsubscribe Event
    }
}
