using System.Runtime.CompilerServices;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
        return health * healthCurve.Evaluate(level);
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
