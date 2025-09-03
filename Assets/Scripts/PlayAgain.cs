using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayAgainButton : MonoBehaviour
{
    [Header("Ball Settings")]
    [Tooltip("Drag your Ball object here")]
    [SerializeField] private GameStarter ballController;
    
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameHUD;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ResetGame);
    }

    public void ResetGame()
    {
        // Reset UI
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
        if(gameHUD != null) gameHUD.SetActive(true);

        // Reset game systems
        LivesSystem.Instance?.ResetLives();
        ScoreSystem.Instance?.ResetScore();
        ScoreSystem.Instance?.ResetRoundCoins();

        // Reset ball using manual position
        if(ballController != null)
        {
            ballController.ResetToStartPosition();
            ballController.StartGame(); // Restart the game flow
        }

        Time.timeScale = 1f; // Unpause
    }
}