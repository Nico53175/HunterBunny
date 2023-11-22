using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    private float baseDamage;
    private int level;
    private AnimationCurve damageCurve;
    private float currentDamage = 0;

    private IEntityEventSubscriber entityEventSubscriber;
    public void Initialize(float damage, int level, AnimationCurve damageCurve, IEntityEventSubscriber entityEventSubscriber)
    {
        this.baseDamage = damage;
        this.level = level;
        this.damageCurve = damageCurve;
        this.entityEventSubscriber = entityEventSubscriber;
        if (entityEventSubscriber != null)
        {
            entityEventSubscriber.SubscribeToLevelUp(OnLevelUp);
        }

        currentDamage = GetDamageAtLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDamageAtLevel()
    {
        return baseDamage * damageCurve.Evaluate(level);
    }

    public void OnLevelUp(int newLevel)
    {
        level = newLevel;
        currentDamage = GetDamageAtLevel();
    }

    public void ApplyDamage(HealthSystem target)
    {
        target.TakeDamage(currentDamage);
    }
    public void ApplyDamageOverTime(HealthSystem target, float damagePerTick, float duration, float tickInterval)
    {
        StartCoroutine(DamageOverTimeCoroutine(target, damagePerTick, duration, tickInterval));
    }

    private IEnumerator DamageOverTimeCoroutine(HealthSystem target, float damagePerTick, float duration, float tickInterval)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            target.TakeDamage(damagePerTick);

            // Wait for the next tick
            yield return new WaitForSeconds(tickInterval);

            // Update the time passed
            timePassed += tickInterval;
        }
    }

    private void OnDestroy()
    {
        if (entityEventSubscriber != null)
        {
            entityEventSubscriber.UnsubscribeFromLevelUp(OnLevelUp);
        }
    }
}
