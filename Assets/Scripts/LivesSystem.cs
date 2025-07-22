using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LivesSystem : MonoBehaviour
{
    public static LivesSystem Instance;
    
    [Header("UI References")]
    public Image[] hearts;          // Assign heart images in Inspector
    public GameObject gameOverPanel; // Assign your Game Over panel
    public TMP_Text finalScoreText;  // Text to show final score
    
    [Header("Settings")]
    public int maxLives = 3;
    private int currentLives;

    void Awake()
    {
        Instance = this;
        currentLives = maxLives;
    }

    public void LoseLife()
    {
        if(currentLives <= 0) return; // Already game over
        
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
    // Activate panel
    gameOverPanel.SetActive(true);
    
    // Update final score text
    TextMeshProUGUI scoreText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
    if(scoreText != null && ScoreSystem.Instance != null)
    {
        scoreText.text = $"Final Score: {ScoreSystem.Instance.GetCurrentScore()}";
    }
    
    // Pause game
    Time.timeScale = 0f;
}

    // Call this to restart the game
   
}