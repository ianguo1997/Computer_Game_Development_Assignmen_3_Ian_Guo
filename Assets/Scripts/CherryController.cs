using UnityEngine;

public class CherryController : MonoBehaviour
{
    public float spawnInterval = 10f;  // Time between cherry spawns
    public float moveSpeed = 5f;       // Speed of cherry movement
    private Vector3 startPosition;     // Initial spawn position
    private Vector3 endPosition;       // Target position to move towards
    private float spawnTimer;          // Timer for spawn interval

    void Start()
    {
        // Initialize the timer with the interval
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        // Update the spawn timer
        spawnTimer -= Time.deltaTime;

        // Check if it's time to spawn a new cherry
        if (spawnTimer <= 0)
        {
            SpawnCherry();
            spawnTimer = spawnInterval; // Reset the spawn timer
        }

        // Move the cherry if it has been spawned
        MoveCherry();
    }

    private void SpawnCherry()
    {
        // Define random spawn position outside the camera view
        float x, y;
        int side = Random.Range(0, 4); // Choose a random side (0: left, 1: top, 2: right, 3: bottom)

        // Set the starting and ending positions based on which side the cherry spawns on
        switch (side)
        {
            case 0: // Left side
                x = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x - 1;
                y = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y,
                                 Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
                startPosition = new Vector3(x, y, 0);
                endPosition = new Vector3(-startPosition.x, startPosition.y, 0); // Move to the opposite side
                break;

            case 1: // Top side
                x = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
                                 Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
                y = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)).y + 1;
                startPosition = new Vector3(x, y, 0);
                endPosition = new Vector3(startPosition.x, -startPosition.y, 0);
                break;

            case 2: // Right side
                x = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x + 1;
                y = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y,
                                 Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
                startPosition = new Vector3(x, y, 0);
                endPosition = new Vector3(-startPosition.x, startPosition.y, 0);
                break;

            default: // Bottom side
                x = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
                                 Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
                y = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).y - 1;
                startPosition = new Vector3(x, y, 0);
                endPosition = new Vector3(startPosition.x, -startPosition.y, 0);
                break;
        }

        // Set cherry position to the start position and enable it if it was disabled
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    private void MoveCherry()
    {
        // Check if the cherry has been spawned (position has been set)
        if (transform.position != endPosition)
        {
            // Move towards the end position using linear interpolation
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

            // Destroy the cherry when it reaches the end position
            if (Vector3.Distance(transform.position, endPosition) < 0.1f)
            {
                Destroy(gameObject); // Destroy or deactivate the cherry
            }
        }
    }
}
