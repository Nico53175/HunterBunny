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

    private Transform playerTransform;

    // Drone Settings 
    private Vector3 targetPosition;
    private float currentSpeed;
    private SphereCollider visionRadiusCollider;
    [SerializeField] private float visionRadius;
    [SerializeField] public DroneState currentState;

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
        currentState = DroneState.Hover;
        
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
        foreach(LineRenderer renderer in laserRenderers)
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
        lr.material.color = Color.green;
        lr.enabled = false;
    }
    private GameObject FindPlayerObject()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        return playerObject;    
    }

    // State Methodes
    // Hover
    private void DroneHoverAroundPlayer()
    {
        if (Time.time > nextUpdateTime)
        {
            // Choose a random point within the hoverRadius around the player
            Vector2 randomCirclePoint = Random.insideUnitCircle * hoverRadius;
            targetPosition = playerTransform.position + new Vector3(randomCirclePoint.x, flightHeight, randomCirclePoint.y);

            // Choose a random speed between minSpeed and maxSpeed
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            if (Vector3.Distance(transform.position, playerTransform.position) > hoverRadius)
            {
                currentSpeed = currentSpeed * 2.5f;
            }
            nextUpdateTime = Time.time + updateFrequency;
        }

        // Move towards the target position at the current speed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // Lerp the drone's rotation to match the player's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, rotationLerpSpeedHover * Time.deltaTime);
    }

    // Attack
    private void DroneAttack()
    {
        if (detectedEnemies.Count > 0)
        {
            foreach (LineRenderer renderer in laserRenderers)
            {
                renderer.enabled = true;
            }

            Transform enemy = GetClosestDetectedEnemy();
            bool enemyDestroyed = false;

            // Calculate the direction towards the enemy
            Vector3 directionToEnemy = enemy.position - transform.position;

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
            attackRayLength = Vector3.Distance(transform.position, enemy.transform.position);

            foreach (var laserRenderer in laserRenderers)
            {
                Vector3 laserStartPosition = laserRenderer.transform.position;
                Vector3 laserEndPosition = laserStartPosition + transform.forward * attackRayLength;

                RaycastHit hit;
                if (Physics.Raycast(laserStartPosition, transform.forward, out hit, attackRayLength))
                {
                    if(hit.transform.gameObject.tag == "Enemy")
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
                foreach(LineRenderer renderer in laserRenderers)
                {
                    renderer.enabled = false;
                }
            }
            rayHideTime = Time.time + rayDisplayDuration;
        }
        else
        {
            currentState = DroneState.Hover;
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
            currentState = DroneState.Attack;
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
        }

        // Move towards the target position at the current speed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // Lerp the drone's rotation to match the player's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, rotationLerpSpeedHover * Time.deltaTime);
    }
}
