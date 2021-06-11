using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickTip : MonoBehaviour
{
    [SerializeField] private Transform targetCameraPos = default;
    [SerializeField] private Transform targetWayPointPos = default;
    [SerializeField] private Transform finalWayPointPos = default;
    [SerializeField] private bool isCheckPickUp = false;

    private void OnMouseDown()
    {
        transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
        PlayerController pc = PlayerController.Instance;

        // 如果设置了检查捡起物，则检查不到时不发生切换
        if(isCheckPickUp == true && pc.gameObject.GetComponent<PickUp>().IsPick == false)
            return;

        if(pc.CheckWalkable(targetWayPointPos) == true && pc.isAllowMove && !pc.isClimbing)
        {
            pc.WalkCoroutine(targetWayPointPos, 1f);
            SceneTransit.Instance.SwitchSceneCoroutine(targetCameraPos.position, finalWayPointPos.position);
        }
    }
}
