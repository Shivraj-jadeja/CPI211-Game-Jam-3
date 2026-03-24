using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [Header("Scenes")]
    public string roomSceneName = "MainScene";
    public string themeSceneName = "UI_Themes";
    public string controlSceneName = "UI_Controls"; 

    void Update()
    {
        // SPACE → go to room
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(roomSceneName);
        }

        // H → go to theme scene
        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene(themeSceneName);
        }

        //Press B got to control
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(controlSceneName);
        }
    }
}