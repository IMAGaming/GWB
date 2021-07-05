using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineArea : MonoBehaviour
{
    [SerializeField] private PlayableDirector director = default;
    // 是否只允许播放一次
    [SerializeField] private bool PlayOnce = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartTimeline();
        }
    }

    public void StartTimeline()
    {
        TimelineMgr.Instance.currentPlayableDirector = director;
        director?.Play();
        if(PlayOnce)
        {
            gameObject.SetActive(false);
        }
    }
}
