using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : Singleton<GameEvent> 
{
    // 无参事件
    public const string OnDragStart = "OnDragStart";
    public const string OnDragEnd = "OnDragEnd";
    public const string StopPlayerMoving = "StopPlayerMoving";
    public const string WayPathUpdate = "WayPathUpdate";

    // 单参数事件
    public const string ModifyMovingLock = "ModifyMovingLock";

    // 双参数事件

    // 无参/有参事件
}
