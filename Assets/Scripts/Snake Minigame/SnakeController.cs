using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] GameObject snakeSegment;
    [SerializeField] float timerDuration;
    float currentTime;
    Vector2 movement = Vector2.right; // Start by moving right
    List<GameObject> snake;
    List<Vector3> path = new List<Vector3>();
    float screenTop;
    float screenBottom;
    float screenRight;
    float screenLeft;

    void Start()
    {
        screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        snake = new List<GameObject>
        {
            this.gameObject
        };
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0) // Only update movement direction if there's input
        {
            if (Mathf.Abs(x) > Mathf.Abs(y))
                movement = new Vector2(x, 0).normalized;
            else
                movement = new Vector2(0, y).normalized;
        }

        if (currentTime >= timerDuration)
        {
            Vector3 newPosition = snake[0].transform.position + (Vector3)movement;
            path.Insert(0, newPosition); // Add new position to the start of the path

            snake[0].transform.position = newPosition;
            currentTime = 0;

            for (int i = 1; i < snake.Count; i++)
            {
                snake[i].transform.position = path[i];
            }

            // Remove any excessive positions that aren't needed anymore
            if (path.Count > snake.Count)
            {
                path.RemoveAt(path.Count - 1);
            }
        }
        WrapAround();
    }

    void WrapAround()
    {
        Vector3 pos = snake[0].transform.position;

        // Check if snake head is outside the boundaries and wrap around
        if (pos.y > screenTop)
        {
            pos.y = screenBottom;
        }
        else if (pos.y < screenBottom)
        {
            pos.y = screenTop;
        }

        if (pos.x > screenRight)
        {
            pos.x = screenLeft;
        }
        else if (pos.x < screenLeft)
        {
            pos.x = screenRight;
        }

        // Update the position of the snake head
        snake[0].transform.position = pos;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            AddSegment();
            Destroy(collision.gameObject); // Destroy the food
        }

        if (collision.gameObject.CompareTag("Segment"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    void AddSegment()
    {
        // Get the last segment's position
        Vector3 lastSegmentPos = snake[snake.Count - 1].transform.position;

        // Adjust the spawn position based on the movement direction
        Vector3 spawnPosition = lastSegmentPos - (Vector3)movement;

        // Create a new segment at the adjusted position and add it to the snake list
        GameObject newSegment = Instantiate(snakeSegment, spawnPosition, Quaternion.identity);
        snake.Add(newSegment);
    }
}
