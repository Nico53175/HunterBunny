using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    // Enemy States
    private IEnemyState _currentState;
    private IEnemyState _idleState;
    private IEnemyState _attackState;

    // Player 
    [HideInInspector] private Transform playerTransform;

    // Enemy State Settings
    [SerializeField] public EnemyIdleStateSO enemyIdleSettings;
    [SerializeField] public EnemyAttackStateSO enemyAttackSettings;
    public void SetState(IEnemyState newState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = newState;
        _currentState.Enter();
    }

    public IEnemyState GetIdleState()
    {
        return _idleState;
    }
    
    public IEnemyState GetAttackState()
    {
        return _attackState;
    }

    private void Start()
    {
        Setup();
        _idleState = new EnemyIdleState(this);
        _attackState = new EnemyAttackState(this, playerTransform);
        
        SetState(_attackState);
    }

    private void Setup()
    {
        // Find Player 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Debug.Log("Player found");
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No GameObject found with the 'Player' tag!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _currentState.Execute();
    }
}
