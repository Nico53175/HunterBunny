using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    EnemyIdleStateSO enemyIdleSettings;
    private Transform playerTransform;

    // Idle Settings
    private float speed;
    private float idleRadius;
    private float visionRadius;

    public EnemyIdleState(EnemyStateManager enemy)
    {
        enemyIdleSettings = enemy.enemyIdleSettings;
        this.speed = enemyIdleSettings.speed;
        this.idleRadius = enemyIdleSettings.idleRadius;
        this.visionRadius = enemyIdleSettings.visionRadius;
    }

    public void Enter()
    {

    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
