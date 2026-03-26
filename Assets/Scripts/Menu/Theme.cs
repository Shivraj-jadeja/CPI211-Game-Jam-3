using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeMenu : MonoBehaviour
{
    public string gameSceneName = "Backup Lighting + Doors + Scream";
    public string mainMenuSceneName = "UI_MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}