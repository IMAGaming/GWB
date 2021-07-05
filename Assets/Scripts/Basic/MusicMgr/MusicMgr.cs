using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMgr : MonoSingleton<MusicMgr>
{
    [Header("背景音乐")]
    public AudioClip openGameMusic;
    public AudioClip[] level_Music;
    

    [Header("旋转物品音效")]
    public AudioClip[] discMusic;

    [Header("编钟敲击音效")]
    public AudioClip[] clockMusic;

    [Header("场景互动音效")]
    public AudioClip windMusic;
    public AudioClip pushMusic;
    public AudioClip pickUpMusic;
    public AudioClip clickMusic;

    public AudioSource bgmSource;
    public AudioSource interactiveSouce;

    private void Start()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        interactiveSouce = gameObject.AddComponent<AudioSource>();
        bgmSource.Play();
        bgmSource.loop = true;
        bgmSource.volume = 0.2f;
    }

    //播放BGM
    public void PlayBGM()
    {
        switch (SceneTransit.Instance.currentScene)
        {
            case TargetScene.LEVEL0:
                bgmSource.clip = level_Music[0];
                bgmSource.Play();
                bgmSource.loop = true;
                break;
            case TargetScene.LEVEL1:
                bgmSource.clip = level_Music[1];
                bgmSource.Play();
                bgmSource.loop = true;
                break;
            case TargetScene.LEVEL2:
                bgmSource.clip = level_Music[2];
                bgmSource.Play();
                bgmSource.loop = true;
                break;
            case TargetScene.LEVEL3:
                bgmSource.clip = level_Music[4];
                bgmSource.Play();
                bgmSource.loop = true;
                break;
            case TargetScene.SELECT:
                StopBGM();
                break;

            default:
                break;
        }

        

    }

    //停止播放BGM
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //播放音效
    public void PlaySound(AudioClip soundClip, bool isLoop)
    {
        //interactiveSouce.clip = soundClip;
        interactiveSouce.PlayOneShot(soundClip);
        interactiveSouce.loop = isLoop;
    }

    //停止播放音效
    public void StopSound()
    {
        interactiveSouce.Stop();
    }
}
