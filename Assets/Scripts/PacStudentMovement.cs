
////Explanation of the Code
////Movement Handling:

////HandleInput() captures player input (W, A, S, D) and stores it in lastInput.
////MovePacStudent() checks if PacStudent is already lerping.If not, it determines the direction based on lastInput and moves PacStudent if the target position is walkable.
////Lerping:

////The movement uses Vector3.MoveTowards to interpolate towards the target position, simulating frame-rate-independent movement.
////When PacStudent reaches the target position, isLerping is set to false to stop movement.
////Walkable Check:

////IsWalkable(Vector3 position) is a placeholder function where you can add logic to check if the next position is valid (e.g., not a wall).
////Animation:

////UpdateAnimation(Vector3 direction) updates the animation direction based on the movement. Each direction has an associated integer value for the animation (e.g., 0 for right, 1 for left).
////Audio:

////PlayMovementSound() starts playing the movement audio if PacStudent is lerping.
////StopMovementSound() stops the audio when PacStudent stops moving.

//using UnityEngine;

//public class PacStudentController : MonoBehaviour
//{
//    public float moveSpeed = 5f; // Speed of PacStudent's movement
//    private Vector3 targetPosition; // Target position for lerping
//    private bool isLerping = false; // Check if PacStudent is currently lerping
//    private Vector3 direction = Vector3.zero; // Current movement direction
//    private KeyCode lastInput; // Last input received
//    private KeyCode currentInput; // Current direction being used for movement

//    private Animator animator; // Animator component for direction-based animations
//    private AudioSource audioSource; // Audio source for movement sounds

//    void Start()
//    {
//        // Initialize target position and components
//        targetPosition = transform.position;
//        animator = GetComponent<Animator>();
//        audioSource = GetComponent<AudioSource>();
//    }

//    void Update()
//    {
//        HandleInput();
//        MovePacStudent();
//    }

//    private void HandleInput()
//    {
//        // Capture player input for movement
//        if (Input.GetKeyDown(KeyCode.W)) lastInput = KeyCode.W;
//        else if (Input.GetKeyDown(KeyCode.A)) lastInput = KeyCode.A;
//        else if (Input.GetKeyDown(KeyCode.S)) lastInput = KeyCode.S;
//        else if (Input.GetKeyDown(KeyCode.D)) lastInput = KeyCode.D;
//    }

//    private void MovePacStudent()
//    {
//        if (!isLerping)
//        {
//            Vector3 newDirection = Vector3.zero;

//            // Set direction based on last input
//            if (lastInput == KeyCode.W) newDirection = Vector3.up;
//            else if (lastInput == KeyCode.A) newDirection = Vector3.left;
//            else if (lastInput == KeyCode.S) newDirection = Vector3.down;
//            else if (lastInput == KeyCode.D) newDirection = Vector3.right;

//            // Check if target position in lastInput direction is walkable
//            if (IsWalkable(targetPosition + newDirection))
//            {
//                currentInput = lastInput;
//                direction = newDirection;
//                targetPosition += direction;
//                isLerping = true;
//                UpdateAnimation(direction);
//                PlayMovementSound();
//            }
//            // If lastInput direction is not walkable, check currentInput direction
//            else if (IsWalkable(targetPosition + direction))
//            {
//                targetPosition += direction;
//                isLerping = true;
//                UpdateAnimation(direction);
//                PlayMovementSound();
//            }
//        }

//        // Lerp the position to the target position
//        if (isLerping)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
//            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
//            {
//                transform.position = targetPosition;
//                isLerping = false;
//                StopMovementSound();
//            }
//        }
//    }

//    // Placeholder for checking if the next position is walkable
//    private bool IsWalkable(Vector3 position)
//    {
//        // Implement walkable check logic here (e.g., check if position is not a wall)
//        return true;
//    }

//    // Update animation based on movement direction
//    private void UpdateAnimation(Vector3 direction)
//    {
//        if (direction == Vector3.up)
//        {
//            animator.SetInteger("Direction", 2); // Up
//        }
//        else if (direction == Vector3.down)
//        {
//            animator.SetInteger("Direction", 3); // Down
//        }
//        else if (direction == Vector3.left)
//        {
//            animator.SetInteger("Direction", 1); // Left
//        }
//        else if (direction == Vector3.right)
//        {
//            animator.SetInteger("Direction", 0); // Right
//        }
//    }

