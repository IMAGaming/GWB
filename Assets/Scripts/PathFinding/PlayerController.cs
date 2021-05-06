using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Priority_Queue;

public class PlayerController : MonoSingleton<PlayerController>
{
    // TODO:有限状态机？
    public bool isMoving = false;
    public bool isClimbing = false;

    [Space]

    private Transform currentWayPoint;
    private Transform targetWayPoint;
    [SerializeField]private Transform indicator = default;

    [Space]
    // 存储寻路结果
    [SerializeField] private List<WayPath> finalPath = new List<WayPath>();

    [Space]

    [SerializeField] private float moveSpeed = .2f;
    [SerializeField] private float checkRange = 1.0f;
    [SerializeField] private float indicatorHeight = 0.5f;

    [Space]
    // 各类动作的动画时间
    // TODO：配置各类动作的动画时间
    [SerializeField] private float climbTime = default;
    [SerializeField] private float ladderTime = default;
    [SerializeField] private float dropTime = default;

    // 位移动画序列
    private Sequence movingSequence;

    // 优先队列存储
    private SimplePriorityQueue<WayPoint> openList = new SimplePriorityQueue<WayPoint>();
    private SimplePriorityQueue<WayPoint> closeList = new SimplePriorityQueue<WayPoint>();

    private Camera cam;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragStart, StopMoving);
        CheckPointDown();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckPointDown();

        // 攀爬状态与正在拖动时无法移动
        if(Input.GetMouseButtonUp(0) && !isClimbing && !DraggableObject.IsDragging)
        {
            indicator.GetComponentInChildren<ParticleSystem>().Stop();

            Vector2 mousePos = Input.mousePosition;
            Vector2 targetPos = cam.ScreenToWorldPoint(mousePos);
            targetWayPoint = WayPointBehaviour.FindClosestWayPoint(targetPos);
            if(targetWayPoint == null)
            {
                return;
            }
            movingSequence.Kill();

            ClearPath();
            AstarPathFinding();

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

        // 人物随当前路径点所在物体移动而移动
        transform.parent = currentWayPoint.parent;
    }

    /// <summary>
    /// A*寻路：找到目标点则调用BuildPath
    /// </summary>
    private void AstarPathFinding()
    {
        // 访问结点数
        int visitedCount = 0;

        Debug.Log("A*寻路：");
        WayPoint start = currentWayPoint.GetComponent<WayPoint>();
        WayPoint end = targetWayPoint.GetComponent<WayPoint>();
        if (start == null || end == null)
        {
            Debug.LogWarning("寻路起始点或终点为空");
            return;
        }

        openList.Enqueue(start, 0);
        while (openList.Count > 0)
        {
            // 取得最小cost的路径点作为当前路径点
            float currentPriority = openList.GetPriority(openList.First());
            WayPoint currentNode = openList.Dequeue();
            // 如果已经是目标点则停止
            if (currentNode.transform == targetWayPoint)
            {
                Debug.LogFormat("访问了{0}个结点", visitedCount);
                BuildPath();
                return;
            }
            // 从邻居路径点中选择符合条件的加入openList
            var currentNeighbors = currentNode.neighbors;
            for(int i = 0; i < currentNeighbors.Count; ++i)
            {
                if (currentNeighbors[i].isActive == false) continue;
                ++visitedCount; 
                WayPoint neighborNode = currentNeighbors[i].target;
                // 计算cost TODO:修改启发式
                float fCost, gCost, hCost; // f = g + h
                gCost = Vector2.Distance(start.transform.position, neighborNode.transform.position);
                hCost = Vector2.Distance(neighborNode.transform.position, end.transform.position);
                fCost = gCost + hCost;
                // 如果该邻居路径点已经在openList中，且cost高于openList中的该邻居路径点，则舍弃
                if(openList.Contains(neighborNode) && openList.GetPriority(neighborNode) - fCost <= 0.001f)
                {
                    continue;
                }
                // 如果该邻居路径点已经在closeList中，且cost高于closeList中的该邻居路径点，则舍弃
                if(closeList.Contains(neighborNode) && closeList.GetPriority(neighborNode) - fCost <= 0.001f)
                {
                    continue;
                }
                // 如果该邻居路径点存在openList/closeList中，
                // 且cost低于openList/closeList中的该邻居路径点，移除旧结点
                openList.TryRemove(neighborNode);
                closeList.TryRemove(neighborNode);
                // 新结点加入openList
                openList.Enqueue(neighborNode, fCost);
                neighborNode.previousWayPoint = currentNode.transform;
            }
            // 将当前路径点放入closeList
            closeList.Enqueue(currentNode, currentPriority);
        }
        Debug.Log("无可行路径");
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
            Transform prev = pathWayPoint.GetComponent<WayPoint>().previousWayPoint;

            // 获取边信息，存入finalPath<WayPath>
            finalPath.Add(prev.GetComponent<WayPoint>().neighbors.Find(
                x => x.target.transform == pathWayPoint));

            if (prev != null)
                pathWayPoint = prev;
            else
                return;
        }

        Debug.LogFormat("找到路径含{0}个结点", finalPath.Count + 1); // 结点数=边数+1
        FollowPath();
    }

    /// <summary>
    /// 人物行走位移动画
    /// </summary>
    private void FollowPath()
    {
        isMoving = true;

        movingSequence = DOTween.Sequence();

        // 这里获取真实位置，在序列提前结束的情况下仍能维持同样的移动速度
        Vector2 curPos = transform.position;

        for (int i = finalPath.Count - 1; i >= 0; --i)
        {
            Vector2 nextPos = finalPath[i].target.transform.position;

            // 包含动画：判断pathType插入动画
            if (finalPath[i].pathType != WayPath.PathType.Normal)
            {
                float timeCount = 0;
                // 动画播放时间
                float animTime = 0;
                switch (finalPath[i].pathType)
                {
                    case WayPath.PathType.ClimbUp:
                    case WayPath.PathType.ClimbDown:
                        animTime = climbTime;
                        break;
                    case WayPath.PathType.LadderUp:
                    case WayPath.PathType.LadderDown:
                        animTime = ladderTime;
                        break;
                    case WayPath.PathType.DropDown:
                        animTime = dropTime;
                        break;
                }
                movingSequence.Append(DOTween.To(() => timeCount, x => timeCount = x, animTime, animTime)
                    .OnStart(() => 
                    {
                        isClimbing = true;
                        // TODO：获取并修改动画状态机中bool值
                        switch(finalPath[i].pathType)
                        {
                            case WayPath.PathType.ClimbUp:
                                break;
                            case WayPath.PathType.ClimbDown:
                                break;
                            case WayPath.PathType.LadderUp:
                                break;
                            case WayPath.PathType.LadderDown:
                                break;
                            case WayPath.PathType.DropDown:
                                break;
                        }
                    }) 
                    .OnComplete(() => 
                    {
                        transform.position = new Vector3(nextPos.x, nextPos.y, transform.position.z);
                        isClimbing = false;
                        // TODO：获取并修改动画状态机中bool值
                        switch (finalPath[i].pathType)
                        {
                            case WayPath.PathType.ClimbUp:
                                break;
                            case WayPath.PathType.ClimbDown:
                                break;
                            case WayPath.PathType.LadderUp:
                                break;
                            case WayPath.PathType.LadderDown:
                                break;
                            case WayPath.PathType.DropDown:
                                break;
                        }
                    }));
                curPos = nextPos;
                continue;
            }

            // Normal：根据距离处理移动时间
            float distance = Vector2.Distance(curPos, nextPos);

            movingSequence.Append(transform.DOMove(new Vector3(nextPos.x, nextPos.y, transform.position.z),
                distance / moveSpeed).SetEase(Ease.Linear));

            curPos = nextPos;
        }

        movingSequence.AppendCallback(() => ClearPath());
    }

    private void ClearPath()
    {
        foreach (WayPoint t in WayPointBehaviour.Instances)
            t.previousWayPoint = null;
        openList.Clear();
        closeList.Clear();
        finalPath.Clear();
        isMoving = false;
    }

    private void StopMoving()
    {
        movingSequence.Kill();
        ClearPath();
    }

}