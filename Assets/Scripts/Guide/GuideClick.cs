using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuideClick : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonUp(1))
        {
            GetComponent<SpriteRenderer>().DOFade(0f, 1f).OnComplete(() => Destroy(gameObject));
        }
    }
}
