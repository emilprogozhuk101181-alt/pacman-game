using UnityEngine;

public class PacmanController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public int tileSize = 32;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] mouthSprites; // For animation

    private Vector2Int gridPosition;
    private Vector2Int nextGridPosition;
    private Vector2Int queuedDirection = Vector2Int.zero;
    private Vector2Int currentDirection = Vector2Int.zero;
    private Vector3 startPosition;
    private float animationTimer = 0f;
    private int currentMouthFrame = 0;

    private MazeGenerator maze;
    private bool canMove = true;

    private void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
        startPosition = transform.position;
        gridPosition = maze.WorldToGridPosition(transform.position);
        nextGridPosition = gridPosition;
    }

    private void Update()
    {
        HandleInput();
        MoveTowardNextTile();
        AnimateMouth();
    }

    private void HandleInput()
    {
        // Get input and queue direction
        if (Input.GetKeyDown(KeyCode.Up))
            queuedDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.Down))
            queuedDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.Left))
            queuedDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.Right))
            queuedDirection = Vector2Int.right;
    }

    private void MoveTowardNextTile()
    {
        // Check if we're at a grid position
        if (Vector3.Distance(transform.position, maze.GridToWorldPosition(gridPosition.x, gridPosition.y)) < 0.1f)
        {
            transform.position = maze.GridToWorldPosition(gridPosition.x, gridPosition.y);

            // Try to move in queued direction
            if (CanMoveTo(gridPosition + queuedDirection))
            {
                currentDirection = queuedDirection;
            }

            // Move in current direction if possible
            if (CanMoveTo(gridPosition + currentDirection))
            {
                gridPosition += currentDirection;
            }

            nextGridPosition = gridPosition;
        }

        // Smoothly move toward next tile
        Vector3 targetPos = maze.GridToWorldPosition(gridPosition.x, gridPosition.y);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Update rotation based on direction
        UpdateRotation();
    }

    private bool CanMoveTo(Vector2Int newPos)
    {
        // Handle tunnel wrapping
        if (newPos.x < 0)
            newPos.x = maze.GetWidth() - 1;
        else if (newPos.x >= maze.GetWidth())
            newPos.x = 0;

        // Check walls
        if (maze.IsWall(newPos.x, newPos.y))
            return false;

        return true;
    }

    private void UpdateRotation()
    {
        if (currentDirection == Vector2Int.up)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (currentDirection == Vector2Int.down)
            transform.rotation = Quaternion.Euler(0, 0, -90);
        else if (currentDirection == Vector2Int.left)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (currentDirection == Vector2Int.right)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void AnimateMouth()
    {
        if (mouthSprites.Length == 0) return;

        animationTimer += Time.deltaTime;
        if (animationTimer > 0.1f)
        {
            animationTimer = 0f;
            currentMouthFrame = (currentMouthFrame + 1) % mouthSprites.Length;
            spriteRenderer.sprite = mouthSprites[currentMouthFrame];
        }
    }

    public Vector2Int GetGridPosition() => gridPosition;

    public void ResetPosition()
    {
        transform.position = startPosition;
        gridPosition = maze.WorldToGridPosition(startPosition);
        nextGridPosition = gridPosition;
        currentDirection = Vector2Int.zero;
        queuedDirection = Vector2Int.zero;
    }
}
