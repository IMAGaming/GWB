using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拖动操作基类
/// </summary>
[RequireComponent(typeof(DraggableObject))]
public abstract class DraggingAction : MonoBehaviour
{
    public static bool IsDragging { get; protected set; }
    /// <summary>
    /// 开始拖动（需要完成IsDragging = true）
    /// </summary>
    public virtual void OnDragStart() { IsDragging = true; }

    /// <summary>
    /// 拖动时
    /// </summary>
    public virtual void OnDragUpdate() { }

    /// <summary>
    /// 结束拖动（需要完成IsDragging = false）
    /// </summary>
    public virtual void OnDragEnd() { IsDragging = false; }

    /// <summary>
    /// 拖拽结束后的动画时间
    /// </summary>
    public float recoverTime = 1f;

}
