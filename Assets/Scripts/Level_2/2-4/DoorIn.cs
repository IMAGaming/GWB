using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorIn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSr = default;
    // 走到门里的路径点
    [SerializeField] private Transform animTarget = default;
    // 瞬移路径点
    [SerializeField] private Transform target = default;
    // 后续移动路径点
    [SerializeField] private Transform endTarget = default;

    [SerializeField] private float duration = 1f;
    // 走到后续路径点的时间
    [SerializeField] private float endDuration = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerSr.DOColor(new Vector4(playerSr.color.r, playerSr.color.g, playerSr.color.b, 0),duration)
                .OnStart(() => {
                    PlayerController.Instance.WalkCoroutine(animTarget, duration);
                })
                .OnComplete(() => {
                    collision.transform.position = target.position;
                    playerSr.color = new Vector4(playerSr.color.r, playerSr.color.g, playerSr.color.b, duration);
                    StartCoroutine(enumerator());
                });          
        }
    }

    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerController.Instance.WalkCoroutine(endTarget, endDuration);
    }


}
