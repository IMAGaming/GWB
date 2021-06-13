using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作为发送给单例DontDestroy对象的信号的中转对象，防止引用丢失
/// </summary>
public class SingletonSignalReceiver : MonoBehaviour
{
    public void TransitScene(int scene)
    {
        SceneTransit.Instance.RealSwitchSceneCoroutine(scene);
    }

    public void ResumeTimeline()
    {
        TimelineMgr.Instance.ResumeTimeline();
    }
}
