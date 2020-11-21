using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public RawImage videoImage;
    public RawImage backgroundColor;
    public Texture videoTexture;
    public GameObject videoGameObject;
    public VideoClip videoClip;
    public AudioSource audioSource;

    private bool isGameStarted;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (isGameStarted)
        {
            RunVideoClip();
        }
    }

    public void StartButton()
    {
        isGameStarted = true;
        Invoke("StartGame", (float)videoClip.length + 1f);
    }

    public void RunVideoClip()
    {
        if(!videoImage.gameObject.activeInHierarchy)
        {
            videoImage.gameObject.SetActive(true);
            backgroundColor.gameObject.SetActive(true);
        }

        videoImage.color = Color.Lerp(videoImage.color, new Color(videoImage.color.r, videoImage.color.g, videoImage.color.b, 1), Time.deltaTime * 4f);
        backgroundColor.color = Color.Lerp(backgroundColor.color, new Color(backgroundColor.color.r, backgroundColor.color.g, backgroundColor.color.b, 1), Time.deltaTime * 4f);
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * 4f);
        Debug.Log(videoImage.color.a);
        if (videoImage.color.a >= 0.98f)
        {
            videoGameObject.SetActive(true);
            videoImage.texture = videoTexture;
            videoImage.color = new Color(1, 1, 1, 1);
            isGameStarted = false;
        }
    }
}
