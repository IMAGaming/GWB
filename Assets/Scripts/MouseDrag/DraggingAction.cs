using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DraggingAction : MonoBehaviour
{
    /// <summary>
    /// 开始拖动
    /// </summary>
    public abstract void OnDragStart();

    /// <summary>
    /// 拖动时
    /// </summary>
    public abstract void OnDragUpdate();

    /// <summary>
    /// 结束拖动
    /// </summary>
    public abstract void OnDragEnd();

    /// <summary>
    /// 当前是否可拖动状态
    /// </summary>
    public abstract bool Draggable { get; set; }
}
