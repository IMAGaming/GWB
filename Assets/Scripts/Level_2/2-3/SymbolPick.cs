using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 碰到该物体，该物体消失，目标物体出现
/// </summary>
public class SymbolPick : MonoBehaviour
{
    // 圆盘上对应图案
    [SerializeField] private SpriteRenderer target = default;
    // 对应圆盘
    [SerializeField] private CircleRotateDrag circleRotateDrag = default;
    // 下一个要出现的GameObject
    [SerializeField] private GameObject nextSymbol = default;
    // 图案在圆盘上对应度数，用于开启事件
    [Range(-180f, 180f)] [SerializeField] private float[] targetDegrees = default;
    // 消失时间
    [SerializeField] private float fadeDuration = 1f;
    // 纵向位移
    [SerializeField] private float yOffset = .2f;

    private Sequence animSeq;

    private void Start()
    {
        animSeq = DOTween.Sequence();
        animSeq.Pause();

        Tween t1 = GetComponent<SpriteRenderer>().DOFade(0f, fadeDuration);
        animSeq.Append(t1);

        Tween t2 = transform.DOMoveY(transform.position.y + yOffset, fadeDuration)
            .OnComplete(() => Debug.Log("YOffsetFadeOut Complete"));
        animSeq.Insert(0f, t2);

        float myFloat = 0f;
        Tween t4 = DOTween.To(() => myFloat, x => myFloat = x, 0f, 0f);
        Tween t5 = DOTween.To(() => myFloat, x => myFloat = x, 0f, 0f);
        if(nextSymbol != null)
        {
            t4 = nextSymbol.GetComponent<SpriteRenderer>().DOFade(1f, fadeDuration)
            .OnStart(() =>
                {
                    nextSymbol.SetActive(true);
                    nextSymbol.transform.position = new Vector3(nextSymbol.transform.position.x,
                    nextSymbol.transform.position.y - yOffset,
                    nextSymbol.transform.position.z);
                });
            animSeq.Join(t4);
            // 延迟0.1s 在t4对Y值赋值后再完成动画
            t5 = nextSymbol.transform.DOMoveY(nextSymbol.transform.position.y, fadeDuration - 0.1f);
            animSeq.Join(t5);
            Tween t6 = nextSymbol.GetComponent<SpriteRenderer>().material.DOFade(1f, fadeDuration);
            animSeq.Join(t6);
        }

        Tween t3 = target.DOFade(1f, fadeDuration)
            .OnStart(() => target.gameObject.SetActive(true));
        animSeq.Append(t3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            for(int i = 0; i < targetDegrees.Length; ++i)
                circleRotateDrag.SetEventActive(targetDegrees[i]);
            animSeq.Goto(0f);
            animSeq.Play();
            Debug.Log("OnPick" + target.name);
        }
    }
}
