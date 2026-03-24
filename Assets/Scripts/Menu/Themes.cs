using UnityEngine;
using UnityEngine.SceneManagement;

public class Themes : MonoBehaviour
{
    [Header("Scenes")]
    public string gameSceneName = "Main_Level";   
    public string controlSceneName = "UI_Controls"; 

    void Update()
    {
        // SPACE → start game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(gameSceneName);
        }

        // B → go to control / room scene
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(controlSceneName);
        }
    }
}