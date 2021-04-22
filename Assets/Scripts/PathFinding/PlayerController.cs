using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    // TODO:有限状态机
    public bool isMoving = false;
    public bool isClimbing = false;

    [Space]

    public Transform currentWayPoint;
    public Transform targetWayPoint;
    public Transform indicator;

    [Space]

    private List<Transform> finalPathPoints = new List<Transform>();
    public List<WayPath> finalPath = new List<WayPath>();

    [Space]
    [SerializeField] private float moveSpeed = .2f;
    [SerializeField] private float checkRange = 1.0f;
    [SerializeField] private float indicatorHeight = 0.5f;
    private Camera cam;
    private Sequence movingSequence;

    private void Start()
    {
        CheckPointDown();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckPointDown();

        if(Input.GetMouseButtonUp(0))
        {
            indicator.GetComponentInChildren<ParticleSystem>().Stop();

            Vector2 mousePos = Input.mousePosition;
            Vector2 targetPos = cam.ScreenToWorldPoint(mousePos);
            targetWayPoint = FindClosestWayPoint(targetPos);
            if(targetWayPoint == null)
            {
                return;
            }
            movingSequence.Kill();
            ClearPath();
            FindPath();

            // 指示物
            indicator.position = new Vector3(targetWayPoint.position.x, targetWayPoint.position.y + indicatorHeight, transform.position.z);
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, .5f);
        Gizmos.DrawWireCube(transform.position, new Vector2(checkRange, checkRange));
    }

    /// <summary>
    /// 检测玩家在哪个路径点上
    /// </summary>
    public void CheckPointDown()
    {
        //Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.5f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position,
            new Vector2(checkRange,checkRange), 0f);

        if (colliders.Length == 0) return;

        List<Transform> wps = new List<Transform>();

        float minDistance = float.MaxValue;
        Transform nearestWayPoint = null;
        for(int i = 0; i < colliders.Length; ++i)
        {
            if(colliders[i].CompareTag("WayPoint"))
            {
                float d = Vector2.Distance(colliders[i].transform.position, transform.position);
                if(d < minDistance)
                {
                    minDistance = d;
                    nearestWayPoint = colliders[i].transform;
                }
            }
        }

        currentWayPoint = nearestWayPoint;
    }

    /// <summary>
    /// 找到当前路径点到目的路径点的一条可行通路
    /// </summary>
    private void FindPath()
    {
        List<Transform> nextWayPoints = new List<Transform>();
        List<Transform> pastWayPoints = new List<Transform>();

        foreach(WayPath path in currentWayPoint.GetComponent<WayPoint>().neighbors)
        {
            if(path.isActive)
            {
                nextWayPoints.Add(path.target);
                path.target.GetComponent<WayPoint>().previousWayPoint = currentWayPoint;
            }
        }

        pastWayPoints.Add(currentWayPoint);

        ExploreWayPoint(nextWayPoints,pastWayPoints);
        BuildPath();
    }

    private void ExploreWayPoint(List<Transform> nextWayPoints, List<Transform> visitedWayPoints)
    {
        // 取下一个可达路径点 且根据目标路径点targetWayPoint选择优先级
        Transform current = nextWayPoints.First();
        nextWayPoints.Remove(current);

        if (current == targetWayPoint)
            return;

        foreach(WayPath path in current.GetComponent<WayPoint>().neighbors)
        {
            // 若有未访问过的可达路径点，保存
            if(!visitedWayPoints.Contains(path.target) && path.isActive)
            {
                nextWayPoints.Add(path.target);
                path.target.GetComponent<WayPoint>().previousWayPoint = current;
            }

            visitedWayPoints.Add(current);
        }

        if (nextWayPoints.Any())
        {
            ExploreWayPoint(nextWayPoints, visitedWayPoints);
        }
    }

    /// <summary>
    /// 从目的路径点的previousWayPoint往前找，若找得到当前路径点，则有可行路径
    /// </summary>
    private void BuildPath()
    {
        // 若目的路径点没有previousWayPoint，则说明找不到路径
        if (targetWayPoint.GetComponent<WayPoint>().previousWayPoint == null)
            return;

        Transform pathWayPoint = targetWayPoint;
        while(pathWayPoint != currentWayPoint)
        {
            finalPathPoints.Add(pathWayPoint);
            Transform prev = pathWayPoint.GetComponent<WayPoint>().previousWayPoint;

            // 获取边信息，存入finalPath<WayPath>
            finalPath.Add(prev.GetComponent<WayPoint>().neighbors.Find(
                x => x.target == pathWayPoint));

            if (prev != null)
                pathWayPoint = prev;
            else
                return;
        }

        FollowPath();
    }

    /// <summary>
    /// 人物行走控制
    /// </summary>
    private void FollowPath()
    {
        Debug.Log("有可行路径");

        isMoving = true;

        movingSequence = DOTween.Sequence();

        // 这里获取真实位置，在序列提前结束的情况下仍能维持同样的移动速度
        Vector2 curPos = transform.position;

        for (int i = finalPath.Count - 1; i >= 0; --i)
        {
            // 动画时间
            float time = 0f;
            if (finalPath[i].isClimbUp || finalPath[i].isClimbDown)
                time = 1.5f;
            else if (finalPath[i].isDropDown)
                time = 0.5f;
            else
                time = 1f;

            Vector2 nextPos = finalPathPoints[i].position;

            // 根据距离处理移动时间
            float distance = Vector2.Distance(curPos, nextPos);

            movingSequence.Append(transform.DOMove(finalPathPoints[i].position, distance / moveSpeed * time).SetEase(Ease.Linear));

            curPos = nextPos;
        }

        movingSequence.AppendCallback(() => ClearPath());
    }

    private void ClearPath()
    {
        foreach (WayPoint t in WayPointBehaviour.Instances)
            t.previousWayPoint = null;
        finalPathPoints.Clear();
        finalPath.Clear();
        isMoving = false;
    }

    /// <summary>
    /// 找到与给定位置最符合要求的路径点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Transform FindClosestWayPoint(Vector2 pos)
    {
        Transform target;
        // X轴范围选取(LINQ)
        var xPosList =
            from wp in WayPointBehaviour.Instances
            where Mathf.Abs(wp.transform.position.x - pos.x) <= 1.0f
            select wp;
        // 高度判断，舍去高度高于目标位置的点
        var heightList =
            from wp in xPosList
            where wp.transform.position.y <= pos.y
            select wp;
        // 距离判断
        if (!heightList.Any())
            return null;
        target = heightList.First().transform;
        float minDistance = Vector2.Distance(target.position,pos);
        foreach(WayPoint wp in heightList)
        {
            float distance = Vector2.Distance(wp.transform.position, pos);
            if (distance <= minDistance)
            {
                target = wp.transform;
                minDistance = distance;
            }
        }
        return target;
    }
}