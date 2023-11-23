using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour, IEntityEventSubscriber
{
    [SerializeField] private EnemySO enemy;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private DamageSystem damageSystem;

    private int enemyLevel;

    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();
    
    private void Awake()
    {
        enemyLevel = enemy.level;

        if (healthSystem != null)
        {
            healthSystem.Initialize(enemy.health, enemy.level, enemy.healthCurve, this);
        }

        if (damageSystem != null)
        {
            damageSystem.Initialize(enemy.damage, enemy.level, enemy.damageCurve, this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubscribeToLevelUp(UnityAction<int> callback)
    {
        OnLevelUp.AddListener(callback);
    }

    public void UnsubscribeFromLevelUp(UnityAction<int> callback)
    {
        OnLevelUp.RemoveListener(callback);
    }

    public void LevelUp()
    {
        enemyLevel++;
        OnLevelUp.Invoke(enemyLevel);
    }
}
