using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public int width = 28;
    public int height = 31;
    public int tileSize = 32;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject pelletPrefab;
    public GameObject powerPelletPrefab;

    private int[,] mazeLayout;
    private GameObject wallsContainer;
    private GameObject pelletsContainer;

    // Maze types: 0 = path, 1 = wall, 2 = pellet spawn, 3 = power pellet spawn
    private void Start()
    {
        GenerateMaze();
        SpawnWalls();
        SpawnPellets();
    }

    private void GenerateMaze()
    {
        mazeLayout = new int[width, height];

        // Create outer walls
        for (int x = 0; x < width; x++)
        {
            mazeLayout[x, 0] = 1;
            mazeLayout[x, height - 1] = 1;
        }
        for (int y = 0; y < height; y++)
        {
            mazeLayout[0, y] = 1;
            mazeLayout[width - 1, y] = 1;
        }

        // Add internal walls to create classic Pac-Man layout
        CreateInternalWalls();

        // Mark pellet spawn locations (all non-wall spaces except specific areas)
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (mazeLayout[x, y] == 0)
                {
                    mazeLayout[x, y] = 2; // Pellet spawn
                }
            }
        }

        // Add power pellets in corners
        mazeLayout[1, 1] = 3;
        mazeLayout[width - 2, 1] = 3;
        mazeLayout[1, height - 2] = 3;
        mazeLayout[width - 2, height - 2] = 3;
    }

    private void CreateInternalWalls()
    {
        // This creates a simplified classic Pac-Man style maze
        // You can customize this pattern as needed

        // Central area walls
        for (int x = 9; x < 19; x++)
        {
            mazeLayout[x, 9] = 1;
            mazeLayout[x, 19] = 1;
        }

        for (int y = 9; y < 19; y++)
        {
            mazeLayout[9, y] = 1;
            mazeLayout[18, y] = 1;
        }

        // Ghost house walls
        for (int x = 11; x < 17; x++)
        {
            mazeLayout[x, 14] = 1;
            mazeLayout[x, 16] = 1;
        }

        for (int y = 14; y < 17; y++)
        {
            mazeLayout[11, y] = 1;
            mazeLayout[16, y] = 1;
        }

        // Additional maze structure
        for (int x = 4; x < 24; x += 5)
        {
            for (int y = 4; y < 27; y += 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (x + i < width - 1 && y + i < height - 1)
                    {
                        mazeLayout[x + i, y] = 1;
                        mazeLayout[x, y + i] = 1;
                    }
                }
            }
        }
    }

    private void SpawnWalls()
    {
        wallsContainer = new GameObject("Walls");
        wallsContainer.transform.parent = transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mazeLayout[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0);
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity, wallsContainer.transform);
                    wall.name = $"Wall_{x}_{y}";
                }
            }
        }
    }

    private void SpawnPellets()
    {
        pelletsContainer = new GameObject("Pellets");
        pelletsContainer.transform.parent = transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mazeLayout[x, y] == 2)
                {
                    Vector3 pos = new Vector3(x * tileSize + tileSize / 2f, y * tileSize + tileSize / 2f, 0);
                    GameObject pellet = Instantiate(pelletPrefab, pos, Quaternion.identity, pelletsContainer.transform);
                    pellet.name = $"Pellet_{x}_{y}";
                }
                else if (mazeLayout[x, y] == 3)
                {
                    Vector3 pos = new Vector3(x * tileSize + tileSize / 2f, y * tileSize + tileSize / 2f, 0);
                    GameObject powerPellet = Instantiate(powerPelletPrefab, pos, Quaternion.identity, pelletsContainer.transform);
                    powerPellet.name = $"PowerPellet_{x}_{y}";
                }
            }
        }

        // Update pellet count in GameManager
        GameManager.Instance.pelletsRemaining = pelletsContainer.transform.childCount;
    }

    public bool IsWall(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return true;
        return mazeLayout[x, y] == 1;
    }

    public Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(x * tileSize + tileSize / 2f, y * tileSize + tileSize / 2f, 0);
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / tileSize);
        int y = Mathf.RoundToInt(worldPos.y / tileSize);
        return new Vector2Int(x, y);
    }

    public int GetWidth() => width;
    public int GetHeight() => height;
}
