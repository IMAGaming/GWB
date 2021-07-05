using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// WayPoint编辑器界面及打点逻辑处理
/// </summary>
public class WayPointCreateWizard : ScriptableWizard
{
    public GameObject wayPointPrefab;
    public List<CreateData> createDatas; // 限制要有EdgeCollider2D组件

    [MenuItem("GameObject/Create Way Points")]
    static private void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<WayPointCreateWizard>("Create Way Points", "Create");
    }

    private void OnWizardCreate()
    {
        if (createDatas == null || createDatas.Count == 0) return;
        foreach(CreateData data in createDatas)
        {
            EdgeCollider2D createTarget = data.createTarget;
            int wayPointNum = data.wayPointNum;

            // 当已存在时，销毁并重新生成
            Transform wayPoints = createTarget.transform.Find("WayPoints");
            if (wayPoints)
            {
                DestroyImmediate(wayPoints.gameObject);
                wayPoints = null;
            }

            // 创建
            if (createTarget == null) return;

            float totalDistance = 0f;
            for (int i = 0; i < createTarget.edgeCount; ++i)
            {
                Vector2 first = createTarget.points[i];
                Vector2 second = createTarget.points[i + 1];
                totalDistance += Vector2.Distance(first, second);
            }

            // 循环内需要用到的变量初始化
            Transform transform = createTarget.transform;
            int generateNum = wayPointNum - createTarget.pointCount;
            WayPoint curWayPoint = null;
            WayPoint prevWayPoint = null;
            GameObject wps = new GameObject("WayPoints");
            wps.transform.parent = transform;
            wps.transform.position = transform.position;
            int pointCount = 0;
            GameObject current = null;
            Vector2 pointVec = Vector2.zero;

            // 开始打点
            for (int i = 0; i < createTarget.edgeCount; ++i)
            {
                // 转相对坐标
                Vector2 first = createTarget.points[i] * transform.localScale + (Vector2)transform.position;
                Vector2 second = createTarget.points[i + 1] * transform.localScale + (Vector2)transform.position;
                // 左端点处理
                current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
                current.transform.position = first;
                curWayPoint = current.GetComponent<WayPoint>();
                current.name = "[" + data.createTarget.gameObject.name + "]" + "WayPoint" + pointCount++;
                if (prevWayPoint != null)
                {
                    curWayPoint.AddNeighbor(prevWayPoint);
                    prevWayPoint.AddNeighbor(curWayPoint);
                }
                prevWayPoint = curWayPoint;

                // 根据该段长度所占百分比获取应在该段上生成多少个点
                float distance = Vector2.Distance(first, second);
                int num = (int)(distance / totalDistance * generateNum);

                Vector2 baseVec = (second - first) / (num + 1);
                pointVec = first + baseVec;
                for (int j = 0; j < num; ++j, pointVec += baseVec)
                {
                    // 打点
                    current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
                    current.transform.position = pointVec;
                    curWayPoint = current.GetComponent<WayPoint>();
                    current.name = "[" + data.createTarget.gameObject.name + "]" + "WayPoint" + pointCount++;

                    // 连接
                    if (prevWayPoint != null)
                    {
                        curWayPoint.AddNeighbor(prevWayPoint);
                        prevWayPoint.AddNeighbor(curWayPoint);
                    }
                    prevWayPoint = curWayPoint;
                }
            }
            // 右端点处理
            current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
            current.transform.position = pointVec;
            curWayPoint = current.GetComponent<WayPoint>();
            current.name = "[" + data.createTarget.gameObject.name + "]" + "WayPoint" + pointCount++;
            if (prevWayPoint != null)
            {
                curWayPoint.AddNeighbor(prevWayPoint);
                prevWayPoint.AddNeighbor(curWayPoint);
            }
        }
    }
}

[System.Serializable]
public class CreateData
{
    public EdgeCollider2D createTarget;
    public int wayPointNum;
}
