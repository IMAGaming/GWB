using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LadderUpAnimation : MonoBehaviour
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
    /// 左直角三角阶梯
    /// </summary>
    private float[] LTA_Pillars = new float[8] {
        -1.3f, -1.6f, -1.9f, -2.2f, -2.5f, -2.8f, -3.1f, -3.4f,
    };

    /// <summary>
    /// 右直角三角阶梯
    /// </summary>
    private float[] RTA_Pillars = new float[8] {
        -1.9f, -1.6f, -1.3f, -1f, -0.7f, -0.4f, -0.1f, 0.2f
    };

    /// <summary>
    /// M阶梯
    /// </summary>
    private float[] M_Pillars = new float[8] {
        -3.4f, -3.7f, -4f, -4.3f, -4.3f, -4f, -3.7f, -3.4f
    };

    /// <summary>
    /// 一字阶梯
    /// </summary>
    private float[] One_Pillars = new float[8] {
        -3.4f, -3.4f, -3.4f, -3.4f, -3.4f, -3.4f, -3.4f, -3.4f,
    };

    public void LadderAnimation(float degree)
    {
        switch (degree)
        {
            case 0f:
                RealLadderAnimation(LTA_Pillars);
                break;
            case 90f:
                RealLadderAnimation(One_Pillars);
                break;
            case -90f:
                RealLadderAnimation(RTA_Pillars);
                break;
            case 180f:
            case -180f:
                RealLadderAnimation(M_Pillars);
                break;
        }
    }

    private void RealLadderAnimation(float[] arr)
    {
        for(int i = 0; i < ladderPillars.Length; ++i)
        {
            ladderPillars[i].DOMoveY(arr[i], animationDuration)
                .OnStart(() => PlayerController.Instance.isAllowMove = false)
                .OnComplete(() => {
                    EventCenter.GetInstance().EventTrigger(GameEvent.WayPathUpdate);
                    PlayerController.Instance.isAllowMove = true;
                    });
        }
    }
}
