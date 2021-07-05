using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 2-4中开门动画
/// </summary>
public class OpenDoorAnimation : MonoBehaviour
{
    [SerializeField] private List<PointDegree> rotatePoints = default;
    [SerializeField] private List<SpriteRenderer> fadeInPoints = default;
    [SerializeField] private Transform offsetCircle = default;
    [SerializeField] private float duration = 1f;

    [System.Serializable]
    private class PointDegree
    {
        public Transform rotatePoint = default;
        public float offsetDegree = default;
    };

    public void OpenDoorAnimaton()
    {
        for(int i = 0; i < fadeInPoints.Count; ++i)
        {
            fadeInPoints[i].DOFade(1f, duration).SetEase(Ease.Linear);
        }

        float offset = -offsetCircle.localRotation.eulerAngles.z;

        for(int i = 0; i < rotatePoints.Count; ++i)
        {
            rotatePoints[i].rotatePoint.DOLocalRotate(new Vector3(0f, 0f, offset + rotatePoints[i].offsetDegree),
                duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear);
        }
    }
}
