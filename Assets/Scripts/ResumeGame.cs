using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResumeButtonController : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenuPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ResumeGame);
        
        if (pauseMenuPanel == null)
            Debug.LogError("ResumeButton: No pause panel assigned!");
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("Game Resumed");
        }
    }
}