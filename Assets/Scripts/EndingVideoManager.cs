using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingVideoManager : MonoBehaviour
{
    public static EndingVideoManager Instance;

    public VideoPlayer videoPlayer;
    public string mainMenuSceneName = "MainMenu";

    private bool isPlaying = false;

    private void Awake()
    {
        Instance = this;

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.skipOnDrop = false;
            videoPlayer.waitForFirstFrame = true;
        }
    }

    public void PlayEndingVideo()
    {
        if (isPlaying || videoPlayer == null) return;

        isPlaying = true;

<<<<<<< HEAD
        // Stop player movement
=======
>>>>>>> 52bcfaec46bc00878e142228e0c303dc05d14f90
        if (GameManager.Instance != null)
        {
            GameManager.Instance.canPlayerMove = false;
        }

        StartCoroutine(PlayAndReturnToMenu());
    }

    private IEnumerator PlayAndReturnToMenu()
    {
        videoPlayer.gameObject.SetActive(true);

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}