//    // Play movement sound if PacStudent is moving
//    private void PlayMovementSound()
//    {
//        if (!audioSource.isPlaying)
//        {
//            audioSource.Play();
//        }
//    }

//    // Stop movement sound when PacStudent stops moving
//    private void StopMovementSound()
//    {
//        if (audioSource.isPlaying)
//        {
//            audioSource.Stop();
//        }
//    }
//}



//////using UnityEngine;

//////public class PacStudentMovement : MonoBehaviour
//////{
//////    public float speed = 2.0f;
//////    private Animator animator;
//////    private Vector2 movementDirection;

//////    void Start()
//////    {
//////        animator = GetComponent<Animator>();
//////        movementDirection = Vector2.right; // Start moving to the right
//////        UpdateAnimatorDirection();
//////    }

//////    void Update()
//////    {
//////        HandleInput();
//////        MoveCharacter();
//////    }

//////    void HandleInput()
//////    {
//////        // Get input from arrow keys or WASD
//////        float moveX = Input.GetAxisRaw("Horizontal");
//////        float moveY = Input.GetAxisRaw("Vertical");

//////        // Prioritize horizontal movement
//////        if (moveX != 0)
//////        {
//////            movementDirection = new Vector2(moveX, 0).normalized;
//////            UpdateAnimatorDirection();
//////        }
//////        else if (moveY != 0)
//////        {
//////            movementDirection = new Vector2(0, moveY).normalized;
//////            UpdateAnimatorDirection();
//////        }
//////    }

//////    void MoveCharacter()
//////    {
//////        Vector2 currentPosition = transform.position;
//////        Vector2 targetPosition = currentPosition + movementDirection * speed * Time.deltaTime;
//////        transform.position = targetPosition;
//////    }

//////    void UpdateAnimatorDirection()
//////    {
//////        if (movementDirection.x > 0)
//////        {
//////            animator.SetInteger("Direction", 0); // Right
//////        }
//////        else if (movementDirection.x < 0)
//////        {
//////            animator.SetInteger("Direction", 1); // Left
//////        }
//////        else if (movementDirection.y > 0)
//////        {
//////            animator.SetInteger("Direction", 2); // Up
//////        }
//////        else if (movementDirection.y < 0)
//////        {
//////            animator.SetInteger("Direction", 3); // Down
//////        }
//////    }
//////}



////using UnityEngine;

////public class PacStudentMovement : MonoBehaviour
////{
////    public float speed = 2.0f;
////    private Vector2[] waypoints;
////    private int currentWaypointIndex = 0;

////    private Animator animator;
////    private AudioSource audioSource;

////    void Start()
////    {
////        animator = GetComponent<Animator>();
////        audioSource = GetComponent<AudioSource>();

////        // Define the waypoints (Replace with your actual coordinates)
////        waypoints = new Vector2[]
////        {
////            new Vector2(-7, 7),  // Point A
////            new Vector2(7, 7),   // Point B
////            new Vector2(7, -7),  // Point C
////            new Vector2(-7, -7), // Point D
////        };

////        // Set initial position
////        transform.position = waypoints[currentWaypointIndex];

////        // Start moving audio
////        audioSource.Play();
////    }

////    void Update()
////    {
////        MoveAlongPath();
////        UpdateAnimation();
////    }

////    void MoveAlongPath()
////    {
////        // Get current position and target waypoint
////        Vector2 currentPosition = transform.position;
////        Vector2 targetPosition = waypoints[currentWaypointIndex];

////        // Move towards the target waypoint
////        float step = speed * Time.deltaTime;
////        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

////        // Check if reached the target waypoint
////        if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
////        {
////            // Proceed to the next waypoint
////            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
////        }
////    }

////    void UpdateAnimation()
////    {
////        // Determine movement direction
////        Vector2 direction = waypoints[currentWaypointIndex] - (Vector2)transform.position;

////        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
////        {
////            // Horizontal movement
////            if (direction.x > 0)
////                animator.SetInteger("Direction", 0); // Right
////            else if (direction.x < 0)
////                animator.SetInteger("Direction", 1); // Left
////        }
////        else
////        {
////            // Vertical movement
////            if (direction.y > 0)
////                animator.SetInteger("Direction", 2); // Up
////            else if (direction.y < 0)
////                animator.SetInteger("Direction", 3); // Down
////        }
////    }
////}
