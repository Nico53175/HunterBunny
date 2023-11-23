using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    EnemyIdleStateSO enemyIdleSettings;
    private Transform playerTransform;
    public EnemyIdleState(EnemyStateManager enemy)
    {
        enemyIdleSettings = enemy.enemyIdleSettings;
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
