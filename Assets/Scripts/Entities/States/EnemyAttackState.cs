using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    EnemyAttackStateSO enemyAttackSettings;
    private Transform playerTransform;
    public EnemyAttackState(EnemyStateManager enemy)
    {
        enemyAttackSettings = enemy.enemyAttackSettings;
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
