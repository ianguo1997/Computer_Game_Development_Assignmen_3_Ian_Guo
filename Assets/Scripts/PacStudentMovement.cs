using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private Animator animator;

    private Vector2[] waypoints = new Vector2[]
    {
        new Vector2(-4, 4),
        new Vector2(4, 4),
        new Vector2(4, -4),
        new Vector2(-4, -4)
    };

    private int currentWaypointIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = waypoints[currentWaypointIndex];
        //animator.SetInteger("Direction", -1);
    }

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        Vector2 targetPosition = waypoints[(currentWaypointIndex + 1) % waypoints.Length];
        Vector2 currentPosition = transform.position;

        // Move towards the next waypoint
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        // Check if the position matches the target
        if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
        {
            // Update to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Determine direction and set Animator parameter
        Vector2 direction = targetPosition - currentPosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement
            if (direction.x > 0)
                animator.SetInteger("Direction", 0); // Right
            else
                animator.SetInteger("Direction", 1); // Left
        }
        else
        {
            // Vertical movement
            if (direction.y > 0)
                animator.SetInteger("Direction", 2); // Up
            else
                animator.SetInteger("Direction", 3); // Down
        }
    }
}
