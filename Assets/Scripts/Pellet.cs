using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int pointsValue = 10;
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;
            GameManager.Instance.AddScore(pointsValue);
            GameManager.Instance.EatPellet();
            gameObject.SetActive(false);
        }
    }
}
