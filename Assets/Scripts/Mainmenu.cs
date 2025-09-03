using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game HUD Elements")]
    [SerializeField] private GameObject heartsPanel;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject coinCountText;

    [Header("Settings")]
    [SerializeField] private float gameOverDelay = 3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("MainMenu initialized");
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

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOver panel reference missing!");
        }

        SetGameHudActive(false);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(gameOverDelay);

        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        Debug.Log("Showing start menu");

        Time.timeScale = 1f;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        SetGameHudActive(false);

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

        if (startPanel != null) startPanel.SetActive(false);
        SetGameHudActive(true);


        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.ResetScore();
            //ScoreSystem.Instance.ResetCoins();
        }
        if (LivesSystem.Instance != null)
        {
            LivesSystem.Instance.ResetLives();
        }
    }

    private void SetGameHudActive(bool isActive)
    {
        if (heartsPanel != null) heartsPanel.SetActive(isActive);
        if (scoreText != null) scoreText.SetActive(isActive);
        if (coinCountText != null) coinCountText.SetActive(isActive);
    }

    public void LoadShopScene()
    {
        Debug.Log("shop ok");
        SceneManager.LoadScene("ShopScene");

    }

    public void LoadMainMenu()
    {
        Debug.Log("Loaded");
        SceneManager.LoadScene("SampleScene");

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}