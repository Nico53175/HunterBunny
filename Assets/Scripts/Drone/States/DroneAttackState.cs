using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneAttackState : IDroneState
{
    private DroneStateManager drone;
    private DroneStateAttackSO droneAttackSettings;

    // Attack Setting
    private float rotationLerpSpeedAttack;
    private int reloadTimer;

    // Transform
    private Transform droneTransform;
    private List<Transform> detectedEnemies = new List<Transform>();
    private List<LineRenderer> laserRenderers = new List<LineRenderer>();

    // Variables 
    private const string reloadTimerId = "ReloadTimer";
    bool areRendererEnabled;
    HealthSystem enemyHealthSystem;
    DamageSystem droneDamageSystem;
    public DroneAttackState(DroneStateManager drone,List<LineRenderer> laserRenderers)
    {
        this.drone = drone;
        this.laserRenderers = laserRenderers;

        // Set Drone Settings from SO
        droneAttackSettings = drone.droneAttackSettings;
        rotationLerpSpeedAttack = droneAttackSettings.rotationLerpSpeedAttack;
        reloadTimer = droneAttackSettings.reloadTimer;

        // Get needed Components
        droneTransform = drone.GetComponent<Transform>();
        droneDamageSystem = drone.GetComponent<DamageSystem>();
    }

    public void Enter()
    {
        foreach (LineRenderer renderer in laserRenderers)
        {
            renderer.enabled = true;
        }
        detectedEnemies = drone.detectedEnemies;
        ReloadTimer.StartTimer(reloadTimerId, 0.5f); // Timer
    }

    public void Execute()
    {
        if (detectedEnemies.Count > 0)
        {
            if (!ReloadTimer.IsTimerDone(reloadTimerId))
            {
                if (areRendererEnabled)
                {
                    DisableLasers();
                    areRendererEnabled = false;
                }
                RotateTowardsTarget();
                ReloadTimer.Update();
            }
            else
            {
                detectedEnemies = drone.detectedEnemies;
                if (!areRendererEnabled)
                {
                    EnableLasers();
                    areRendererEnabled = true;
                }
                ProcessAttack();
            }
        }
        else
        {
            drone.SetState(drone.GetHoverState());
        }
    }

    public void Exit()
    {
        foreach (LineRenderer renderer in laserRenderers)
        {
            renderer.enabled = false;
        }

        ReloadTimer.StopTimer(reloadTimerId);
    }

    private void DrawRay(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private void ProcessAttack()
    {        
        Transform enemy = GetClosestDetectedEnemy();
        enemyHealthSystem = enemy.GetComponent<HealthSystem>();
        if (enemy != null)
        {
            PerformAttack(enemy);
        }
    }

    private void PerformAttack(Transform enemy)
    {
        RotateTowardsEnemy(enemy);
        AttackEnemy(enemy);
    }

    private void RotateTowardsEnemy(Transform enemy)
    {
        Vector3 directionToEnemy = enemy.position - droneTransform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(directionToEnemy, Vector3.up);
        droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, desiredRotation, rotationLerpSpeedAttack * Time.deltaTime);
    }

    //private void AttackEnemy(Transform enemy)
    //{
    //    float attackRayLength = Vector3.Distance(droneTransform.position, enemy.position);
    //    foreach (var laserRenderer in laserRenderers)
    //    {
    //        Vector3 laserStartPosition = laserRenderer.transform.position;
    //        Vector3 laserEndPosition = laserStartPosition + droneTransform.forward * attackRayLength;

    //        if (RaycastEnemy(laserStartPosition, laserEndPosition, attackRayLength))
    //        {
    //            ReloadTimer.StartTimer(reloadTimerId, reloadTimer); // Start Reload Timer

    //        }
    //        else
    //        {
    //            DrawRay(laserRenderer, laserStartPosition, laserEndPosition);
    //        }
    //    }
    //}

    //private bool RaycastEnemy(Vector3 start, Vector3 end, float length)
    //{
    //    if (Physics.Raycast(start, droneTransform.forward, out RaycastHit hit, length))
    //    {

    //        if (hit.transform.gameObject.tag == "Enemy")
    //        {
    //            Debug.Log("Hitting Enemy");
    //            //drone.DestroyEnemy(hit.transform.gameObject);
    //            //detectedEnemies.Remove(hit.transform);

    //            return true;
    //        }
    //    }
    //    return false;
    //}
    private void AttackEnemy(Transform enemy)
    {
        float attackRayLength = Vector3.Distance(droneTransform.position, enemy.position);
        foreach (var laserRenderer in laserRenderers)
        {
            Vector3 laserStartPosition = laserRenderer.transform.position;
            Vector3 laserEndPosition = laserStartPosition + new Vector3(0, 0.5f, 0) + droneTransform.forward * attackRayLength;

            bool enemyDestroyed;
            if (RaycastEnemy(laserStartPosition, laserEndPosition, attackRayLength, out enemyDestroyed))
            {
                if (enemyDestroyed)
                {
                    Debug.Log("Enemy Destroyed");
                    enemyHealthSystem = null;
                    drone.DestroyEnemy(enemy.transform.gameObject);
                    ReloadTimer.StartTimer(reloadTimerId, reloadTimer); // Start Reload Timer
                }
            }
            else
            {
                DrawRay(laserRenderer, laserStartPosition, laserEndPosition);
            }
        }
    }

    private bool RaycastEnemy(Vector3 start, Vector3 end, float length, out bool enemyDestroyed)
    {
        enemyDestroyed = false;

        if (Physics.Raycast(start, droneTransform.forward, out RaycastHit hit, length))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Hitting Enemy");

                // Apply damage to the enemy               
                if (enemyHealthSystem != null)
                {
                    droneDamageSystem.ApplyDamage(enemyHealthSystem);
                    enemyDestroyed = enemyHealthSystem.IsEntityDestroyed();
                }

                return true;
            }
        }
        return false;
    }


    private void DisableLasers()
    {
        foreach (LineRenderer renderer in laserRenderers)
        {
            DrawRay(renderer, Vector3.zero, Vector3.zero);
            renderer.enabled = false;
        }
    }

    private void EnableLasers()
    {
        foreach (LineRenderer renderer in laserRenderers)
        {
            renderer.enabled = true;
        }
    }

    private Transform GetClosestDetectedEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Transform enemy in detectedEnemies)
        {
            float distance = Vector3.Distance(droneTransform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;        
    }

    private void RotateTowardsTarget()
    {
        Vector3 enemyPos = GetClosestDetectedEnemy().position;
        droneTransform.position = Vector3.MoveTowards(droneTransform.position, droneTransform.position - enemyPos, Time.deltaTime);
        Quaternion targetRotation = Quaternion.LookRotation(enemyPos - droneTransform.position);
        droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, targetRotation, Time.deltaTime);
    }
}
