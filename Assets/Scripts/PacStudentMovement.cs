using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private Animator animator;
    private Vector2 movementDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        movementDirection = Vector2.right; // Start moving to the right
        UpdateAnimatorDirection();
    }

    void Update()
    {
        HandleInput();
        MoveCharacter();
    }

    void HandleInput()
    {
        // Get input from arrow keys or WASD
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Prioritize horizontal movement
        if (moveX != 0)
        {
            movementDirection = new Vector2(moveX, 0).normalized;
            UpdateAnimatorDirection();
        }
        else if (moveY != 0)
        {
            movementDirection = new Vector2(0, moveY).normalized;
            UpdateAnimatorDirection();
        }
    }

    void MoveCharacter()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + movementDirection * speed * Time.deltaTime;
        transform.position = targetPosition;
    }

    void UpdateAnimatorDirection()
    {
        if (movementDirection.x > 0)
        {
            animator.SetInteger("Direction", 0); // Right
        }
        else if (movementDirection.x < 0)
        {
            animator.SetInteger("Direction", 1); // Left
        }
        else if (movementDirection.y > 0)
        {
            animator.SetInteger("Direction", 2); // Up
        }
        else if (movementDirection.y < 0)
        {
            animator.SetInteger("Direction", 3); // Down
        }
    }
}
