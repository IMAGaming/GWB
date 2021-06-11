using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorIn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSr = default;
    [SerializeField] private Transform animTarget = default;
    [SerializeField] private Transform target = default;
    [SerializeField] private float duration = 1f;

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
                });          
        }
    }


}
