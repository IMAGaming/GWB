using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineMgr : MonoSingleton<TimelineMgr>
{
    public enum TimeMode
    {
        PLAY,       // 正常播放
        DIALOGUE    // 要求对话响应
    }

    [HideInInspector] public TimeMode mode = TimeMode.PLAY;
    [HideInInspector] public PlayableDirector currentPlayableDirector = default;

    public void PauseTimeline()
    {
        mode = TimeMode.DIALOGUE;
        currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
    }

    public void ResumeTimeline()
    {
        mode = TimeMode.PLAY;
        currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
    }

}
