using System.Collections.Generic;
using UnityEngine;

public class DroneAttackState : IDroneState
{
    private DroneStateManager drone;
    private DroneStateAttackSO droneAttackSettings;

    // Attack Setting
    private float rotationLerpSpeedAttack;

    // Transform
    private Transform droneTransform;
    private List<Transform> detectedEnemies = new List<Transform>();
    private List<LineRenderer> laserRenderers = new List<LineRenderer>();

    public DroneAttackState(DroneStateManager drone,List<LineRenderer> laserRenderers)
    {
        this.drone = drone;
        this.laserRenderers = laserRenderers;

        // Set Drone Settings from SO
        droneAttackSettings = drone.droneAttackSettings;
        rotationLerpSpeedAttack = droneAttackSettings.rotationLerpSpeedAttack;

        // Get needed Components
        droneTransform = drone.GetComponent<Transform>();
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

            Vector3 directionToEnemy = enemyPosition - droneTransform.position;

            Vector3 upVector = Vector3.Dot(droneTransform.up, directionToEnemy) < 0 ? -droneTransform.forward : droneTransform.up; // Code to not flip drone on the head
            Quaternion desiredRotation = Quaternion.LookRotation(directionToEnemy, upVector);
            droneTransform.rotation = Quaternion.Lerp(droneTransform.rotation, desiredRotation, rotationLerpSpeedAttack * Time.deltaTime);

            float attackRayLength = Vector3.Distance(droneTransform.position, enemyPosition); // Get RayLength

            foreach (var laserRenderer in laserRenderers)
            {
                Vector3 laserStartPosition = laserRenderer.transform.position;
                Vector3 laserEndPosition = laserStartPosition + droneTransform.forward * attackRayLength;

                RaycastHit hit;
                if (Physics.Raycast(laserStartPosition, droneTransform.forward, out hit, attackRayLength))
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
            float distance = Vector3.Distance(droneTransform.position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
