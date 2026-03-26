using UnityEngine;

public class Final : MonoBehaviour
{
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