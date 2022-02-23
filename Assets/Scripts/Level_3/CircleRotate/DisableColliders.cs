using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 臭代码：在圆盘转动时关闭圆盘上地块的路径点检测 避免地块上路径点带动人物
public class DisableColliders : MonoBehaviour
{
    [SerializeField] private WayPointGroup[] wayPointGroups = default;

    /// <summary>
    /// 物体位置对上连接位置才将路径点isWalk检测打开
    /// </summary>
    [System.Serializable]
    public class WayPointGroup
    {
        public Transform mainObject;
        public Transform targetPos;
        public WayPoint[] wayPoints;
    }

    private void Update()
    {
        for(int i = 0; i < wayPointGroups.Length; ++i)
        {
            WayPointGroup wpg = wayPointGroups[i];
            if(Vector2.Distance(wpg.mainObject.position, wpg.targetPos.position) <= 0.05f)
            {
                for(int j = 0; j < wpg.wayPoints.Length; ++j)
                {
                    wpg.wayPoints[j].isWalk = true;
                }
            }
            else
            {
                for (int j = 0; j < wpg.wayPoints.Length; ++j)
                {
                    wpg.wayPoints[j].isWalk = false;
                }
            }
        }
    }
}
