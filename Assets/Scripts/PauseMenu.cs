using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your pause menu panel here")]
    public GameObject pauseMenu;
    
    [Tooltip("Drag your pause button here")]
    public Button pauseButton;

    [Header("Settings")]
    public bool enableESCKey = true; // For editor testing

    private bool isPaused = false;

    void Start()
    {
        // Initialize UI state
        if (pauseMenu != null) 
            pauseMenu.SetActive(false);
        else
            Debug.LogError("PauseMenu: No pause panel assigned!");

        // Setup button events
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
        else
        {
            Debug.LogError("PauseMenu: No pause button assigned!");
        }

        // Initial cursor state
       
    }

    void Update()
    {
        // Editor-only ESC key support
        #if UNITY_EDITOR
        if (enableESCKey && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        #endif
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        // Update game state
        Time.timeScale = isPaused ? 0f : 1f;
        
        // Update UI
        if (pauseMenu != null)
            pauseMenu.SetActive(isPaused);
        
        // Update cursor
       
        
        Debug.Log("Game " + (isPaused ? "Paused" : "Resumed"));
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Resume()
    {
        // This is now redundant with TogglePause but kept for compatibility
        TogglePause();
    }
}