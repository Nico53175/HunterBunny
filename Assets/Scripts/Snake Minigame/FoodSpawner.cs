using System.Collections;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab;
    [SerializeField] float checkInterval = 1f; // Time interval to check for the presence of food

    void Start()
    {
        StartCoroutine(CheckAndSpawnFood());
    }

    IEnumerator CheckAndSpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (GameObject.FindGameObjectWithTag("Food") == null)
            {
                SpawnFood();
            }
        }
    }

    void SpawnFood()
    {
        Vector2 minWorldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector2 maxWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Ensure that values are divisible by 2.
        int randomX = Mathf.FloorToInt(Random.Range(minWorldPoint.x, maxWorldPoint.x) / 2) * 2;
        int randomY = Mathf.FloorToInt(Random.Range(minWorldPoint.y, maxWorldPoint.y) / 2) * 2;

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
