using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;

[ExecuteInEditMode]
public class WayPoint : WayPointBehaviour
{
    public List<WayPath> neighbors;

    [Space]
    public Transform previousWayPoint;

    [Conditional("UNITY_EDITOR")]
    private void OnDrawGizmos()
    {
        Vector3 curPos = transform.position;
        Vector3 targetPos;

        Gizmos.DrawSphere(curPos, .1f);

        if (neighbors == null)
            return;

        foreach (WayPath path in neighbors)
        {
            if (path == null || path.target == null) continue;

            targetPos = path.target.transform.position;
            if (path.isActive)
            {
                switch(path.pathType)
                {
                    case WayPath.PathType.ClimbUp:
                    case WayPath.PathType.ClimbDown:
                        Gizmos.color = Color.yellow;
                        break;
                    case WayPath.PathType.LadderUp:
                    case WayPath.PathType.LadderDown:
                        Gizmos.color = Color.blue;
                        break;
                    case WayPath.PathType.DropDown:
                        Gizmos.color = Color.red;
                        break;
                    default:
                        Gizmos.color = Color.green;
                        break;
                }
                Gizmos.DrawLine(curPos, targetPos);
            }
        }
    }

    public void AddNeighbor(WayPoint wp, bool active = true)
    {
        neighbors.Add(new WayPath(wp, active));
    }
}

[System.Serializable]
public class WayPath : IEquatable<WayPath> // 实现比较接口供Collection的Contains和Exists调用
{
    public enum PathType { Normal, ClimbUp, ClimbDown, LadderUp, LadderDown, DropDown };

    public WayPoint target;
    public bool isActive = true;

    [Space]

    public PathType pathType = PathType.Normal;

    public WayPath(WayPoint _wp, bool _active = true)
    {
        target = _wp;
        isActive = _active;
    }

    // 为路径更新时提供比较方法
    public bool Equals(WayPath wp)
    {
        if (target.Equals(wp.target) && pathType.Equals(wp.pathType))
        {
            return true;
        }
        return false;
    }
}
