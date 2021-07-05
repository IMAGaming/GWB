using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxis : MonoBehaviour
{
    private List<float> objectsZAxis;
    private List<float> waypointsZAxis;
    [Header("SceneObject ZAxis Configuration")]
    [Tooltip("目标对象")] [SerializeField] private List<Transform> targetObjects = default;
    [Tooltip("旋转时Z值")] [SerializeField] private float targetZAxis = 0f;

    [Header("WayPoint ZAxis Configuration")]
    [Tooltip("目标路径点")] [SerializeField] private List<Transform> targetWayPoints = default;
    [Header("Target DraggingAction")]
    [Tooltip("拖拽对象")] [SerializeField] private AxisRotateDrag dragging = default;

    private void Awake()
    {
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragStart, ZStart);
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragEnd, ZEnd);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener(GameEvent.OnDragStart, ZStart);
        EventCenter.GetInstance().RemoveEventListener(GameEvent.OnDragEnd, ZEnd);
    }

    private void Start()
    {
        if (targetObjects == null)
            targetObjects = new List<Transform>();
        if (targetWayPoints == null)
            targetWayPoints = new List<Transform>();

        objectsZAxis = new List<float>();
        waypointsZAxis = new List<float>();
        for (int i = 0; i < targetObjects.Count; ++i)
        {
            objectsZAxis.Add(targetObjects[i].localPosition.z);
        }
        for (int i = 0; i < targetWayPoints.Count; ++i)
        {
            waypointsZAxis.Add(targetWayPoints[i].localPosition.z);
        }
    }

    private void ZStart()
    {
        for(int i = 0; i < targetObjects.Count; ++i)
        {
            targetObjects[i].localPosition = new Vector3(targetObjects[i].localPosition.x,
                targetObjects[i].localPosition.y, targetZAxis);
        }
    }

    private void ZEnd()
    {
        float progress = dragging.progressValue;
        if(Mathf.Approximately(progress,0))
        {
            for (int i = 0; i < targetObjects.Count; ++i)
            {
                targetObjects[i].localPosition = new Vector3(targetObjects[i].localPosition.x,
                    targetObjects[i].localPosition.y, objectsZAxis[i]);
            }
            for(int i = 0; i < targetWayPoints.Count; ++i)
            {
                targetWayPoints[i].localPosition = new Vector3(targetWayPoints[i].localPosition.x,
                    targetWayPoints[i].localPosition.y, waypointsZAxis[i]);
            }
        }
        else if(Mathf.Approximately(progress, 1f))
        {
            for (int i = 0; i < targetObjects.Count; ++i)
            {
                targetObjects[i].localPosition = new Vector3(targetObjects[i].localPosition.x,
                    targetObjects[i].localPosition.y, -objectsZAxis[i]);
            }
            for (int i = 0; i < targetWayPoints.Count; ++i)
            {
                targetWayPoints[i].localPosition = new Vector3(targetWayPoints[i].localPosition.x,
                    targetWayPoints[i].localPosition.y, -waypointsZAxis[i]);
            }
        }
    }
}
