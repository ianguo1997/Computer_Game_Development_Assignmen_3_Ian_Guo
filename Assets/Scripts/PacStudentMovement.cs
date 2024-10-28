//using UnityEngine;

//public class PacStudentMovement : MonoBehaviour
//{
//    public float speed = 2.0f;
//    private Animator animator;
//    private Vector2 movementDirection;

//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        movementDirection = Vector2.right; // Start moving to the right
//        UpdateAnimatorDirection();
//    }

//    void Update()
//    {
//        HandleInput();
//        MoveCharacter();
//    }

//    void HandleInput()
//    {
//        // Get input from arrow keys or WASD
//        float moveX = Input.GetAxisRaw("Horizontal");
//        float moveY = Input.GetAxisRaw("Vertical");

//        // Prioritize horizontal movement
//        if (moveX != 0)
//        {
//            movementDirection = new Vector2(moveX, 0).normalized;
//            UpdateAnimatorDirection();
//        }
//        else if (moveY != 0)
//        {
//            movementDirection = new Vector2(0, moveY).normalized;
//            UpdateAnimatorDirection();
//        }
//    }

//    void MoveCharacter()
//    {
//        Vector2 currentPosition = transform.position;
//        Vector2 targetPosition = currentPosition + movementDirection * speed * Time.deltaTime;
//        transform.position = targetPosition;
//    }

//    void UpdateAnimatorDirection()
//    {
//        if (movementDirection.x > 0)
//        {
//            animator.SetInteger("Direction", 0); // Right
//        }
//        else if (movementDirection.x < 0)
//        {
//            animator.SetInteger("Direction", 1); // Left
//        }
//        else if (movementDirection.y > 0)
//        {
//            animator.SetInteger("Direction", 2); // Up
//        }
//        else if (movementDirection.y < 0)
//        {
//            animator.SetInteger("Direction", 3); // Down
//        }
//    }
//}



using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private Vector2[] waypoints;
    private int currentWaypointIndex = 0;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Define the waypoints (Replace with your actual coordinates)
        waypoints = new Vector2[]
        {
            new Vector2(-7, 7),  // Point A
            new Vector2(7, 7),   // Point B
            new Vector2(7, -7),  // Point C
            new Vector2(-7, -7), // Point D
        };

        // Set initial position
        transform.position = waypoints[currentWaypointIndex];

        // Start moving audio
        audioSource.Play();
    }

    void Update()
    {
        MoveAlongPath();
        UpdateAnimation();
    }

    void MoveAlongPath()
    {
        // Get current position and target waypoint
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = waypoints[currentWaypointIndex];

        // Move towards the target waypoint
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        // Check if reached the target waypoint
        if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
        {
            // Proceed to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void UpdateAnimation()
    {
        // Determine movement direction
        Vector2 direction = waypoints[currentWaypointIndex] - (Vector2)transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement
            if (direction.x > 0)
                animator.SetInteger("Direction", 0); // Right
            else if (direction.x < 0)
                animator.SetInteger("Direction", 1); // Left
        }
        else
        {
            // Vertical movement
            if (direction.y > 0)
                animator.SetInteger("Direction", 2); // Up
            else if (direction.y < 0)
                animator.SetInteger("Direction", 3); // Down
        }
    }
}
