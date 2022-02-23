using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonTip : MonoBehaviour
{
    [SerializeField] private Image clickTip = default;

    // K：关卡 V：颜色
    private Dictionary<int, string> colorDic = default;

    private void Awake()
    {
        colorDic = new Dictionary<int, string>();
        colorDic.Add(0, "#ffb57c");
        colorDic.Add(1, "#affa9a");
        colorDic.Add(2, "#faf0a8");
        colorDic.Add(3, "#f99b9b");
    }

    private void Start()
    {
        TipFadeIn();
        ButtonColorfy();
    }

    public void TipFadeIn()
    {
        clickTip?.DOFade(1f, 1f);
    }

    public void TipFadeOut()
    {
        clickTip?.DOFade(0f, 1f);
    }

    // 根据解锁情况更改按钮颜色
    private void ButtonColorfy()
    {
        for(int i = 0; i < colorDic.Count; ++i)
        {
            Color targetColor;
            if(ColorUtility.TryParseHtmlString(colorDic[i], out targetColor))
            {
                // 判断是否解锁
                if(GameManager.instance.locks[i] == true)
                {
                    transform.Find("Button" + i).GetComponent<Image>().color = targetColor;
                }
            }
        }
    }
}
