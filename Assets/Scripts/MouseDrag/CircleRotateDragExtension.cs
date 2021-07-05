using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotateDragExtension : MonoBehaviour
{
    [SerializeField] private List<WayPoint> disableWalkPoints = default;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragStart, DisablePoints);
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragEnd, EnablePoints);
    }

    private void DisablePoints()
    {
        if(disableWalkPoints != null)
        {
            foreach(WayPoint wp in disableWalkPoints)
            {
                wp.isWalk = false;
            }
        }
    }

    private void EnablePoints()
    {
        if (disableWalkPoints != null)
        {
            foreach (WayPoint wp in disableWalkPoints)
            {
                wp.isWalk = true;
            }
        }
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener(GameEvent.OnDragStart, DisablePoints);
        EventCenter.GetInstance().RemoveEventListener(GameEvent.OnDragEnd, EnablePoints);
    }
}
