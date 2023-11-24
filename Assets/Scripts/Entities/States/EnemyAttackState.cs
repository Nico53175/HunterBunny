using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyStateManager enemy;
    EnemyAttackStateSO enemyAttackSettings;

    // Attack Setting
    private float speed;
    private float attackSpeed;

    // Transform
    private Transform transform;
    private Transform playerTransform;

    // Variables 
    HealthSystem playerHealthSystem;
    DamageSystem damageSystem;
    public EnemyAttackState(EnemyStateManager enemy, Transform playerTransform)
    {
        this.enemy = enemy;
        this.playerTransform = playerTransform;
        this.transform = enemy.transform;

        enemyAttackSettings = enemy.enemyAttackSettings;
        speed = enemyAttackSettings.speed;
        attackSpeed = enemyAttackSettings.attackSpeed;

        damageSystem = enemy.GetComponent<DamageSystem>();
        playerHealthSystem = playerTransform.GetComponent<HealthSystem>();
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        transform.LookAt(playerTransform);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.transform.position, speed * Time.deltaTime);
    }

    public void Exit()
    {

    }   
}
