using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    [Header("Scenes")]
    public string gameSceneName = "Main_Level"; 
    public string themeSceneName = "UI_Themes";  
    

    void Update()
    {
        // SPACE → start game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(gameSceneName);
        }

        // H → go to theme scene
        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene(themeSceneName);
        }
    }
}