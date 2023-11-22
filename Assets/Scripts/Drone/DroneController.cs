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
        droneLevel = drone.level;

        if (healthSystem != null)
        {
            healthSystem.Initialize(drone.health, drone.level, drone.healthCurve, this);
        }

        if (damageSystem != null)
        {
            damageSystem.Initialize(drone.damage, drone.level, drone.damageCurve, null);
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
