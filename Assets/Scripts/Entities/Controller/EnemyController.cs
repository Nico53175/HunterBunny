using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour, IEntityEventSubscriber
{
    private Transform player;
    [SerializeField] private EnemySO enemy;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private DamageSystem damageSystem;

    private int enemyLevel;

    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();
    
    private void Awake()
    {
        enemyLevel = enemy.enemyLevel;

        if (healthSystem != null)
        {
            healthSystem.Initialize(enemy.enemyHealth, enemy.enemyLevel, enemy.healthCurve, this);
        }

        if (damageSystem != null)
        {
            damageSystem.Initialize(enemy.enemyDamage, enemy.enemyLevel, enemy.damageCurve, this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.enemySpeed * Time.deltaTime);
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
