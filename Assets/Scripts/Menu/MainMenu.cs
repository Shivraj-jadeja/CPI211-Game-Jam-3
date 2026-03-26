using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Backup Lighting + Doors + Scream";
    public string themeSceneName = "UI_Theme";
    public string controlsSceneName = "UI_Control";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            OpenTheme();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenControls();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenTheme()
    {
        SceneManager.LoadScene(themeSceneName);
    }

    public void OpenControls()
    {
        SceneManager.LoadScene(controlsSceneName);
    }

    public void QuitGame()
    {
    Debug.Log("Quit Game");

    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
    }
}