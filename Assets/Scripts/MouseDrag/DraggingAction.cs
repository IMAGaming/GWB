using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拖动操作基类
/// </summary>
[RequireComponent(typeof(DraggableObject))]
public abstract class DraggingAction : MonoBehaviour
{
    /// <summary>
    /// 场景中所有物体的拖拽状态
    /// </summary>
    public static bool IsDragging { get; protected set; }

    /// <summary>
    /// 开始拖动（需要完成IsDragging = true）
    /// </summary>
    public virtual void OnDragStart() { IsDragging = true; PlayerController.Instance.isAllowMove = false; }

    /// <summary>
    /// 拖动时
    /// </summary>
    public virtual void OnDragUpdate() { }

    /// <summary>
    /// 结束拖动（需要完成IsDragging = false）
    /// </summary>
    public virtual void OnDragEnd() { IsDragging = false; PlayerController.Instance.isAllowMove = true; }

}
