using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float health;
    private int level;
    private AnimationCurve healthCurve;
    private float maxHealth;
    private float currentHealth;
    private IEntityEventSubscriber entityEventSubscriber;

    public void Initialize(float health, int level, AnimationCurve healthCurve, IEntityEventSubscriber entityEventSubscriber)
    {
        this.health = health;
        this.level = level;
        this.healthCurve = healthCurve;
        this.entityEventSubscriber = entityEventSubscriber;
        if (entityEventSubscriber != null)
        {
            entityEventSubscriber.SubscribeToLevelUp(OnLevelUp);
        }

        maxHealth = GetHealthAtLevel();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Damage: {damage}, new Health: {currentHealth}");
    }

    public bool IsEntityDestroyed()
    {
        bool isDestroyed = currentHealth < 0 ? true : false;
        return isDestroyed;
    }

    public float GetHealthAtLevel()
    {
        return maxHealth * healthCurve.Evaluate(level);
    }

    public void OnLevelUp(int newLevel)
    {
        level = newLevel;
        maxHealth = GetHealthAtLevel();
    }

    private void OnDestroy()
    {
        if (entityEventSubscriber != null)
        {
            entityEventSubscriber.UnsubscribeFromLevelUp(OnLevelUp);
        }
    }
}
