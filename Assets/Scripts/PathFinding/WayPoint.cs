using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

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

        foreach(WayPath path in neighbors)
        {
            targetPos = path.target.position;
            if(path.isActive)
            {
                if (path.isClimbDown || path.isClimbUp)
                    Gizmos.color = Color.yellow;
                else if (path.isDropDown)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.green;
                Gizmos.DrawLine(curPos, targetPos);
            }
        }
    }

    public void AddNeighbor(Transform wp, bool active = true)
    {
        neighbors.Add(new WayPath(wp, active));
    }

    public void AddNeighbor(WayPoint wp, bool active = true)
    {
        neighbors.Add(new WayPath(wp, active));
    }
}

[System.Serializable]
public class WayPath
{
    public Transform target;
    public bool isActive = true;

    [Space]

    public bool isClimbUp = false;
    public bool isClimbDown = false;
    public bool isDropDown = false;

    public WayPath(Transform _target, bool _active = true)
    {
        target = _target;
        isActive = _active;
    }

    public WayPath(WayPoint _wp, bool _active = true)
    {
        target = _wp.transform;
        isActive = _active;
    }
}
