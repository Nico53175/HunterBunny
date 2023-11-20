using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    [SerializeField] private EnemySO enemy;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private DamageSystem damageSystem;
    private void Awake()
    {
        if (healthSystem != null)
        {
            Debug.Log("H");
            healthSystem.Initialize(enemy.enemyHealth, enemy.enemyLevel, enemy.healthCurve);
        }

        if (damageSystem != null)
        {
            Debug.Log("D");
            damageSystem.Initialize(enemy.enemyDamage, enemy.enemyLevel, enemy.damageCurve);
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

    private void OnDestroy()
    {
        // Play death animation 
        // Particle System
    }
}
