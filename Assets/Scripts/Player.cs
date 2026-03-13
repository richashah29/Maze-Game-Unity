using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text m_FinishText;
    [SerializeField] private TMP_Text timerText;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;       // units per second
    [SerializeField] private float gridSize = 35f;        // size of one grid cell

    [Header("Animation Frames")]
    [SerializeField] private Sprite[] upFrames;          // 2 frames
    [SerializeField] private Sprite[] downFrames;        // 2 frames
    [SerializeField] private Sprite[] leftFrames;        // 2 frames
    [SerializeField] private Sprite[] rightFrames;       // 2 frames
    [SerializeField] private float frameTime = 0.3f;     // seconds per frame
    

    [Header("Grid Bounds")]
    [SerializeField] private int gridWidth = 8;
    [SerializeField] private int gridHeight = 8;
    [SerializeField] private Vector2 gridOrigin = Vector2.zero;
    [SerializeField] private TimerManager timerManager;

    private Vector2 moveInput;
    private Vector3 targetPos;
    private bool isMoving = false;

    private SpriteRenderer sr;
    private Direction lastDirection = Direction.Down;       // default facing down

    private int currentFrame = 0;
    private float frameTimer = 0f;

    private float startTime;

    private InputSystem_Actions inputActions;
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        startTime = Time.time;

        gridOrigin = new Vector2(transform.position.x, transform.position.y);     

        // pass grid origin to enemy spawner
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.gridOrigin = gridOrigin;
            
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        if (m_FinishText != null)
            m_FinishText.text = "";
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        HandleMovement();
        AnimateCharacter();
    }

    private Vector3 GetMoveDirection(Vector2 input)
    {
        Vector3 dir = Vector3.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // horizontal movement takes priority
            if (input.x > 0)
                dir = Vector3.right;
            else if (input.x < 0)
                dir = Vector3.left;
        }
        else if (Mathf.Abs(input.y) > 0)
        {
            // vertical movement
            if (input.y > 0)
                dir = Vector3.up;
            else if (input.y < 0)
                dir = Vector3.down;
        }

        return dir;
    }

    private void HandleMovement()
    {
        if (isMoving) return;

        Vector3 moveDir = GetMoveDirection(moveInput);


        if (moveDir != Vector3.zero)
        {
            timerManager.StartRun();

            // update lastDirection using enum
            if (moveDir == Vector3.up)
                lastDirection = Direction.Up;
            else if (moveDir == Vector3.down)
                lastDirection = Direction.Down;
            else if (moveDir == Vector3.left)
                lastDirection = Direction.Left;
            else if (moveDir == Vector3.right)
                lastDirection = Direction.Right;

            Vector3 proposedTarget = transform.position + moveDir * gridSize;

            float minX = gridOrigin.x;
            float maxX = gridOrigin.x + (gridWidth - 1) * gridSize;

            float minY = gridOrigin.y;
            float maxY = gridOrigin.y + (gridHeight - 1) * gridSize;

            if (proposedTarget.x >= minX && proposedTarget.x <= maxX &&
                proposedTarget.y >= minY && proposedTarget.y <= maxY)
            {
                targetPos = proposedTarget;
                StartCoroutine(MoveToTarget());
            }
            

        }
    }

    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos; // snap exactly
        isMoving = false;
    }

    private void AnimateCharacter()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= frameTime)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % 2; // 2 frames per direction

            switch (lastDirection)
            {
                case Direction.Up:
                    sr.sprite = upFrames[currentFrame];
                    break;
                case Direction.Down:
                    sr.sprite = downFrames[currentFrame];
                    break;
                case Direction.Left:
                    sr.sprite = leftFrames[currentFrame];
                    break;
                case Direction.Right:
                    sr.sprite = rightFrames[currentFrame];
                    break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish") && m_FinishText != null)
        {
            inputActions.Disable();
            timerManager.FinishRun();
        }
    }
}