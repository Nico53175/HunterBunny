using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static DroneController;

public class DroneStateManager : MonoBehaviour
{
    private IDroneState _currentState;
    private IDroneState _hoverState;
    private IDroneState _attackState;
    private IDroneState _observeState;

    // Player 
    [HideInInspector] public Transform playerTransform;

    // Drone Settings 
    [SerializeField] public float currentSpeed;
    [HideInInspector] public SphereCollider visionRadiusCollider;
    [SerializeField] public DroneStateHoverSO droneHoverSettings;
    [SerializeField] public DroneStateAttackSO droneAttackSettings;
    [SerializeField] public DroneStateObserveSO droneObserveSettings;

    // Attack
    private List<LineRenderer> laserRenderers;
    public List<Transform> detectedEnemies = new List<Transform>();

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

    // Execution
    private void Start()
    {
        Setup();
        _hoverState = new DroneHoverState(this);
        _attackState = new DroneAttackState(this, laserRenderers);
        _observeState = new DroneObserveState(this);
        SetState(_hoverState);
    }

    private void Update()
    {
        _currentState.Execute();

        if (Input.GetKeyDown(KeyCode.O))
        {
            SetState(_currentState != _observeState ? _observeState : _hoverState);
        }
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
        lr.startWidth = 0.1f; // adjust as needed
        lr.endWidth = 0.1f;   // adjust as needed
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
        // Check if the object that entered the trigger has an "Enemy" tag (or any other criteria you set for enemies)
        if (other.CompareTag("Enemy"))
        {
            detectedEnemies.Add(other.transform);
            SetState(GetAttackState());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Remove the object from the detectedEnemies list if it leaves the trigger area
        if (other.CompareTag("Enemy"))
        {
            detectedEnemies.Remove(other.transform);
        }
    }
}

