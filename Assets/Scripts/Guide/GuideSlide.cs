using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuideSlide : MonoBehaviour
{
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener(GameEvent.OnDragStart, Disappear);
    }

    private void Disappear()
    {
        GetComponent<SpriteRenderer>().DOFade(0f, 1f).OnComplete(() => Destroy(gameObject));
    }

    private void OnDisable()
    {
        EventCenter.GetInstance().RemoveEventListener(GameEvent.OnDragStart, Disappear);
    }
}
