using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndingVideoManager : MonoBehaviour
{
    public static EndingVideoManager Instance;

    public VideoPlayer videoPlayer;
    public string mainMenuSceneName = "UI_MainMenu";

    private bool isPlaying = false;

    private void Awake()
    {
        Instance = this;

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    public void PlayEndingVideo()
    {
        if (isPlaying || videoPlayer == null) return;

        isPlaying = true;

        // Stop player movement
        if (GameManager.Instance != null)
        {
            GameManager.Instance.canPlayerMove = false;
        }

        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}