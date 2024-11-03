using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public LevelGenerator levelGenerator; // Reference to LevelGenerator to access tileSize and levelMap
    private Vector2 targetPosition; // Target position to lerp towards
    private Vector2Int gridPosition; // Current grid position on the map
    private bool isMoving = false; // Flag to check if PacStudent is moving
    private string lastInput = ""; // Track last player input for direction
    private string currentInput = ""; // Store the direction PacStudent is moving

    public float moveSpeed = 5f; // Speed of lerping
    public Animator animator; // Reference to the Animator component

    public AudioSource movementAudioSource; // Audio source for movement
    public AudioSource eatingAudioSource; // Audio source for pellet eating
    public AudioClip movementClip; // Movement audio clip
    public AudioClip eatingClip; // Eating audio clip

    void Start()
    {
        // Initialize grid position and target position based on current position
        gridPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        targetPosition = transform.position;

        // Assign audio clips to sources
        movementAudioSource.clip = movementClip;
        eatingAudioSource.clip = eatingClip;
    }

    void Update()
    {
        HandleInput(); // Get input and set movement direction

        if (!isMoving)
        {
            MovePacStudent();
        }

        // Lerp PacStudent's position if moving
        if (isMoving)
        {
            LerpToTarget();
        }

        UpdateAnimation();
    }

    // Get input from the player
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = "Up";
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = "Down";
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = "Left";
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = "Right";
    }

    // Check and move PacStudent if a grid position is walkable
    private void MovePacStudent()
    {
        Vector2Int direction = Vector2Int.zero;

        switch (lastInput)
        {
            case "Up": direction = Vector2Int.up; break;
            case "Down": direction = Vector2Int.down; break;
            case "Left": direction = Vector2Int.left; break;
            case "Right": direction = Vector2Int.right; break;
        }

        Vector2Int newPosition = gridPosition + direction;

        // Check if new position is within bounds and walkable
        if (IsWalkable(newPosition))
        {
            currentInput = lastInput;
            gridPosition = newPosition;
            targetPosition = (Vector2)newPosition * levelGenerator.tileSize;
            isMoving = true;

            // Check if the target tile has a pellet (e.g., value 5 in levelMap)
            if (levelGenerator.levelMap[gridPosition.y, gridPosition.x] == 5)
            {
                PlayEatingAudio();
            }
            else
            {
                PlayMovementAudio();
            }
        }
    }

    // Lerp PacStudent to the target position
    private void LerpToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == targetPosition)
        {
            isMoving = false;
            StopAudio();
        }
    }

    // Check if a grid position is walkable based on levelMap
    private bool IsWalkable(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;

        // Return true if within bounds and is walkable (e.g., not walls in levelMap)
        return x >= 0 && y >= 0 && x < levelGenerator.levelMap.GetLength(1) &&
               y < levelGenerator.levelMap.GetLength(0) &&
               (levelGenerator.levelMap[y, x] == 0 || levelGenerator.levelMap[y, x] == 5);
    }

    // Update the animation state based on movement direction
    private void UpdateAnimation()
    {
        if (isMoving)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("MoveX", gridPosition.x - (int)targetPosition.x);
            animator.SetFloat("MoveY", gridPosition.y - (int)targetPosition.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Play movement audio
    private void PlayMovementAudio()
    {
        if (!movementAudioSource.isPlaying)
        {
            movementAudioSource.Play();
        }
        eatingAudioSource.Stop();
    }

    // Play eating audio
    private void PlayEatingAudio()
    {
        if (!eatingAudioSource.isPlaying)
        {
            eatingAudioSource.Play();
        }
        movementAudioSource.Stop();
    }

    // Stop both audio sources when movement stops
    private void StopAudio()
    {
        movementAudioSource.Stop();
        eatingAudioSource.Stop();
    }
}





//using UnityEngine;
//using System.Collections;



//public class PacStudentController : MonoBehaviour
//{


//    private LevelGenerator levelGenerator; // Reference to the LevelGenerator script

//    public float speed = 2f;
//    private Vector2Int gridPosition;
//    private Vector3 targetPosition;
//    private bool isLerping;
//    private Animator animator;

//    void Start()
//    {
//        // Find and reference the LevelGenerator component
//        levelGenerator = FindObjectOfType<LevelGenerator>();

//        // Access tileSize and tilemap
//        float tileSize = levelGenerator.tileSize;
//        Tilemap tilemap = levelGenerator.tilemap;
//        gridPosition = new Vector2Int(1, 1);
//        targetPosition = new Vector3(gridPosition.x * tileSize, -gridPosition.y * tileSize, 0);
//        transform.position = targetPosition;

//        animator = GetComponent<Animator>(); // 获取Animator组件
//    }

//    void Update()
//    {
//        if (!isLerping)
//        {
//            Vector2Int direction = Vector2Int.zero;

//            // Gather input for W, A, S, D
//            if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
//            else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
//            else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
//            else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;

//            Vector2Int newGridPosition = gridPosition + direction;
//            if (IsWalkable(newGridPosition))
//            {
//                gridPosition = newGridPosition;
//                targetPosition = new Vector3(gridPosition.x * tileSize, -gridPosition.y * tileSize, 0);
//                StartCoroutine(LerpToPosition(direction));
//            }
//        }
//    }

//    bool IsWalkable(Vector2Int pos)
//    {
//        // 根据levelMap检查该位置是否可行走
//        return levelMap[pos.y, pos.x] != 2;
//    }

//    IEnumerator LerpToPosition(Vector2Int direction)
//    {
//        isLerping = true;
//        UpdateAnimation(true, direction); // 开始移动时更新动画
//        float elapsed = 0f;

//        Vector3 startPosition = transform.position;
//        while (elapsed < 1f)
//        {
//            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed);
//            elapsed += Time.deltaTime * speed;
//            yield return null;
//        }

//        transform.position = targetPosition;
//        isLerping = false;
//        UpdateAnimation(false, Vector2Int.zero); // 停止移动时停止动画
//    }

//    void UpdateAnimation(bool isMoving, Vector2Int direction)
//    {
//        animator.SetBool("isMoving", isMoving);

//        if (isMoving)
//        {
//            // 根据方向设置动画触发
//            if (direction == Vector2Int.up)
//                animator.SetTrigger("MoveUp");
//            else if (direction == Vector2Int.down)
//                animator.SetTrigger("MoveDown");
//            else if (direction == Vector2Int.left)
//                animator.SetTrigger("MoveLeft");
//            else if (direction == Vector2Int.right)
//                animator.SetTrigger("MoveRight");
//        }
//    }
//}




////public class PacStudentController : MonoBehaviour
////{
////    // 定义变量来存储玩家输入和运动状态
////    public float speed = 5f; // 速度，表示PacStudent的移动速度
////    private Vector3 targetPosition; // 目标位置
////    private Vector2Int gridPosition;
////    private bool isLerping = false; // 是否正在lerping状态
////    private string lastInput; // 上一次输入的方向
////    private string currentInput; // 当前移动的方向



////    public AudioClip pelletAudioClip; // 播放吃豆音效
////    public AudioClip moveAudioClip;   // 普通移动音效
////    private AudioSource audioSource;


////    private Animator animator;

////    void Start()
////    {
////        // 初始化目标位置为当前所在位置
////        //targetPosition = transform.position;

////        gridPosition = new Vector2Int(1, 1);
////        targetPosition = new Vector3(gridPosition.x * tileSize, -gridPosition.y * tileSize, 0);
////        transform.position = targetPosition;

////        animator = GetComponent<Animator>(); // 获取Animator组件
////        audioSource = GetComponent<AudioSource>();

////        //animator = GetComponent<Animator>();
////    }

////    bool IsWalkable(Vector2Int pos)
////    {
////        // 根据levelMap检查该位置是否可行走
////        return levelMap[pos.y, pos.x] != 2;
////    }

////    void Update()
////    {
////        if (!isLerping)
////        {
////            Vector2Int direction = Vector2Int.zero;

////            // Gather input for W, A, S, D
////            if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
////            else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
////            else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
////            else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;

////            Vector2Int newGridPosition = gridPosition + direction;
////            if (IsWalkable(newGridPosition))
////            {
////                gridPosition = newGridPosition;
////                targetPosition = new Vector3(gridPosition.x * tileSize, -gridPosition.y * tileSize, 0);
////                StartCoroutine(LerpToPosition(direction));
////            }
////        }
////    }


////    IEnumerator LerpToPosition(Vector2Int direction)
////    {
////        isLerping = true;
////        UpdateAnimation(true, direction); // 开始移动时更新动画
////        float elapsed = 0f;

////        Vector3 startPosition = transform.position;
////        while (elapsed < 1f)
////        {
////            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed);
////            elapsed += Time.deltaTime * speed;
////            yield return null;
////        }

////        transform.position = targetPosition;
////        isLerping = false;
////        UpdateAnimation(false, Vector2Int.zero); // 停止移动时停止动画
////    }


////    private void UpdateAnimation(bool isMoving)
////    {
////        // 根据是否在移动来播放动画
////        animator.SetBool("isMoving", isMoving);
////    }

////    private void PlayAudioForMovement(bool isEatingPellet)
////    {
////        // 根据是否吃豆来播放不同音效
////        audioSource.clip = isEatingPellet ? pelletAudioClip : moveAudioClip;
////        if (!audioSource.isPlaying)
////        {
////            audioSource.Play();
////        }
////    }

////    private void StopAudio()
////    {
////        audioSource.Stop();
////    }



////    private void CheckPlayerInput()
////    {
////        // 检查玩家按下的按键并存储到lastInput中
////        if (Input.GetKeyDown(KeyCode.W)) lastInput = "up";
////        if (Input.GetKeyDown(KeyCode.S)) lastInput = "down";
////        if (Input.GetKeyDown(KeyCode.A)) lastInput = "left";
////        if (Input.GetKeyDown(KeyCode.D)) lastInput = "right";
////    }

////    private void LerpToTarget()
////    {
////        // 使用Lerp方法平滑移动到目标位置
////        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

////        // 如果已经接近目标位置，停止lerping
////        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
////        {
////            transform.position = targetPosition;
////            isLerping = false;
////        }
////    }

////    private void MovePacStudent()
////    {
////        Vector3 direction = Vector3.zero;

////        // 根据lastInput决定方向
////        if (lastInput == "up") direction = Vector3.up;
////        else if (lastInput == "down") direction = Vector3.down;
////        else if (lastInput == "left") direction = Vector3.left;
////        else if (lastInput == "right") direction = Vector3.right;

////        // 计算目标位置
////        Vector3 potentialTarget = transform.position + direction;

////        // 检查目标位置是否可走
////        if (IsWalkable(potentialTarget))
////        {
////            // 如果可走，设置currentInput并进入lerping状态
////            currentInput = lastInput;
////            targetPosition = potentialTarget;
////            isLerping = true;
////        }
////        else if (!string.IsNullOrEmpty(currentInput))
////        {
////            // 尝试使用currentInput的方向
////            if (currentInput == "up") direction = Vector3.up;
////            else if (currentInput == "down") direction = Vector3.down;
////            else if (currentInput == "left") direction = Vector3.left;
////            else if (currentInput == "right") direction = Vector3.right;

////            potentialTarget = transform.position + direction;
////            if (IsWalkable(potentialTarget))
////            {
////                targetPosition = potentialTarget;
////                isLerping = true;
////            }
////        }



////    }
////    private bool IsWalkable(Vector3 position)
////    {
////        // 假设此处有逻辑来检测该位置是否可走
////        // 返回true表示可走，false表示不可走
////        // 具体实现将依赖于您的游戏设计和关卡生成逻辑
////        return true; // 在实际代码中需要替换
////    }


////}




//////Explanation of Key Parts
//////HandleInput(): Captures movement input (W, A, S, D) and stores it in lastInput.
//////MovePacStudent(): Moves PacStudent using lerping based on lastInput and currentInput.
//////IsWalkable(Vector3 position): Placeholder function to check if a grid position is walkable.
//////UpdateAnimation(Vector3 direction): Updates the animation based on movement direction.
//////PlayMovementSound() and StopMovementSound(): Manages audio based on movement.
//////IsNextPositionPellet(Vector3 position): Placeholder for detecting pellets in the next position.
//////dustEffect: Particle system to play dust effect when moving.






