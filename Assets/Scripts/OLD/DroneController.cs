using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public enum DroneState
    {
        Hover,
        Attack,
        Observe
    }
    // Player 
    private Transform playerTransform;

    // Drone Settings 
    private Vector3 targetPosition;
    private float currentSpeed;
    private SphereCollider visionRadiusCollider;
    [SerializeField] private float visionRadius;
    [SerializeField] public DroneState currentState;
    private DroneState previousState;

    // State Settings
    // Hover
    [SerializeField] private float hoverRadius;
    [SerializeField] private float flightHeight;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationLerpSpeedHover;

    // Attack
    private float attackRayLength; // Length of the attack rays
    [SerializeField] private float attackRayDuration; // Duration for which the ray visualization will stay visible    
    [SerializeField] private float rayDisplayDuration; // How long the ray remains visible after firing
    [SerializeField] private float rotationLerpSpeedAttack;
    [SerializeField] private Color rayColor;
    private float rayHideTime;
    private List<LineRenderer> laserRenderers;
    private List<Transform> detectedEnemies = new List<Transform>();
    // Timer
    [SerializeField] private float updateFrequency;
    private float nextUpdateTime = 0;

    private void Start()
    {
        Setup();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            SetState(currentState != DroneState.Observe ? DroneState.Observe : DroneState.Hover);
        }

        switch (currentState)
        {
            case DroneState.Hover:
                DroneHoverAroundPlayer();
                break;
            case DroneState.Attack:
                DroneAttack();
                break;

            case DroneState.Observe:
                DroneObserve();
                break;
        }
    }
    private void Setup()
    {
        // Set default state
        SetState(DroneState.Hover);

        // Find Player 
        GameObject player = FindPlayerObject();
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

        // Set starting Position
        targetPosition = transform.position;

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
        visionRadiusCollider.radius = visionRadius;

    }
    private void InitializeLineRenderer(LineRenderer lr)
    {
        lr.startWidth = 0.1f; // adjust as needed
        lr.endWidth = 0.1f;   // adjust as needed
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = rayColor;
        lr.enabled = false;
    }
    private GameObject FindPlayerObject()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        return playerObject;
    }
    public void SetState(DroneState newState)
    {
        previousState = currentState;
        currentState = newState;
        if (previousState != currentState)
        {
            OnStateEnter();
        }
    }
    private void OnStateEnter()
    {
        // Actions which should only be made once when entering the state
        switch (currentState)
        {
            case DroneState.Hover:
                visionRadiusCollider.radius = visionRadius;
                break;

            case DroneState.Attack:
                foreach (LineRenderer renderer in laserRenderers)
                {
                    renderer.enabled = true;
                }
                break;

            case DroneState.Observe:

                break;
        }
    }

    // State Methodes
    // Hover
    private void DroneHoverAroundPlayer()
    {
        Quaternion playerRotation = playerTransform.rotation;
        if (Time.time > nextUpdateTime)
        {
            Vector3 playerPosition = playerTransform.position;
            // Choose a random point within the hoverRadius around the player
            Vector2 randomCirclePoint = Random.insideUnitCircle * hoverRadius;
            targetPosition = playerPosition + new Vector3(randomCirclePoint.x, flightHeight, randomCirclePoint.y);

            // Choose a random speed between minSpeed and maxSpeed
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            if (Vector3.Distance(transform.position, playerPosition) > hoverRadius)
            {
                currentSpeed = currentSpeed * 2.5f;
            }
            nextUpdateTime = Time.time + updateFrequency;
        }

        // Move towards the target position at the current speed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // Lerp the drone's rotation to match the player's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, playerRotation, rotationLerpSpeedHover * Time.deltaTime);
    }

    // Attack
    private void DroneAttack()
    {
        if (detectedEnemies.Count > 0)
        {
            Transform enemy = GetClosestDetectedEnemy();
            Vector3 enemyPosition = enemy.position;
            bool enemyDestroyed = false;

            // Calculate the direction towards the enemy
            Vector3 directionToEnemy = enemyPosition - transform.position;

            // Determine the "up" vector for the drone's rotation based on the relative vertical position of the enemy
            Vector3 upVector;
            if (Vector3.Dot(transform.up, directionToEnemy) < 0) // If the enemy is below the drone
            {
                upVector = -transform.forward;
            }
            else
            {
                upVector = transform.up;
            }

            // Rotate the drone to look at the enemy
            Quaternion desiredRotation = Quaternion.LookRotation(directionToEnemy, upVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationLerpSpeedAttack * Time.deltaTime);

            // Ray attack logic
            Vector3[] laserStartPositions = new Vector3[laserRenderers.Count];
            Vector3[] laserEndPositions = new Vector3[laserRenderers.Count];
            attackRayLength = Vector3.Distance(transform.position, enemyPosition);

            foreach (var laserRenderer in laserRenderers)
            {
                Vector3 laserStartPosition = laserRenderer.transform.position;
                Vector3 laserEndPosition = laserStartPosition + transform.forward * attackRayLength;

                RaycastHit hit;
                if (Physics.Raycast(laserStartPosition, transform.forward, out hit, attackRayLength))
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        Destroy(hit.transform.gameObject);
                        detectedEnemies.Remove(hit.transform);
                        laserEndPosition = hit.point;
                        enemyDestroyed = true;
                    }
                }
                if (!enemyDestroyed)
                {
                    DrawRay(laserRenderer, laserStartPosition, laserEndPosition);
                }
            }

            if (enemyDestroyed)
            {
                foreach (LineRenderer renderer in laserRenderers)
                {
                    renderer.enabled = false;
                }
            }
            rayHideTime = Time.time + rayDisplayDuration;
        }
        else
        {
            SetState(DroneState.Hover);
        }
    }
    private void DrawRay(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    public Transform GetClosestDetectedEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Transform enemy in detectedEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has an "Enemy" tag (or any other criteria you set for enemies)
        if (other.CompareTag("Enemy"))
        {
            detectedEnemies.Add(other.transform);
            SetState(DroneState.Attack);
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

    // Observe
    private void DroneObserve() // Add Minimap to reveal it when entering this state
    {
        if (Time.time > nextUpdateTime)
        {
            targetPosition = playerTransform.position + new Vector3(0, 10, 0);
            nextUpdateTime = Time.time + updateFrequency;
            // Move towards the target position at the current speed
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        }
        // Lerp the drone's rotation to match the player's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, rotationLerpSpeedHover * Time.deltaTime);
    }
}
