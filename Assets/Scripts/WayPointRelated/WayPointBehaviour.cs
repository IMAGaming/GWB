using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public abstract class WayPointBehaviour : MonoBehaviour
{
    private static readonly HashSet<WayPointBehaviour> instances = new HashSet<WayPointBehaviour>();

    // 封装成属性 保证外界获取到元素都是拷贝
    public static HashSet<WayPointBehaviour> Instances => new HashSet<WayPointBehaviour>(instances);

    /// <summary>
    /// 是否允许玩家行走的路径点
    /// </summary>
    public bool isWalk = true;

    /// <summary>
    /// 是否允许玩家鼠标点击检测到的路径点
    /// </summary>
    public bool isCheck = true;

    protected virtual void Awake()
    {
        instances.Add(this);
    }

    protected virtual void OnDestroy()
    {
        instances.Remove(this);
    }

    /// <summary>
    /// 找到与给定位置最符合要求的路径点
    /// </summary>
    /// <param name="pos">给定位置</param>
    /// <returns></returns>
    public static Transform FindClosestWayPoint(Vector2 pos)
    {
        Transform target;
        // X轴范围选取(LINQ)
        var xPosList =
            from wp in Instances
            where Mathf.Abs(wp.transform.position.x - pos.x) <= 1.0f && wp.isCheck
            select wp;
        // 高度判断，舍去高度与目标位置高度相差0.5以上的点
        var heightList =
            from wp in xPosList
            where (wp.transform.position.y - pos.y) <= 0.5f
            select wp;
        // 距离判断
        if (!heightList.Any())
            return null;
        target = heightList.First().transform;
        float minDistance = Vector2.Distance(target.position, pos);
        foreach (WayPoint wp in heightList)
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

    /// <summary>
    /// 在路径点列表中查找离给定位置最近的点
    /// </summary>
    /// <param name="wps">路径点列表</param>
    /// <param name="pos">给定位置</param>
    /// <returns></returns>
    public static Transform FindCloestWayPoint(IEnumerable<WayPoint> wps, Vector2 pos)
    {
        Transform target;

        target = wps.First().transform;
        float minDistance = Vector2.Distance(target.position, pos);
        foreach (WayPoint wp in wps)
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
