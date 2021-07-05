using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneClickTip : MonoBehaviour
{
    [SerializeField] private TargetScene scene = default;
    private bool isClick = false;

    private void OnMouseDown()
    {
        if(!isClick)
        {
            transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
            SceneTransit.Instance.RealSwitchSceneCoroutine((int)scene);
            isClick = true;
        }
    }
}
