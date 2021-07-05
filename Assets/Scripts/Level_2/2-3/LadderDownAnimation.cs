using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LadderDownAnimation : MonoBehaviour
{
    /// <summary>
    /// 动画时间长度
    /// </summary>
    [SerializeField] private float animationDuration = 1f;

    /// <summary>
    /// 阶梯元素（柱）
    /// </summary>
    [SerializeField] private Transform[] ladderPillars = new Transform[8];

    /// <summary>
    /// 倒V阶梯
    /// </summary>
    private float[] RV_Pillars = new float[8] { 
        -5.4f, -5.1f, -4.8f, -4.5f, -4.5f, -4.8f, -5.1f, -5.4f
    };

    /// <summary>
    /// 三角阶梯
    /// </summary>
    private float[] TA_Pillars = new float[8] {
        -1.2f, -1.5f, -1.8f, -2.1f, -2.4f, -2.7f, -3f, -3.3f
    };

    /// <summary>
    /// M阶梯
    /// </summary>
    private float[] M_Pillars = new float[8] {
        -4.5f, -4.8f, -5.1f, -5.4f, -5.4f, -5.1f, -4.8f, -4.5f
    };

    /// <summary>
    /// 一字阶梯
    /// </summary>
    private float[] One_Pillars = new float[8] {
        -3.3f, -3.3f, -3.3f, -3.3f, -3.3f, -3.3f, -3.3f, -3.3f
    };

    public void LadderAnimation(float degree)
    {
        switch (degree)
        {
            case 0f:
                RealLadderAnimation(RV_Pillars);
                break;
            case 90f:
                RealLadderAnimation(TA_Pillars);
                break;
            case -90f:
                RealLadderAnimation(M_Pillars);
                break;
            case 180f:
            case -180f:
                RealLadderAnimation(One_Pillars);
                break;
        }
    }

    private void RealLadderAnimation(float[] arr)
    {
        for (int i = 0; i < ladderPillars.Length; ++i)
        {
            ladderPillars[i].DOMoveY(arr[i], animationDuration)
                .OnStart(() => PlayerController.Instance.isAllowMove = false)
                .OnComplete(() =>
                {
                    EventCenter.GetInstance().EventTrigger(GameEvent.WayPathUpdate);
                    PlayerController.Instance.isAllowMove = true;
                });
        }
    }
}
