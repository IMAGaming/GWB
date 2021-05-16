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

    /// <summary>
    /// 待连接路径点
    /// </summary>
    [SerializeField]
    protected List<WayPointConnection> connections;

    protected virtual void Awake()
    {
        WayPathUpdate();
    }

    /// <summary>
    /// 动画结束后判断路径更新
    /// </summary>
    protected void WayPathUpdate()
    {
        foreach(WayPointConnection connection in connections)
        {
            // 判断条件成立数：所有物体位置符合即条件成立
            int conditionCount = 0;
            foreach (Condition cd in connection.conditions)
            {
                if (Vector2.Distance(cd.targetObject.position, cd.targetPosition.position) <= 0.05f)
                {
                    ++conditionCount;
                }
            }
            bool isActive = (conditionCount == connection.conditions.Count);
            // 更新每一个已配置的连接，若连接不存在则新增，若连接存在则根据条件判断结果连通/关闭
            foreach (ConnectPath cp in connection.connectPaths)
            {
                WayPath foundPath = cp.wayPoint.neighbors.Find(x => x.Equals(cp.wayPath));
                if (foundPath != null)
                    foundPath.isActive = isActive;
                else
                {
                    cp.wayPath.isActive = isActive;
                    cp.wayPoint.neighbors.Add(cp.wayPath);
                }
            }
        }
    }
}
