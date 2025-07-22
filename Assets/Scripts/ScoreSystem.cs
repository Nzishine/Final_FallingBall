using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;
    
    [Header("UI References")]
    public TMP_Text scoreText;            // Drag your TextMeshPro UI element here
    public TMP_Text highScoreText;        // Optional: For displaying high score
    
    [Header("Settings")]
    [SerializeField] private int currentScore = 0;
    private int highScore = 0;
    private const string HIGH_SCORE_KEY = "HighScore";

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes if needed
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadHighScore();
    }

    private void FindScoreText()
{
    if (scoreText == null)
    {
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        if (scoreText == null)
        {
            Debug.LogError("ScoreText not found in scene!");
        }
    }
}

void Start()
{
    FindScoreText(); // Add this line
    UpdateScoreDisplay();
}

    public void AddScore(int points)
    {
        currentScore += points;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            UpdateHighScoreDisplay();
        }
        UpdateScoreDisplay();
    }

    public int GetCurrentScore() => currentScore;
    public int GetHighScore() => highScore;

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
        else
        {
            Debug.LogWarning("Score Text reference is missing!");
        }
    }

    private void UpdateHighScoreDisplay()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"Best: {highScore}";
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();
    }

    // Call this when showing game over screen
    public void DisplayFinalScore(TMP_Text targetText)
    {
        if (targetText != null)
        {
            targetText.text = $"Final Score: {currentScore}\nHigh Score: {highScore}";
        }
    }
}