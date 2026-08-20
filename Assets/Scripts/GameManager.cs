using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int score = 0;
    public int lives = 3;
    public int pelletsRemaining;
    public int currentLevel = 1;
    
    [Header("UI References")]
    public Text scoreText;
    public Text livesText;
    public Text pelletsText;
    public Text gameOverText;
    public Text winText;

    [Header("Game References")]
    public PacmanController pacman;
    public GhostController[] ghosts;
    public MazeGenerator maze;

    private bool gameOver = false;
    private bool gameWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameOver || gameWon) return;

        // Check win condition
        if (pelletsRemaining <= 0)
        {
            WinLevel();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    public void EatPellet()
    {
        pelletsRemaining--;
        UpdateUI();
    }

    public void PacmanDied()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // Reset positions
            pacman.ResetPosition();
            foreach (var ghost in ghosts)
            {
                ghost.ResetPosition();
            }
        }
    }

    public void EatGhost(GhostController ghost)
    {
        AddScore(200);
        ghost.Die();
    }

    private void GameOver()
    {
        gameOver = true;
        gameOverText.gameObject.SetActive(true);
        pacman.enabled = false;
        foreach (var ghost in ghosts)
        {
            ghost.enabled = false;
        }
    }

    private void WinLevel()
    {
        gameWon = true;
        winText.gameObject.SetActive(true);
        pacman.enabled = false;
        foreach (var ghost in ghosts)
        {
            ghost.enabled = false;
        }
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        pelletsText.text = "Pellets: " + pelletsRemaining;
    }

    public bool IsGameOver() => gameOver;
    public bool IsGameWon() => gameWon;
}
