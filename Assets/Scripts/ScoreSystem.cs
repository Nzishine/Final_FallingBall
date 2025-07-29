using UnityEngine;
using System.Collections; // For Coroutines
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance; 
    
    // Original Score System UI and Settings
    [Header("Score UI References")]
    public TMP_Text scoreText; 
    
    [Header("Score Settings")]
    [SerializeField] private int currentScore = 0; 

    // Coin System UI and Settings (NEWLY ADDED)
    [Header("Coin UI References")]
    public TMP_Text coinText; 
    [SerializeField] private int currentCoins = 0; 

    [Header("Coin Spawner Settings")]
    public GameObject coinPrefab; 
    public float minX = -8f; 
    public float maxX = 8f; 
    public float minY = 0f; 
    public float maxY = 4f; 
    public float minSpawnInterval = 1f; 
    public float maxSpawnInterval = 3f; 
    public int minCoinsPerSpawn = 1; 
    public int maxCoinsPerSpawn = 3; 
    public float coinLifetime = 5f; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
    }

    private void FindScoreText()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
            if (scoreText == null)
            {
                Debug.LogError("ScoreText not found in scene! Please assign it in the Inspector or ensure a GameObject named 'ScoreText' with a TMP_Text component exists.");
            }
        }
    }

    private void FindCoinText()
    {
        if (coinText == null)
        {
            coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
            if (coinText == null)
            {
                Debug.LogError("CoinText not found in scene! Please assign it in the Inspector or ensure a GameObject named 'CoinText' with a TMP_Text component exists.");
            }
        }
    }

    void Start()
    {
        FindScoreText(); 
        UpdateScoreDisplay();

        FindCoinText(); // Initialize Coin Text
        UpdateCoinDisplay(); // Display initial coins
        
        // Start Coin Spawner
        StartCoroutine(SpawnCoinsRoutine());
    }

    // --- Original Score System Methods ---
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }

    public int GetCurrentScore() => currentScore;

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
            Debug.LogWarning("Score Text reference is missing! Cannot update score display.");
        }
    }

    public void DisplayFinalScore(TMP_Text targetText)
    {
        if (targetText != null)
        {
            targetText.text = $"Final Score: {currentScore}";
        }
    }

    // --- Coin System Methods (NEWLY ADDED) ---
    public void AddCoin(int amount)
    {
        currentCoins += amount;
        UpdateCoinDisplay();
    }

    public int GetCurrentCoins() => currentCoins;

    public void ResetCoins()
    {
        currentCoins = 0;
        UpdateCoinDisplay();
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {currentCoins}";
        }
        else
        {
            Debug.LogWarning("Coin Text reference is missing! Cannot update coin display.");
        }
    }

    public void DisplayFinalCoins(TMP_Text targetText)
    {
        if (targetText != null)
        {
            targetText.text = $"Coins: {currentCoins}";
        }
    }

    // --- Coin Spawner Methods (NEWLY ADDED) ---
    IEnumerator SpawnCoinsRoutine()
    {
        while (true)
        {
            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnDelay);

            int numberOfCoinsToSpawn = Random.Range(minCoinsPerSpawn, maxCoinsPerSpawn + 1);

            for (int i = 0; i < numberOfCoinsToSpawn; i++)
            {
                SpawnSingleCoin();
            }
        }
    }

    void SpawnSingleCoin()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin Prefab is not assigned in ScoreSystem!");
            return;
        }

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        GameObject spawnedCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        
        CollectableComponent collectable = spawnedCoin.AddComponent<CollectableComponent>();
        collectable.Initialize(this, coinLifetime); // Pass ScoreSystem instance and lifetime
    }
}

// Collectable Component (NEWLY ADDED - MUST BE IN THE SAME FILE AS ScoreSystem)
public class CollectableComponent : MonoBehaviour
{
    private ScoreSystem _scoreSystemInstance; 

    public void Initialize(ScoreSystem manager, float lifetime)
    {
        _scoreSystemInstance = manager;
        Destroy(gameObject, lifetime); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) 
        {
            if (_scoreSystemInstance != null) 
            {
                _scoreSystemInstance.AddCoin(1); // Call AddCoin on the ScoreSystem instance
            }
            else
            {
                Debug.LogWarning("ScoreSystem instance is missing in CollectableComponent!");
            }
            Destroy(gameObject); 
        }
    }
}