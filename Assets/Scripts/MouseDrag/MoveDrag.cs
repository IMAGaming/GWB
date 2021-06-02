using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 直线拖动操作类
/// </summary>
public class MoveDrag : DraggingAction
{
    // Start和End对应拖动进度progress的0和1
    [SerializeField] private Transform offsetStart = default;
    [SerializeField] private Transform offsetEnd = default;

    // 停止点，数量为0的话则在全部路径点选取最近的
    [SerializeField] private List<WayPoint> stopPoints = new List<WayPoint>();
    // 动画恢复时间
    [SerializeField] private float recoverTime = 1f;
    // 缓动类型
    [SerializeField] private Ease easeType = DOTween.defaultEaseType;

    private Camera cam;
    private float progressValue; // 当前progress值 拖动时即时更新
    private float offsetLength;
    private Vector3 dragStartPos; // 记录拖拽开始时的位置
    private Vector3 originPos; // 保存最开始未拖拽时的位置
    private Vector2 progressVec; // 保存
    private Vector2 curMousePos; // 当前帧鼠标位置
    private Vector2 prevMousePos; // 上一帧鼠标位置

    private void Start()
    {
        cam = Camera.main;
        // 偏移相关变量
        progressVec = offsetEnd.position - offsetStart.position;
        offsetLength = progressVec.magnitude;
        // 位置设置
        dragStartPos = originPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(offsetStart.position, .1f);
        Gizmos.DrawSphere(offsetEnd.position, .1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, .1f);
    }

    public override void OnDragStart()
    {
        base.OnDragStart();
        progressVec = offsetEnd.position - offsetStart.position;
        EventCenter.GetInstance().EventTrigger(GameEvent.OnDragStart);
        prevMousePos = curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        dragStartPos = transform.position;
        // 当前progress
        progressValue = Vector2.Distance(dragStartPos,offsetStart.position) / offsetLength;
    }

    public override void OnDragUpdate()
    {
        // 获取当前鼠标位置，与上一次记录鼠标位置相减，结果向量投射到progressVec上得到占比，加上该占比则为当前帧物体位置
        curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 frameOffset = curMousePos - prevMousePos;
        float offset = Vector2.Dot(progressVec, frameOffset) / (offsetLength * offsetLength);
        progressValue += offset;

        if (progressValue > 1)
            progressValue = 1;
        else if (progressValue < 0)
            progressValue = 0;

        Vector2 lerpResult = Vector2.Lerp(offsetStart.position, offsetEnd.position, progressValue);
        Vector3 finalResult = new Vector3(lerpResult.x, lerpResult.y, originPos.z);
        transform.position = finalResult;
        prevMousePos = curMousePos;
    }

    public override void OnDragEnd()
    {
        // 滑动嵌入 动画结束后设置progress
        Transform targetTsf;
        if (stopPoints.Count != 0)
            targetTsf = WayPointBehaviour.FindCloestWayPoint(stopPoints, transform.position);
        else
            targetTsf = WayPointBehaviour.FindClosestWayPoint(transform.position);
        Vector3 targetPos = targetTsf ? targetTsf.position : dragStartPos;
        transform.DOMove(new Vector3(targetPos.x,targetPos.y,transform.position.z), recoverTime).SetEase(easeType)
            .OnComplete(()=> { 
                progressValue = Vector2.Distance(targetPos, offsetStart.position) / offsetLength;
                base.OnDragEnd();
                EventCenter.GetInstance().EventTrigger(GameEvent.OnDragEnd);
            });
    }
}
