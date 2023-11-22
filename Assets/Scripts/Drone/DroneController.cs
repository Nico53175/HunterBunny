using UnityEngine;
using UnityEngine.Events;

public class DroneController : MonoBehaviour, IEntityEventSubscriber
{
    [SerializeField] DroneSO drone;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private DamageSystem damageSystem;

    private int droneLevel;

    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();
    private void Awake()
    {
        droneLevel = drone.droneLevel;

        if (healthSystem != null)
        {
            healthSystem.Initialize(drone.droneHealth, drone.droneLevel, drone.healthCurve, this);
        }

        if (damageSystem != null)
        {
            damageSystem.Initialize(drone.droneDamage, drone.droneLevel, drone.damageCurve, null);
        }
    }

    public void SubscribeToLevelUp(UnityAction<int> callback)
    {
        OnLevelUp.AddListener(callback);
    }

    public void UnsubscribeFromLevelUp(UnityAction<int> callback)
    {
        OnLevelUp.RemoveListener(callback);
    }

    public void LevelUp()
    {
        droneLevel++;
        OnLevelUp.Invoke(droneLevel);
    }
}
