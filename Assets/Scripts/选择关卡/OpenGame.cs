using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour
{
    //private bool isPlay;
    private GameObject videoPlayer;

    private void Start()
    {
        videoPlayer = GameObject.Find("VideoPlayer");
        //if (SceneTransit.Instance.currentScene == TargetScene.OPEN)
        //{
        //    isPlay = true;
        //}
        //else
        //{
        //    isPlay = false;
        //}
    }

    public void GameStart()
    {
        //if(isPlay)
        //{
        //    videoPlayer.GetComponent<CGPlayer>().PlayCG();
        //}
        //else
        //{
        //    SceneTransit.Instance.RealSwitchSceneCoroutine((int)TargetScene.SELECT);
        //}
        videoPlayer.GetComponent<CGPlayer>().PlayCG();
    }

    public void GameExit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
