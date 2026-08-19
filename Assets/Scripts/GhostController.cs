using UnityEngine;

public enum GhostType
{
    Blinky,  // Red - Direct chaser
    Pinky,   // Pink - Ambusher
    Inky,    // Cyan - Unpredictable
    Clyde    // Orange - Chaser/Runner
}

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public GhostType ghostType;
    public float moveSpeed = 1.5f;
    public int tileSize = 32;
    public Color normalColor;
    public Color frightenedColor;

    [Header("AI Settings")]
    public float chaseDistance = 10f;
    public float scatterDistance = 5f;
    public float modeChangeInterval = 7f;

    private Vector2Int gridPosition;
    private Vector2Int nextGridPosition;
    private Vector2Int targetPosition;
    private Vector3 startPosition;
    private MazeGenerator maze;
    private PacmanController pacman;
    private bool isFrightened = false;
    private float modeTimer = 0f;
    private bool isChasing = true;
    private Vector2Int[] scatterTargets;

    private void Start()
    {
        maze = FindObjectOfType<MazeGenerator>();
        pacman = FindObjectOfType<PacmanController>();
        startPosition = transform.position;
        gridPosition = maze.WorldToGridPosition(transform.position);
        nextGridPosition = gridPosition;

        // Set scatter targets for each ghost (corners)
        scatterTargets = new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(maze.GetWidth() - 2, 1),
            new Vector2Int(1, maze.GetHeight() - 2),
            new Vector2Int(maze.GetWidth() - 2, maze.GetHeight() - 2)
        };

        GetComponent<SpriteRenderer>().color = normalColor;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWon())
            return;

        MoveTowardNextTile();
        UpdateAI();
        CheckCollisionWithPacman();
    }

    private void UpdateAI()
    {
        // Update mode timer
        modeTimer += Time.deltaTime;
        if (modeTimer > modeChangeInterval)
        {
            modeTimer = 0f;
            isChasing = !isChasing;
        }

        // Check if we're at a grid position to make decision
        if (Vector3.Distance(transform.position, maze.GridToWorldPosition(gridPosition.x, gridPosition.y)) < 0.1f)
        {
            if (isFrightened)
            {
                targetPosition = GetRandomAdjacentTile();
            }
            else if (isChasing)
            {
                targetPosition = GetChaseTarget();
            }
            else
            {
                targetPosition = scatterTargets[(int)ghostType];
            }
        }
    }

    private Vector2Int GetChaseTarget()
    {
        Vector2Int pacmanPos = pacman.GetGridPosition();

        switch (ghostType)
        {
            case GhostType.Blinky:
                // Direct chase
                return pacmanPos;

            case GhostType.Pinky:
                // Target 4 tiles ahead of Pac-Man
                return pacmanPos + GetPacmanDirection() * 4;

            case GhostType.Inky:
                // Complex targeting using Blinky
                Vector2Int blinkyOffset = GetPacmanDirection() * 2;
                return pacmanPos + blinkyOffset;

            case GhostType.Clyde:
                // Chase if far, scatter if close
                float distance = Vector2Int.Distance(gridPosition, pacmanPos);
                if (distance > chaseDistance)
                    return pacmanPos;
                else
                    return scatterTargets[3];

            default:
                return pacmanPos;
        }
    }

    private Vector2Int GetPacmanDirection()
    {
        // Simplified - returns the general direction
        Vector2Int pacmanPos = pacman.GetGridPosition();
        Vector2Int diff = pacmanPos - gridPosition;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            return diff.x > 0 ? Vector2Int.right : Vector2Int.left;
        else
            return diff.y > 0 ? Vector2Int.up : Vector2Int.down;
    }

    private Vector2Int GetRandomAdjacentTile()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int newPos = gridPosition + directions[Random.Range(0, 4)];

        if (!maze.IsWall(newPos.x, newPos.y))
            return newPos;

        return gridPosition;
    }

    private void MoveTowardNextTile()
    {
        // Check if at grid position
        if (Vector3.Distance(transform.position, maze.GridToWorldPosition(gridPosition.x, gridPosition.y)) < 0.1f)
        {
            transform.position = maze.GridToWorldPosition(gridPosition.x, gridPosition.y);

            // Choose next move toward target
            Vector2Int bestMove = gridPosition;
            float bestDistance = float.MaxValue;

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (Vector2Int dir in directions)
            {
                Vector2Int newPos = gridPosition + dir;

                // Handle tunnel wrapping
                if (newPos.x < 0)
                    newPos.x = maze.GetWidth() - 1;
                else if (newPos.x >= maze.GetWidth())
                    newPos.x = 0;

                if (!maze.IsWall(newPos.x, newPos.y))
                {
                    float distance = Vector2Int.Distance(newPos, targetPosition);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestMove = newPos;
                    }
                }
            }

            if (bestMove != gridPosition)
                gridPosition = bestMove;
        }

        // Smoothly move toward grid position
        Vector3 targetPos = maze.GridToWorldPosition(gridPosition.x, gridPosition.y);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void CheckCollisionWithPacman()
    {
        if (Vector3.Distance(transform.position, pacman.transform.position) < tileSize * 0.4f)
        {
            if (isFrightened)
            {
                GameManager.Instance.EatGhost(this);
            }
            else
            {
                GameManager.Instance.PacmanDied();
            }
        }
    }

    public void SetFrightened(bool frightened)
    {
        isFrightened = frightened;
        GetComponent<SpriteRenderer>().color = frightened ? frightenedColor : normalColor;
    }

    public void Die()
    {
        transform.position = startPosition;
        gridPosition = maze.WorldToGridPosition(startPosition);
        isFrightened = false;
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        gridPosition = maze.WorldToGridPosition(startPosition);
        isFrightened = false;
        GetComponent<SpriteRenderer>().color = normalColor;
    }
}
