using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    public int pointsValue = 50;
    public float frightenedDuration = 8f;
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;
            GameManager.Instance.AddScore(pointsValue);
            GameManager.Instance.EatPellet();
            ActivateFrightenedMode();
            gameObject.SetActive(false);
        }
    }

    private void ActivateFrightenedMode()
    {
        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        foreach (var ghost in ghosts)
        {
            ghost.SetFrightened(true);
        }

        // Return to normal after duration
        Invoke("DeactivateFrightenedMode", frightenedDuration);
    }

    private void DeactivateFrightenedMode()
    {
        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        foreach (var ghost in ghosts)
        {
            ghost.SetFrightened(false);
        }
    }
}
