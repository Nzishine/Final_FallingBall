using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton pattern for easy access
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameHUD;

    [Header("Settings")]
    [SerializeField] private float gameOverDelay = 3f;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("GameManager initialized");
        ShowStartMenu();
    }

    public void GameOver()
    {
        Debug.Log("GameOver called");
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        Debug.Log("Starting GameOver routine");
        
        // 1. Show Game Over Panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("GameOver panel activated");
        }
        else
        {
            Debug.LogError("GameOver panel reference missing!");
        }

        // 2. Hide HUD
        if (gameHUD != null)
        {
            gameHUD.SetActive(false);
        }

        // 3. Wait for delay
        yield return new WaitForSeconds(gameOverDelay);

        // 4. Return to menu
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        Debug.Log("Showing start menu");
        
        // Hide all other panels
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (gameHUD != null) gameHUD.SetActive(false);
        
        // Show start panel
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Start panel reference missing!");
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting game");
        
        // Hide menu
        if (startPanel != null) startPanel.SetActive(false);
        
        // Show game UI
        if (gameHUD != null) gameHUD.SetActive(true);
        
        // Reset game state
        // Add your game initialization code here
    }
}