using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    // Singleton for easy access
    public static GameOverController Instance;

    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 3f;

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
            return;
        }

        // Initialize panel state
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }
    }

    public void ShowGameOver()
    {
        Debug.Log("[GameOver] ShowGameOver called");
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        Debug.Log("[GameOver] Starting sequence");
        
        // Activate panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("[GameOver] Panel activated");
        }

        // Fade in
        if (gameOverCanvasGroup != null)
        {
            Debug.Log("[GameOver] Starting fade in");
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                gameOverCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed/fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            gameOverCanvasGroup.alpha = 1;
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
        }

        // Wait for display duration
        Debug.Log("[GameOver] Waiting for display duration");
        yield return new WaitForSeconds(displayDuration);

        // Return to menu (optional)
        // MenuController.Instance.ReturnToMainMenu();
    }
}