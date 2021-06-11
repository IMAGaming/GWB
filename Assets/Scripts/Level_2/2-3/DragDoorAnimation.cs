using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 2-5中推拉门动画（已弃用）
/// </summary>
public class DragDoorAnimation : MonoBehaviour
{
    [SerializeField] private Transform leftDragDoor = default;
    [SerializeField] private Transform rightDragDoor = default;
    [SerializeField] private Transform leftPoint = default;
    [SerializeField] private Transform rightPoint = default;
    [SerializeField] private float duration = 1f;

    /// <summary>
    /// 外部调用的拖拉门动画方法
    /// </summary>
    public void DragDoorOpenAnimation()
    {
        leftDragDoor.DOMoveX(leftPoint.position.x, duration);
        rightDragDoor.DOMoveX(rightPoint.position.x, duration);
    }

    /// <summary>
    /// 接入TimeLine的Director.Play()
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeLineStart()
    {
        yield return null;
    }
}
