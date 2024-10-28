using UnityEngine;

public class GhostStateCycler : MonoBehaviour
{
    private Animator animator;
    private float stateTimer;
    private int currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        stateTimer = 0f;
        currentState = 0;
        animator.SetInteger("State", currentState);
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer >= 3f)
        {
            stateTimer = 0f;
            currentState = (currentState + 1) % 4; // Cycle through states 0 to 3
            animator.SetInteger("State", currentState);
        }
    }
}
