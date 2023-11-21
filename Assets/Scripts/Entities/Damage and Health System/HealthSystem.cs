using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float health;
    private int level;
    private AnimationCurve healthCurve;
    private float currentHealth;

    public void Initialize(float health, int level, AnimationCurve healthCurve, PlayerController playerController, EnemyController enemyController)
    {
        this.health = health;
        this.level = level;
        this.healthCurve = healthCurve;

        if(playerController != null)
        {
            playerController.SubscribeToLevelUp(OnLevelUp);
        }
        if(enemyController != null)
        {
            enemyController.SubscribeToLevelUp(OnLevelUp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = GetHealthAtLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHealthAtLevel()
    {
        return health * healthCurve.Evaluate(level);
    }

    public void OnLevelUp(int newLevel)
    {
        level = newLevel;
        currentHealth = GetHealthAtLevel();
    }

    private void OnDestroy()
    {
        // Unsubscripe Event  
    }
}
