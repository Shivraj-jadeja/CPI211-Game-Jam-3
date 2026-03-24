using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuStart : MonoBehaviour
{
    private bool keyPressed = false;

    void Update()
    {
        if (!keyPressed)
        {
            // loads main scene or game when space is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                keyPressed = true;
                SceneManager.LoadScene(0);
            }

            // loads themes scene
            if (Input.GetKeyDown(KeyCode.H))
            {
                keyPressed = true;
                SceneManager.LoadScene(3);
            }
        }
    }
}