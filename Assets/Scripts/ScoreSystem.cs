using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    [Header("Score UI References")]
    public TMP_Text scoreText;

    [Header("Score Settings")]
    [SerializeField] private int currentScore = 0;

    [Header("Coin UI References")]
    public TMP_Text coinText;
    public AudioClip coinSound;
    private int currentCoins = 0;

    [Header("Background Settings")]
    public SpriteRenderer backgroundRenderer;
    public Sprite defaultBackground;
    public Sprite backgroundAt20;
    public Sprite backgroundAt40;


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

    void Start()
    {
        FindScoreText();
        UpdateScoreDisplay();

        FindCoinText();

        currentCoins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinDisplay();

        if (backgroundRenderer != null && defaultBackground != null)
            backgroundRenderer.sprite = defaultBackground;


        StartCoroutine(SpawnCoinsRoutine());
    }

    private void FindScoreText()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
    }

    private void FindCoinText()
    {
        if (coinText == null)
            coinText = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
    }

    // --- Score Methods ---
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        CheckBackgroundChange();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
        CheckBackgroundChange();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }

    private void CheckBackgroundChange()
    {
        if (backgroundRenderer == null) return;

        if (currentScore >= 40 && backgroundAt40 != null)
        {
            backgroundRenderer.sprite = backgroundAt40;
        }
        else if (currentScore >= 20 && backgroundAt20 != null)
        {
            backgroundRenderer.sprite = backgroundAt20;
        }
        else if (defaultBackground != null)
        {
            backgroundRenderer.sprite = defaultBackground;
        }
    }


    // --- Coin Methods ---
    public void AddCoin(int amount)
    {
        currentCoins += amount;
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.Save();
        UpdateCoinDisplay();
    }

    public int GetCurrentCoins() => currentCoins;


    public void ResetCoins()
    {
        currentCoins = 0;
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.Save();
        UpdateCoinDisplay();
    }
    public void ResetRoundCoins()
    {
        currentCoins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinDisplay();
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
            coinText.text = $"Coins: {currentCoins}";
    }

    public void DisplayFinalScore(TMP_Text targetText)
    {
        if (targetText != null)
        {
            targetText.text = $"Final Score: {currentScore}";
        }
    }

    public void DisplayFinalCoins(TMP_Text targetText)
    {
        if (targetText != null)
        {
            targetText.text = $"Final Coins: {currentCoins}";
        }
    }

    // --- Coin Spawner ---
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
        collectable.Initialize(this, coinLifetime);
    }
}

// --- Collectable Component ---
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
            _scoreSystemInstance?.AddCoin(1);

            if (_scoreSystemInstance != null && _scoreSystemInstance.coinSound != null)
            {
                AudioSource.PlayClipAtPoint(_scoreSystemInstance.coinSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
