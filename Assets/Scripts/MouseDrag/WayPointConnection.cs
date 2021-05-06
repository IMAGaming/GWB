using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
