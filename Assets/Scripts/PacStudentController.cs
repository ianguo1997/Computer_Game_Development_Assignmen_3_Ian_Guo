


//Explanation of Key Parts
//HandleInput(): Captures movement input (W, A, S, D) and stores it in lastInput.
//MovePacStudent(): Moves PacStudent using lerping based on lastInput and currentInput.
//IsWalkable(Vector3 position): Placeholder function to check if a grid position is walkable.
//UpdateAnimation(Vector3 direction): Updates the animation based on movement direction.
//PlayMovementSound() and StopMovementSound(): Manages audio based on movement.
//IsNextPositionPellet(Vector3 position): Placeholder for detecting pellets in the next position.
//dustEffect: Particle system to play dust effect when moving.

using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of PacStudent's movement
    private Vector3 targetPosition; // Target position for lerping
    private bool isLerping = false; // Check if PacStudent is currently lerping
    private Vector3 direction = Vector3.zero; // Current movement direction
    private KeyCode lastInput; // Last input received
    private KeyCode currentInput; // Current direction being used for movement

    private Animator animator; // Animator component for direction-based animations
    private AudioSource audioSource; // Audio source for movement sounds
    public AudioClip pelletClip; // Audio when moving toward a pellet
    public AudioClip movementClip; // Audio when moving without pellet
    public ParticleSystem dustEffect; // Particle system for dust

    void Start()
    {
        // Initialize target position and components
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInput();
        MovePacStudent();
    }

    private void HandleInput()
    {
        // Capture player input for movement
        if (Input.GetKeyDown(KeyCode.W)) lastInput = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = KeyCode.D;
    }

    private void MovePacStudent()
    {
        if (!isLerping)
        {
            Vector3 newDirection = Vector3.zero;

            // Set direction based on last input
            if (lastInput == KeyCode.W) newDirection = Vector3.up;
            else if (lastInput == KeyCode.A) newDirection = Vector3.left;
            else if (lastInput == KeyCode.S) newDirection = Vector3.down;
            else if (lastInput == KeyCode.D) newDirection = Vector3.right;

            // Check if target position in lastInput direction is walkable
            if (IsWalkable(targetPosition + newDirection))
            {
                currentInput = lastInput;
                direction = newDirection;
                targetPosition += direction;
                isLerping = true;
                UpdateAnimation(direction);
                PlayMovementSound();
                dustEffect.Play();
            }
            // If lastInput direction is not walkable, check currentInput direction
            else if (IsWalkable(targetPosition + direction))
            {
                targetPosition += direction;
                isLerping = true;
                UpdateAnimation(direction);
                PlayMovementSound();
                dustEffect.Play();
            }
        }

        // Lerp the position to the target position
        if (isLerping)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isLerping = false;
                StopMovementSound();
                dustEffect.Stop();
            }
        }
    }

    // Placeholder for checking if the next position is walkable
    private bool IsWalkable(Vector3 position)
    {
        // Implement walkable check logic here (e.g., check if position is not a wall)
        return true;
    }

    // Update animation based on movement direction
    private void UpdateAnimation(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            animator.SetInteger("Direction", 2); // Up
        }
        else if (direction == Vector3.down)
        {
            animator.SetInteger("Direction", 3); // Down
        }
        else if (direction == Vector3.left)
        {
            animator.SetInteger("Direction", 1); // Left
        }
        else if (direction == Vector3.right)
        {
            animator.SetInteger("Direction", 0); // Right
        }
    }

    // Play movement sound if PacStudent is moving
    private void PlayMovementSound()
    {
        if (audioSource.isPlaying) return;

        // Decide which sound to play based on whether a pellet is ahead
        audioSource.clip = (IsNextPositionPellet(targetPosition + direction)) ? pelletClip : movementClip;
        audioSource.Play();
    }

    // Stop movement sound when PacStudent stops moving
    private void StopMovementSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Placeholder to check if the next position contains a pellet
    private bool IsNextPositionPellet(Vector3 position)
    {
        // Implement logic to check for pellet in the next position
        return false; // Replace with actual condition
    }
}
