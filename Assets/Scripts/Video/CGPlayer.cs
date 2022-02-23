using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CGPlayer : MonoBehaviour
{
    public RawImage rawImage;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    private bool isEnd = false;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        //audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Debug.LogFormat("{0}/{1}", videoPlayer.frame, videoPlayer.frameCount);
        if(videoPlayer.texture != null && !isEnd)
        {
            if((long)videoPlayer.frameCount - videoPlayer.frame <= 5)
            {
                SwitchScene();
            }
        }
    }

    public void PlayCG()
    {
        if (videoPlayer.texture == null)
        {
            return;
        }

        rawImage.gameObject.SetActive(true);
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

    public void SwitchScene()
    {
        isEnd = true;
        SceneTransit.Instance.RealSwitchSceneCoroutine((int)TargetScene.SELECT);
    }


}
