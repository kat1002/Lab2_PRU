using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    private void Awake()
    {
        GameEvents.OnUpdateScore.RemoveListener(UpdateScore);
        GameEvents.OnUpdateScore.AddListener(UpdateScore);
        GameEvents.OnGameOver.RemoveListener(OnGameOver);
        GameEvents.OnGameOver.AddListener(OnGameOver);
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    private void OnGameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        gameOverScoreText.text = scoreText.text;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
        Time.timeScale = 1f; // Resume time in case it was paused
    }
}
