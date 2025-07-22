using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButtonController : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        Debug.Log("Quit Game");
    }
}