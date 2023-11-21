using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    [SerializeField] private EnemySO enemy;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private DamageSystem damageSystem;

    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();
    private void Awake()
    {
        if (healthSystem != null)
        {
            healthSystem.Initialize(enemy.enemyHealth, enemy.enemyLevel, enemy.healthCurve, null, this);
        }

        if (damageSystem != null)
        {
            damageSystem.Initialize(enemy.enemyDamage, enemy.enemyLevel, enemy.damageCurve, null, this);
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

    private void OnDestroy()
    {
        // Play death animation 
        // Particle System
    }
}
