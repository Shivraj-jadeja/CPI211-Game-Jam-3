using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuStart : MonoBehaviour
{
    private bool keyPressed = false;

    void Update()
    {
        if (Input.anyKeyDown && !keyPressed)
        {
            keyPressed = true;
            SceneManager.LoadScene(0);
        }
    }
}