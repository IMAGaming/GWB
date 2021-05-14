using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenTest : MonoBehaviour
{
    private float tweenCount = 0f;
    private int tweenFrameCount = 0;
    Tweener t;
    void Start()
    {
        Debug.Log("start" + Time.time);
        DOTween.To(() => tweenCount, x => tweenCount = x, 2f, 2f).SetUpdate(UpdateType.Fixed);
            //.OnUpdate(() => Debug.LogFormat("【{0}】DOTween.To:{1} 【Time.time:{2}】", tweenFrameCount++, tweenCount, Time.time))
            //.SetUpdate(UpdateType.Fixed);
    }

    private int fixedFrameCount = 0;
    //private void FixedUpdate()
    //{
    //    //Debug.LogFormat("【{0}】FixedUpdate:{1}",fixedFrameCount++,Time.time);
    //}

    private void FixedUpdate()
    {
        Debug.LogFormat("【{0}】Update:{1}【Time.time:{2}】", fixedFrameCount++, tweenCount, Time.time);
    }

}
