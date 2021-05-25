using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WayPointConnector : MonoBehaviour
{
    private void Awake()
    {
        WayPathUpdate();
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragEnd, WayPathUpdate);
    }

    /// <summary>
    /// 待连接路径点
    /// </summary>
    [SerializeField]
    private List<WayPointConnection> connections = default;

    /// <summary>
    /// 动画结束后判断路径更新
    /// </summary>
    public void WayPathUpdate()
    {
        foreach (WayPointConnection connection in connections)
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

/// <summary>
/// 连接类：存放条件列表和连接路径列表
/// </summary>
[Serializable]
public class WayPointConnection
{
    public List<Condition> conditions;
    public List<ConnectPath> connectPaths;
}

/// <summary>
/// 条件类：目标游戏对象到达目标位置，视为条件达成
/// </summary>
[Serializable]
public class Condition
{
    public Transform targetObject;
    public Transform targetPosition;
}

/// <summary>
/// 连接路径类：全部条件达成后要连接的路径
/// </summary>
[Serializable]
public class ConnectPath
{
    public WayPoint wayPoint;
    public WayPath wayPath;
}
