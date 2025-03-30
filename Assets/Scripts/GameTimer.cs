using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timeLimit = 120f; // 2 minutes
    [SerializeField] private Text timerText;

    private float currentTime;
    private bool gameOver = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerUI();
    }

    void Update()
    {
        if (gameOver) return;

        currentTime -= Time.deltaTime;
        if (currentTime < 0) currentTime = 0; // Prevent negative time

        if (Mathf.FloorToInt(currentTime) != Mathf.FloorToInt(currentTime + Time.deltaTime))
        {
            UpdateTimerUI();
        }

        if (currentTime <= 0)
        {
            GameOver();
        }
    }


    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("Time Up! Game Over.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart game
    }
}
