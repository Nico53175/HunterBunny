using UnityEngine;
using System.Collections.Generic;
using System.Data;

public class DroneAttackState : IDroneState
{
    private DroneStateManager drone;
    private DroneStateAttackSO droneAttackSettings;

    private List<Transform> detectedEnemies = new List<Transform>();
    private float rayDisplayDuration;
    private float rayHideTime;
    private float rotationLerpSpeedAttack;
    private List<LineRenderer> laserRenderers = new List<LineRenderer>(); 

    public DroneAttackState(DroneStateManager drone,List<LineRenderer> laserRenderers)
    {
        this.drone = drone;
        this.laserRenderers = laserRenderers;
        droneAttackSettings = drone.droneAttackSettings;
        rayDisplayDuration = droneAttackSettings.rayDisplayDuration;
        rotationLerpSpeedAttack = droneAttackSettings.rotationLerpSpeedAttack;
    }

    public void Enter()
    {
        foreach (LineRenderer renderer in laserRenderers)
        {
            renderer.enabled = true;
        }
    }

    public void Execute()
    {
        detectedEnemies = drone.detectedEnemies;
        if (detectedEnemies.Count > 0)
        {
            Transform enemy = GetClosestDetectedEnemy();
            Vector3 enemyPosition = enemy.position;
            bool enemyDestroyed = false;

            Vector3 directionToEnemy = enemyPosition - drone.transform.position;

            Vector3 upVector = Vector3.Dot(drone.transform.up, directionToEnemy) < 0 ? -drone.transform.forward : drone.transform.up;

            Quaternion desiredRotation = Quaternion.LookRotation(directionToEnemy, upVector);
            drone.transform.rotation = Quaternion.Lerp(drone.transform.rotation, desiredRotation, rotationLerpSpeedAttack * Time.deltaTime);

            float attackRayLength = Vector3.Distance(drone.transform.position, enemyPosition);

            foreach (var laserRenderer in laserRenderers)
            {
                Vector3 laserStartPosition = laserRenderer.transform.position;
                Vector3 laserEndPosition = laserStartPosition + drone.transform.forward * attackRayLength;

                RaycastHit hit;
                if (Physics.Raycast(laserStartPosition, drone.transform.forward, out hit, attackRayLength))
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        drone.DestroyEnemy(hit.transform.gameObject);
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
            drone.SetState(drone.GetHoverState());
        }
    }

    public void Exit()
    {
        foreach (LineRenderer renderer in laserRenderers)
        {
            renderer.enabled = false;
        }
    }

    private void DrawRay(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private Transform GetClosestDetectedEnemy()
    {
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Transform enemy in detectedEnemies)
        {
            float distance = Vector3.Distance(drone.transform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
