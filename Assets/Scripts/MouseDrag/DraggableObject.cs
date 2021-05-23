using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 可拖动对象类，管理可拖拽对象的共同属性
/// </summary>
public class DraggableObject : MonoBehaviour
{
    // 拖拽计时
    private float dragTimer = 0f;
    // 拖拽判断时间
    private const float dragTime = .1f;

    [SerializeField]private DraggingAction draggingAction;

    private void Awake()
    {
        if(draggingAction == null)
            draggingAction = GetComponent<DraggingAction>();
        if (draggingAction == null)
            Debug.LogErrorFormat("{0}没有绑定DraggingAction组件", gameObject.name);
    }

    private void OnMouseDown()
    {

    }

    // OnMouseDown先于OnMouseDrag执行
    private void OnMouseDrag()
    {
        // 人物播放攀爬动画中
        if (PlayerController.Instance.isClimbing) return;
        // 拖拽判断计时
        dragTimer += Time.deltaTime;
        if (DraggingAction.IsDragging == false && dragTimer >= dragTime)
        {
            draggingAction.OnDragStart();
        }

        // 这里逻辑有点小问题
        if(DraggingAction.IsDragging)
        {
            draggingAction.OnDragUpdate();
        }
    }

    private void OnMouseUp()
    {
        if(DraggingAction.IsDragging)
            draggingAction.OnDragEnd();
        dragTimer = 0f;
    }

}
