using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For reloading the scene

public class LivesSystem : MonoBehaviour
{
    public static LivesSystem Instance;

    [Header("UI References")]
    public Image[] hearts; 
    public GameObject gameOverPanel; 
    public TMP_Text finalScoreText; 
    public TMP_Text finalCoinsText; // Add this for displaying final coins

    [Header("Settings")]
    public int maxLives = 3;
    private int currentLives;

    void Awake()
    {
        Instance = this;
        ResetLives();
    }

    public void LoseLife()
    {
        if(currentLives <= 0) return; 

        currentLives--;
        UpdateHeartsUI();

        if(currentLives <= 0)
        {
            GameOver();
        }
    }

    void UpdateHeartsUI()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);

        // Display final score
        if(finalScoreText != null && ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.DisplayFinalScore(finalScoreText);
        }
        // Display final coins
        if(finalCoinsText != null && ScoreSystem.Instance != null) 
        {
            ScoreSystem.Instance.DisplayFinalCoins(finalCoinsText); 
        }

        Time.timeScale = 0f;
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        UpdateHeartsUI();
    }

    // Call this from a UI Button when you want to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause the game

        // Reset Score and Coins
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.ResetScore();
            ScoreSystem.Instance.ResetCoins(); // Reset coins through ScoreSystem
        }

        gameOverPanel.SetActive(false);

        // Reload the current scene to effectively restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}