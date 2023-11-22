using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    private float damage;
    private int level;
    private AnimationCurve damageCurve;
    private float currentDamage = 0;

    private IEntityEventSubscriber entityEventSubscriber;
    public void Initialize(float damage, int level, AnimationCurve damageCurve, IEntityEventSubscriber entityEventSubscriber)
    {
        this.damage = damage;
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
        return damage * damageCurve.Evaluate(level);
    }

    public void OnLevelUp(int newLevel)
    {
        level = newLevel;
        currentDamage = GetDamageAtLevel();
    }

    public void ApplyDamageOverTime(float damagePerTick, float duration, float tickInterval)
    {
        StartCoroutine(DamageOverTimeCoroutine(damagePerTick, duration, tickInterval));
    }

    private IEnumerator DamageOverTimeCoroutine(float damagePerTick, float duration, float tickInterval)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            // Apply damage
            ApplyDamage(damagePerTick);

            // Wait for the next tick
            yield return new WaitForSeconds(tickInterval);

            // Update the time passed
            timePassed += tickInterval;
        }
    }
    private void ApplyDamage(float damage)
    {
        // Apply damage to the health system, enemy, player, etc.
        Debug.Log($"Applying {damage} damage.");
    }

    private void OnDestroy()
    {
        if (entityEventSubscriber != null)
        {
            entityEventSubscriber.UnsubscribeFromLevelUp(OnLevelUp);
        }
    }
}
