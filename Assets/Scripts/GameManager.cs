using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canPlayerMove = false;
    public bool introFinished = false;
    public bool firstMoveDialoguePlayed = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}