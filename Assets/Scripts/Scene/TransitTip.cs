using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransitTip : MonoBehaviour
{
    [SerializeField] private GameObject tipSprite = default;

    private SpriteRenderer tipSr;
    private Vector4 tipColor;

    private void Start()
    {
        if (tipSprite == null)
            tipSprite = transform.Find("TipSprite")?.gameObject;
        tipSprite.SetActive(false);
        tipSr = tipSprite.GetComponent<SpriteRenderer>();
        tipColor = tipSr.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            tipSr.DOColor(new Vector4(tipColor.x, tipColor.y, tipColor.z, 1.0f), 1.0f)
                .OnStart(() => tipSprite.SetActive(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tipSr.DOColor(new Vector4(tipColor.x, tipColor.y, tipColor.z, 0.0f), 1.0f)
                .OnComplete(() => tipSprite.SetActive(false));
        }
    }
}
