using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Priority_Queue;

public class PlayerController : MonoBehaviour
{
    #region 单例
    public static PlayerController Instance { get; private set; }

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = (PlayerController)this;
        }
        else
        {
            Destroy(Instance);
            Instance = (PlayerController)this;
        }
    }
    #endregion

    // TODO:有限状态机？
    public bool isMoving = false;
    public bool isClimbing = false;
    public bool isAllowMove = true;

    public Transform currentWayPoint { get; private set; }
    private Transform targetWayPoint;
    //[SerializeField]private Transform indicator = default;

    [Space]
    // 存储寻路结果
    [SerializeField] private List<WayPath> finalPath = new List<WayPath>();

    [Space]

    [SerializeField] private float moveSpeed = .2f;
    [SerializeField] private float checkRange = 1.0f;

    [Space]
    // 各类动作的动画时间
    private float climbUpTime = default;
    private float climbDownTime = default;
    private float turnTime = default;

    [Space]
    [SerializeField] private Animator animator = default;
    [SerializeField] private AnimationClip climbUpAnimClip = default;
    [SerializeField] private AnimationClip climbDownAnimClip = default;
    [SerializeField] private AnimationClip turnAnimClip = default;
    [SerializeField] private AnimationClip firstStepAnimClip = default;

    // 默认渲染层级
    private int defaultSortingLayerID;
    private int defaultSortingOrder;

    private SpriteRenderer sr;
    private Transform spriteTsf;
    private Vector3 afterClimbPos;

    // 位移动画序列
    private Sequence movingSequence;

    // 优先队列存储
    private SimplePriorityQueue<WayPoint> openList = new SimplePriorityQueue<WayPoint>();
    private SimplePriorityQueue<WayPoint> closeList = new SimplePriorityQueue<WayPoint>();

    private Camera cam;

    private void Start()
    {

        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragStart, StopMoving);
        EventCenter.GetInstance().AddEventListener(GameEvent.StopPlayerMoving, StopMoving);

        cam = Camera.main;
        spriteTsf = transform.Find("Sprite");
        sr = spriteTsf.GetComponent<SpriteRenderer>();
        defaultSortingLayerID = sr.sortingLayerID;
        defaultSortingOrder = sr.sortingOrder;

        climbUpTime = climbUpAnimClip.length;
        climbDownTime = climbDownAnimClip.length;
        turnTime = turnAnimClip.length  + firstStepAnimClip.length;
        // TODO: ladderTime dropTime
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener(GameEvent.OnDragStart, StopMoving);
        EventCenter.GetInstance().RemoveEventListener(GameEvent.StopPlayerMoving, StopMoving);
    }

    private void Update()
    {
        CheckPointDown();

        // 攀爬状态与正在拖动时无法移动
        if(Input.GetMouseButtonUp(1) && !isClimbing && isAllowMove)
        {
            //indicator.GetComponentInChildren<ParticleSystem>().Stop();

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
            //indicator.position = new Vector3(targetWayPoint.position.x, targetWayPoint.position.y + indicatorHeight, transform.position.z);
            //Sequence s = DOTween.Sequence();
            //s.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
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
            if(colliders[i].CompareTag("WayPoint") && colliders[i].GetComponent<WayPoint>().isWalk)
            {
                float d = Vector2.Distance(colliders[i].transform.position, transform.position);
                if(d < minDistance)
                {
                    minDistance = d;
                    nearestWayPoint = colliders[i].transform;
                }
            }
        }
        if(nearestWayPoint != null)
            currentWayPoint = nearestWayPoint;

        // 人物随当前路径点所在物体移动而移动 且切换图层（面板上按奇数设置SortingLayer，然后将人物SortingLayer设置到当前物体SortingLayer + 1上）
        transform.parent = currentWayPoint?.parent?.parent;
        SpriteRenderer targetSr = transform.parent.GetComponent<SpriteRenderer>();
        if (targetSr.sortingLayerID == defaultSortingLayerID)
            sr.sortingOrder = targetSr.sortingOrder + 1;
        else
            sr.sortingOrder = defaultSortingOrder;
    }

    /// <summary>
    /// A*寻路：找到目标点则调用BuildPath
    /// </summary>
    private bool AstarPathFinding()
    {
        // 访问结点数
        int visitedCount = 0;

        Debug.Log("A*寻路：");
        WayPoint start = currentWayPoint.GetComponent<WayPoint>();
        WayPoint end = targetWayPoint.GetComponent<WayPoint>();
        if (start == null || end == null)
        {
            Debug.LogWarning("寻路起始点或终点为空");
            return false;
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
                return true;
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
        StopMoving();
        ClearPath();
        return false;
    }

    /// <summary>
    /// 从目的路径点的previousWayPoint往前找，若找得到当前路径点，则有可行路径
    /// </summary>
    private void BuildPath()
    {
        // 若目的路径点没有previousWayPoint，则说明目的路径点即当前路径点
        if (targetWayPoint.GetComponent<WayPoint>().previousWayPoint == null)
        {
            ClearPath();
            StopMoving();
            return;
        }

        Transform pathWayPoint = targetWayPoint;
        while(pathWayPoint != currentWayPoint)
        {
            Transform prev = pathWayPoint.GetComponent<WayPoint>().previousWayPoint;

            // 获取边信息，存入finalPath<WayPath>
            finalPath.Add(prev.GetComponent<WayPoint>().neighbors.Find(
                x => x.target.transform == pathWayPoint && x.isActive == true));

            if (prev != null)
                pathWayPoint = prev;
            else
                return;
        }
        // 结点数=边数+1
        Debug.LogFormat("找到路径含{0}个结点", finalPath.Count + 1);
        FollowPath();
    }

    /// <summary>
    /// 人物行走位移动画
    /// </summary>
    private void FollowPath()
    {
        movingSequence = DOTween.Sequence();
        animator.SetBool("Walk", true);

        if (!isMoving)
        {
            isMoving = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName("泥人站姿")
        || animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName("泥人转身执行动作")
        /*|| animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName("泥人转回站姿") */)
        {
            float timeCount = 0f;
            movingSequence.Append(DOTween.To(() => timeCount, x => timeCount = x, turnTime, turnTime)
                .OnStart(() => CheckFlip(finalPath[finalPath.Count - 1].target.transform.position)));
        }

        // 这里获取真实位置，在序列提前结束的情况下仍能维持同样的移动速度
        Vector2 curPos = transform.position;

        for (int i = finalPath.Count - 1; i >= 0; --i)
        {
            Vector2 nextPos = finalPath[i].target.transform.position;
            WayPath.PathType thisPathType = finalPath[i].pathType;

            // 包含动画：判断pathType插入动画
            if (thisPathType == WayPath.PathType.ClimbUp || thisPathType == WayPath.PathType.ClimbDown)
            {
                float timeCount = 0;
                // 动画播放时间
                float animTime = 0;
                switch (thisPathType)
                {
                    case WayPath.PathType.ClimbUp:
                        animTime = climbUpTime;
                        break;
                    case WayPath.PathType.ClimbDown:
                        animTime = climbDownTime;
                        break;
                }
                movingSequence.Append(DOTween.To(() => timeCount, x => timeCount = x, animTime, animTime).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed)
                    .OnStart(() =>
                    {
                        //spriteTsf.parent = transform.parent;
                        // Climb相关设置
                        checkOnce = false;
                        afterClimbPos = nextPos;
                        climbUpCountDown = climbDownCountDown = 0f;
                        Debug.LogFormat("OnTweenStart：{0}【{1}】【Time.time:{2}】", timeCount, climbUpCountDown, Time.time);
                        CheckFlip(nextPos);
                        isClimbing = true;
                        // TODO：获取并修改动画状态机中bool值
                        switch (thisPathType)
                        {
                            case WayPath.PathType.ClimbUp:
                                animator.SetBool("ClimbUp", true);
                                animator.Play("爬上", 0);
                                break;
                            case WayPath.PathType.ClimbDown:
                                animator.SetBool("ClimbDown", true);
                                animator.Play("爬下", 0);
                                break;
                        }
                    })
                    .OnUpdate(() =>
                    {
                        Debug.LogFormat("OnTweenUpdate：【timeCount:{0}】【countDown:{1}】【动画标准时间：{2}】【当前动画：{3}】【Time.time:{4}】",
                            timeCount, climbUpCountDown, animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).normalizedTime,
                            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name, Time.time);
                        if (animTime - timeCount <= 0.5f)
                        {
                            // TODO：获取并修改动画状态机中bool值
                            switch (thisPathType)
                            {
                                case WayPath.PathType.ClimbUp:
                                    animator.SetBool("ClimbUp", false);
                                    break;
                                case WayPath.PathType.ClimbDown:
                                    animator.SetBool("ClimbDown", false);
                                    break;
                            }
                        }
                    })
                    .OnComplete(() =>
                    {
                        //transform.position = new Vector3(nextPos.x, nextPos.y, transform.position.z);
                        //spriteTsf.position = transform.position;
                        //spriteTsf.parent = transform;
                        Debug.LogFormat("OnTweenComplete:【{0}】", climbUpCountDown);
                        checkOnce = false;
                    }));
                curPos = nextPos;
                movingSequence.AppendInterval(0.05f);
                continue;
            }

            // Normal：根据距离处理移动时间
            float distance = Vector2.Distance(curPos, nextPos);

            if(thisPathType == WayPath.PathType.Normal)
            {
                movingSequence.Append(transform.DOMove(new Vector3(nextPos.x, nextPos.y, transform.position.z), distance / moveSpeed).SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    CheckFlip(nextPos);
                    animator.SetBool("Ladder", false);
                }));
            }
            else if(thisPathType == WayPath.PathType.LadderDown || thisPathType == WayPath.PathType.LadderUp)
            {
                movingSequence.Append(transform.DOMove(new Vector3(nextPos.x, nextPos.y, transform.position.z), distance / moveSpeed).SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    CheckFlip(nextPos);
                    animator.SetBool("Ladder", true);
                    isAllowMove = false;
                })
                .OnComplete(() => 
                {
                animator.SetBool("Ladder", false);
                    isAllowMove = true;
                }));
            }

            curPos = nextPos;
        }

        movingSequence.AppendInterval(0.01f);
        movingSequence.AppendCallback(() => { ClearPath(); StopMoving(); });
    }


    // 攀爬相关
    private bool checkOnce = false;
    private float climbUpCountDown = 0f;
    private float climbDownCountDown = 0f;
    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName("爬上"))
        {
            climbUpCountDown += Time.deltaTime;
        }
        else if (animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsName("爬下"))
        {
            climbDownCountDown += Time.deltaTime;
        }
        if (climbUpCountDown - climbUpTime >= 0.001f && !checkOnce)
        {
            Debug.LogFormat("攀爬动画结束：【countDown:{0}】",climbUpCountDown);
            checkOnce = !checkOnce;
            transform.position = new Vector3(afterClimbPos.x, afterClimbPos.y, transform.position.z);
            isClimbing = false;
        }
        else if(climbDownCountDown - climbDownTime >= 0.001f && !checkOnce)
        {
            Debug.LogFormat("攀爬动画结束：【countDown:{0}】", climbDownCountDown);
            checkOnce = !checkOnce;
            transform.position = new Vector3(afterClimbPos.x, afterClimbPos.y, transform.position.z);
            isClimbing = false;
        }
    }

    // 清空路径
    private void ClearPath()
    {
        foreach (WayPoint t in WayPointBehaviour.Instances)
            t.previousWayPoint = null;
        openList.Clear();
        closeList.Clear();
        finalPath.Clear();
    }

    // 停止人物移动状态与动画
    public void StopMoving()
    {
        movingSequence.Kill();
        isMoving = false;
        animator.SetBool("Walk", false);
        animator.SetBool("Ladder", false);
    }

    private void CheckFlip(Vector3 toward)
    {
        Vector3 r = transform.rotation.eulerAngles;
        // 旋转Y值
        if(Mathf.Approximately(r.y, 180f) || Mathf.Approximately(r.y, -180f))
        {
            // 旋转Z值
            if(r.z >= 0f && r.z <= 90f || r.z >= 270f && r.z <= 360f)
            {
                sr.flipX = (toward - transform.position).x < 0 ? false : true;
            }
            else
            {
                sr.flipX = (toward - transform.position).x < 0 ? true : false;
            }
        }
        else
        {
            // 旋转Z值
            if (r.z >= 0f && r.z <= 90f || r.z >= 270f && r.z <= 360f)
            {
                sr.flipX = (toward - transform.position).x < 0 ? true : false;
            }
            else
            {
                sr.flipX = (toward - transform.position).x < 0 ? false : true;
            }
        }
    }

    // 人物行走
    public void WalkCoroutine(Transform target, float waitTime)
    {
        StartCoroutine(WalkAnimation(target, waitTime));
    }

    public bool CheckWalkable(Transform target)
    {
        targetWayPoint = target;
        return AstarPathFinding();
    }

    private IEnumerator WalkAnimation(Transform target, float waitTime)
    {
        // 检测当前路径点
        CheckPointDown();
        // 停止人物移动
        StopMoving();
        ClearPath();
        isAllowMove = false;
        // 开始朝目标点移动
        targetWayPoint = target;
        if(!CheckWalkable(target))
        {
            isAllowMove = true;
            StopCoroutine(WalkAnimation(target, waitTime));
        }

        yield return new WaitForSeconds(waitTime);

        StopMoving();
        ClearPath();
        isAllowMove = true;
    }

    public void SetPlayerMovingAllowState(bool state)
    {
        StopMoving();
        isAllowMove = state;
    }
}