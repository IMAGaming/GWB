using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为Timeline发送给单例DontDestroy对象的信号的中转对象，防止引用丢失
/// </summary>
public class SingletonSignalReceiver : MonoBehaviour
{
    /// <summary>
    /// Timeline Signal调用
    /// </summary>
    /// <param name="scene"></param>
    public void TransitScene(int scene)
    {
        SceneTransit.Instance.RealSwitchSceneCoroutine(scene);
    }

    /// <summary>
    /// DialogueUI Button调用
    /// </summary>
    public void ResumeTimeline()
    {
        TimelineMgr.Instance.ResumeTimeline();
    }
}
