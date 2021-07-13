using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRoadDetect : MonoBehaviour
{
    [SerializeField] private Transform[] detectWayPoints = default;
    [SerializeField] private Collider2D[] disableColliders = default; 

    [SerializeField] private bool isStartDetect = false;

    public void SetDetectState(bool state)
    {
        isStartDetect = state;
    }

    // 关闭圆盘旋转
    private void DisableRotate()
    {
        for(int i = 0; i < disableColliders.Length; ++i)
        {
            disableColliders[i].enabled = false;
        }
    }

    // 开启圆盘旋转
    private void EnableRotate()
    {
        for (int i = 0; i < disableColliders.Length; ++i)
        {
            disableColliders[i].enabled = true;
        }
    }

    void Update()
    {
        if(isStartDetect)
        {
            for(int i = 0; i < detectWayPoints.Length; ++i)
            {
                // 如果人物当前位于检测点上时，关闭圆盘旋转
                if(detectWayPoints[i].Equals(PlayerController.Instance.currentWayPoint))
                {
                    DisableRotate();
                }
            }
        }
        else
        {
            EnableRotate();
        }
    }
}
