using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum GenerateType { ByDistance, ByNum, ByPos}

[RequireComponent(typeof(EdgeCollider2D))]
public class WayPointGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wayPointPrefab;
    [Space]
    public GenerateType generateType;
    [Space]
    public int wayPointNum = 30;
    public float wayPointDistance = 1.0f;
    public List<Transform> wayPointsPos = new List<Transform>();

    public void WayPointGenerate()
    {
        // 当已存在时，销毁并重新生成
        Transform wayPoints = transform.Find("WayPoints");
        if (wayPoints)
        {
            DestroyImmediate(wayPoints.gameObject);
            wayPoints = null;
        }

        switch(generateType)
        {
            case GenerateType.ByDistance:
                GenerateByDistance();
                break;            
            case GenerateType.ByNum:
                GenerateByNum();
                break;
            case GenerateType.ByPos:
                GenerateByPos();
                break;
        }

    }

    private void GenerateByDistance()
    {
        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();
        if (collider == null) return;

        // 上一个点的相关信息
        float prevRemainDistance = 0f;
        WayPoint currentWayPoint = null;
        WayPoint prevWayPoint = null;

        for (int i = 0; i < collider.pointCount - 1; ++i)
        {
            // 相对坐标转绝对坐标
            Vector2 leftEdge = collider.points[i] * transform.localScale + (Vector2)transform.position;
            Vector2 rightEdge = collider.points[i + 1] * transform.localScale + (Vector2)transform.position;

            Vector2 distanceVec = rightEdge - leftEdge;
            Vector2 dir = distanceVec.normalized * wayPointDistance;
            float distance = dir.magnitude;

            GameObject wps = new GameObject("WayPoints");
            wps.transform.parent = transform;

            GameObject current = null;
            Vector2 pos = Vector2.zero;
            int loopCount = 0;
            for (pos = leftEdge; (pos - leftEdge).magnitude < distanceVec.magnitude; pos += dir, ++loopCount)
            {
                // 当要打最左端点且上一段打点有残留时
                if (pos == leftEdge && prevRemainDistance >= 0.01f)
                {
                    // 在距离向量上加上残留长度
                    pos += dir + distanceVec.normalized * prevRemainDistance;
                    prevRemainDistance = 0f;
                }
                current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
                current.transform.position = pos;
                current.name = "WayPoint" + loopCount;
                currentWayPoint = current.GetComponent<WayPoint>();

                // 连接
                if (prevWayPoint != null)
                {
                    currentWayPoint.AddNeighbor(prevWayPoint);
                    prevWayPoint.AddNeighbor(currentWayPoint);
                }
                prevWayPoint = currentWayPoint;
            }

            // 下一段打点需要空出的长度
            prevRemainDistance = (pos - leftEdge - distanceVec).magnitude;

            // 右端点处理
            current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
            current.transform.position = rightEdge;
            current.name = "WayPoint" + loopCount;
            currentWayPoint = current.GetComponent<WayPoint>();
            if (prevWayPoint != null)
            {
                currentWayPoint.AddNeighbor(prevWayPoint);
                prevWayPoint.AddNeighbor(currentWayPoint);
            }

        }
    }

    private void GenerateByNum()
    {
        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();
        if (collider == null) return;

        int generateNum = wayPointNum - collider.pointCount;

        float totalDistance = 0f;
        for(int i = 0; i < collider.edgeCount; ++i)
        {
            Vector2 first = collider.points[i];
            Vector2 second = collider.points[i + 1];
            totalDistance += Vector2.Distance(first, second);
        }

        // 循环内需要用到的变量初始化
        WayPoint curWayPoint = null;
        WayPoint prevWayPoint = null;
        GameObject wps = new GameObject("WayPoints");
        wps.transform.parent = transform;
        wps.transform.position = transform.position;
        int pointCount = 0;
        GameObject current = null;
        Vector2 pointVec = Vector2.zero;
        // 开始打点
        for (int i = 0; i < collider.edgeCount; ++i)
        {
            // 转相对坐标
            Vector2 first = collider.points[i] * transform.localScale + (Vector2)transform.position;
            Vector2 second = collider.points[i + 1] * transform.localScale + (Vector2)transform.position;
            // 左端点处理
            current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
            current.transform.position = first;
            current.name = "WayPoint" + pointCount++;
            curWayPoint = current.GetComponent<WayPoint>();
            if(prevWayPoint != null)
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
            for(int j = 0; j < num; ++j, pointVec += baseVec)
            {
                // 打点
                current = PrefabUtility.InstantiatePrefab(wayPointPrefab, wps.transform) as GameObject;
                current.transform.position = pointVec;
                current.name = "WayPoint" + pointCount++;
                curWayPoint = current.GetComponent<WayPoint>();

                // 连接
                if(prevWayPoint != null)
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
        current.name = "WayPoint" + pointCount++;
        curWayPoint = current.GetComponent<WayPoint>();
        if(prevWayPoint != null)
        {
            curWayPoint.AddNeighbor(prevWayPoint);
            prevWayPoint.AddNeighbor(curWayPoint);
        }
        prevWayPoint = curWayPoint;
    }

    private void GenerateByPos()
    {

    }

}
