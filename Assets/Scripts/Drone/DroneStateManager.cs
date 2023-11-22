using System.Collections.Generic;
using UnityEngine;

public class DroneStateManager : MonoBehaviour
{
    // States
    private IDroneState _currentState;
    private IDroneState _hoverState;
    private IDroneState _attackState;
    private IDroneState _observeState;
    private IDroneState _playerState;

    // Player 
    [HideInInspector] public Transform playerTransform;

    // Drone Settings 
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public SphereCollider visionRadiusCollider;
    [SerializeField] public DroneStateHoverSO droneHoverSettings;
    [SerializeField] public DroneStateAttackSO droneAttackSettings;
    [SerializeField] public DroneStateObserveSO droneObserveSettings;
    [SerializeField] public DroneStatePlayerSO dronePlayerSettings;
    

    // Attack
    private List<LineRenderer> laserRenderers;
    [HideInInspector] public List<Transform> detectedEnemies = new List<Transform>();

    // Interface Methodes
    public void SetState(IDroneState newState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = newState;
        _currentState.Enter();
    }
    public IDroneState GetHoverState()
    {
        return _hoverState;
    }

    public IDroneState GetAttackState()
    {
        return _attackState;
    }
    
    public IDroneState GetObserveState()
    {
        return _observeState;
    }
    
    public IDroneState GetPlayerState()
    {
        return _playerState;
    }

    // Execution
    private void Start()
    {
        Setup();
        _hoverState = new DroneHoverState(this);
        _attackState = new DroneAttackState(this, laserRenderers);
        _observeState = new DroneObserveState(this);
        _playerState = new DronePlayerState(this);
        SetState(_hoverState);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetState(_currentState != _observeState ? _observeState : _hoverState);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_currentState != _playerState)
            {
                SetState(_playerState);
            }
            else
            {
                if (detectedEnemies.Count > 0)
                {
                    SetState(_attackState);
                }
                else
                {
                    SetState(_hoverState);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        _currentState.Execute();
    }

    private void Setup()
    {
        // Find Player 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No GameObject found with the 'Player' tag!");
            enabled = false;
            return;
        }

        // Get and Initialize LineRender for Attack
        laserRenderers = new List<LineRenderer>();
        laserRenderers.Add(transform.Find("LeftLaserUp").GetComponent<LineRenderer>());
        laserRenderers.Add(transform.Find("RightLaserUp").GetComponent<LineRenderer>());
        laserRenderers.Add(transform.Find("LeftLaserDown").GetComponent<LineRenderer>());
        laserRenderers.Add(transform.Find("RightLaserDown").GetComponent<LineRenderer>());
        foreach (LineRenderer renderer in laserRenderers)
        {
            InitializeLineRenderer(renderer);
        }

        // Get Collider for Enemy detection
        visionRadiusCollider = GetComponent<SphereCollider>();
    }
    private void InitializeLineRenderer(LineRenderer lr)
    {
        lr.startWidth = 0.1f; 
        lr.endWidth = 0.1f;   
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = droneAttackSettings.rayColor;
        lr.enabled = false;
    }
    public void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            detectedEnemies.Add(other.transform);
            if(_currentState != _playerState)
            {
                SetState(_attackState);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            detectedEnemies.Remove(other.transform);
        }
    }
}

