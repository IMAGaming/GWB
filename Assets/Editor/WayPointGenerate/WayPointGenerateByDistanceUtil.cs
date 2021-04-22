using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointGenerateByDistanceUtil : MonoBehaviour
{
    public float wayPointDistance = 1.0f;
    [SerializeField] private GameObject wayPointPrefab;
/*    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;*/

    public void WayPointGenerate()
    {
        // 当已存在时，销毁并重新生成
        Transform wayPoints = transform.Find("WayPoints");
        if (wayPoints)
        {
            DestroyImmediate(wayPoints.gameObject);
            wayPoints = null;
        }

        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();

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
            for (pos = leftEdge; (pos - leftEdge).magnitude < distanceVec.magnitude ; pos += dir, ++loopCount)
            {
                // 当要打最左端点且上一段打点有残留时
                if (pos == leftEdge && prevRemainDistance >= 0.01f)
                {
                    // 在距离向量上加上残留长度
                    pos += dir + distanceVec.normalized * prevRemainDistance;
                    prevRemainDistance = 0f;
                }
                current = PrefabUtility.InstantiatePrefab(wayPointPrefab,wps.transform) as GameObject;
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
                // TODO:添加距离
            }

        }
    }
}
