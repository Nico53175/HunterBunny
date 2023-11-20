using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float enemyHealth;
    private int enemyLevel;
    private AnimationCurve enemyHealthCurve;

    public void Initialize(float health, int level, AnimationCurve healthCurve)
    {
        enemyHealth = health;
        enemyLevel = level;
        enemyHealthCurve = healthCurve;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetHealthAtLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHealthAtLevel()
    {
        return enemyHealth * enemyHealthCurve.Evaluate(enemyLevel);
    }
}
