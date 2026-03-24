using UnityEngine;

public class Final : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Game Quit");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}