using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float health;
    private int level;
    private AnimationCurve healthCurve;
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

        currentHealth = GetHealthAtLevel();
    }

    // Start is called before the first frame update
    void Start()
    {

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
        if (entityEventSubscriber != null)
        {
            entityEventSubscriber.UnsubscribeFromLevelUp(OnLevelUp);
        }
    }
}
