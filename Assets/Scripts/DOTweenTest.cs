using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenTest : MonoBehaviour
{
    private float tweenCount = 0f;
    private int tweenFrameCount = 0;
    void Start()
    {
        Debug.Log("start" + Time.time);
        transform.position = Vector3.zero;
        DOTween.To(() => tweenCount, x => tweenCount = x, 3f, 3f)
            //.OnUpdate(() => Debug.LogFormat("【{0}】DOTween.To:{1}",tweenFrameCount++,Time.time))
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => Debug.Log("OnComplete:" + Time.time));
    }

    private int fixedFrameCount = 0;
    private void FixedUpdate()
    {
        Debug.LogFormat("【{0}】FixedUpdate:{1}",fixedFrameCount++,Time.time);
    }

}
