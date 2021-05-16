using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AxisRotateDrag : DraggingAction
{
    // Start和End对应拖动进度progress的0和1
    [SerializeField] private Transform offsetStart;
    [SerializeField] private Transform offsetEnd;

    [SerializeField] private Animator animator = default;
    private AnimatorStateInfo animatorStateInfo;

    private Camera cam;
    private float progressValue; // 当前progress值 拖动时即时更新
    private float offsetLength;
    private Vector2 progressVec; // 保存
    private Vector2 curMousePos; // 当前帧鼠标位置
    private Vector2 prevMousePos; // 上一帧鼠标位置

    private void Start()
    {
        cam = Camera.main;
        // 位移相关变量
        progressVec = offsetEnd.position - offsetStart.position;
        offsetLength = progressVec.magnitude;
        progressValue = 0f;
        // 设置动画进度 设置后动画自动暂停
        animator = GetComponent<Animator>();
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play("Rotation", 0, progressValue);
    }

    public override void OnDragStart()
    {
        IsDragging = true;
        EventCenter.GetInstance().EventTrigger(GameEvent.OnDragStart);
        prevMousePos = curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void OnDragUpdate()
    {
        // 获取当前鼠标位置，与上一帧鼠标位置相减计算偏移量
        curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 frameOffset = curMousePos - prevMousePos;
        float offset = Vector2.Dot(progressVec, frameOffset) / (offsetLength * offsetLength);
        progressValue += offset;

        if (progressValue > 1)
            progressValue = 1;
        else if (progressValue < 0)
            progressValue = 0;

        animator.Play("Rotation", 0, progressValue);

        prevMousePos = curMousePos;
    }

    public override void OnDragEnd()
    {
        IsDragging = false;
        if (progressValue >= 0.5f)
        {
            animator.SetFloat("PlaySpeed", 1f);
            progressValue = 1f;
        }
        else if (progressValue < 0.5f)
        {
            animator.SetFloat("PlaySpeed", -1f);
            progressValue = 0f;
        }
    }

}
