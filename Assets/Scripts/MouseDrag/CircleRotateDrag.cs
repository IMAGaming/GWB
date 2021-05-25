using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CircleRotateDrag : DraggingAction
{
    [SerializeField] private Vector2 centerPos = default;
    [SerializeField] private List<DegreeEvent> eventList = default;
    // 动画恢复时间
    [SerializeField] private float recoverTime = 1f;

    private Camera cam;
    private Vector2 startVec;
    private Vector2 rotateVec;
    private Vector2 curMousePos;
    private Vector3 originRotation;

    [System.Serializable]
    private class DegreeEvent
    {
        public float degree = default;
        public UnityEvent toRaise = default;
    }

    private void Start()
    {
        centerPos = transform.position;
        cam = Camera.main;
    }

    public override void OnDragStart()
    {
        base.OnDragStart();
        curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        rotateVec = startVec = centerPos - curMousePos;
        originRotation = transform.eulerAngles;
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
        transform.DORotate(targetRotation, recoverTime).SetEase(Ease.OutBack)
            .OnComplete(() => {
                eventList[index].toRaise?.Invoke();
                EventCenter.GetInstance().EventTrigger(GameEvent.OnDragEnd);
                originRotation = transform.eulerAngles;
                PlayerController.Instance.isAllowMove = true;
            });
    }
}
