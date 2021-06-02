using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickTip : MonoBehaviour
{
    [SerializeField] private Transform targetCameraPos = default;
    [SerializeField] private Transform targetWayPointPos = default;
    [SerializeField] private Transform finalWayPointPos = default;

    private void OnMouseDown()
    {
        transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
        PlayerController pc = PlayerController.Instance;
        if(pc.CheckWalkable(targetWayPointPos) == true && pc.isAllowMove && !pc.isClimbing)
        {
            pc.WalkCoroutine(targetWayPointPos, 1f);
            SceneTransit.Instance.SwitchSceneCoroutine(targetCameraPos.position, finalWayPointPos.position);
        }
    }
}
