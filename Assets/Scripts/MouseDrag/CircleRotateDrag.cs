using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CircleRotateDrag : DraggingAction
{
    // 音频碰撞器
    [SerializeField] private AudioCollider audioCollider = default;
    // 动画恢复时间
    [SerializeField] private float recoverTime = 1f;

    [SerializeField] private Vector2 centerPos = default;
    [SerializeField] private List<DegreeEvent> eventList = default;

    private Camera cam;
    private Vector2 startVec;
    private Vector2 rotateVec;
    private Vector2 curMousePos;
    private Vector3 originRotation;

    private Tween slideTween = null;

    [System.Serializable]
    private class MyFloatEvent : UnityEvent<float>
    {

    }

    [System.Serializable]
    private class DegreeEvent
    {
        [Range(-180f,180f)] public float degree = default;
        public bool isActive = false;
        public MyFloatEvent toRaise = default;
    }

    private void Start()
    {
        centerPos = transform.position;
        cam = Camera.main;
    }

    public override void OnDragStart()
    {
        base.OnDragStart();
        slideTween?.Kill();
        // 中心点更改
        centerPos = transform.position;
        EventCenter.GetInstance().EventTrigger(GameEvent.OnDragStart);
        curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        rotateVec = startVec = centerPos - curMousePos;
        originRotation = transform.eulerAngles;
        // 开启音频碰撞器
        if(audioCollider != null)
        {
            audioCollider.collider.enabled = true;
        }
    }

    public override void OnDragUpdate()
    {
        base.OnDragUpdate();
        curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        rotateVec = centerPos - curMousePos;
        Vector3 extraRotation = new Vector3(0, 0, originRotation.z + Vector2.SignedAngle(startVec, rotateVec));
        transform.rotation = Quaternion.Euler(extraRotation);
    }

    public override void OnDragEnd()
    {
        IsDragging = false;
        Vector3 targetRotation;
        float minDifference = float.MaxValue;
        // eulerAngles [0,360]
        float zValue = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        int index = 0;
        // 找到最近度数
        for (int i = 0; i < eventList.Count; ++i)
        {
            float difference = Mathf.Abs(eventList[i].degree - zValue);
            if (difference < minDifference)
            {
                minDifference = difference;
                index = i;
            }
        }
        targetRotation = new Vector3(0, 0, eventList[index].degree);
        slideTween = transform.DORotate(targetRotation, recoverTime).SetEase(Ease.OutBack)
            .OnComplete(() => {
                if(eventList[index].isActive)
                    eventList[index].toRaise?.Invoke(eventList[index].degree);
                EventCenter.GetInstance().EventTrigger(GameEvent.OnDragEnd);
                originRotation = transform.eulerAngles;
                PlayerController.Instance.isAllowMove = true;
                // 关闭音频碰撞器
                if(audioCollider != null)
                {
                    audioCollider.collider.enabled = false;
                }
            });
    }

    public void SetEventActive(float degree)
    {
        for (int i = 0; i < eventList.Count; ++i)
        {
            if(Mathf.Abs(eventList[i].degree - degree) <= 0.01f)
            {
                eventList[i].isActive = true;
                return;
            }
        }
        Debug.LogErrorFormat("{0}脚本上找不到角度为{1}的触发事件", this.name, degree);
    }
}
