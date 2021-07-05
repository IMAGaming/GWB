using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SymbolManager : MonoBehaviour
{
    [SerializeField] private Transform down_TA = default;
    [SerializeField] private Transform down_One = default;
    [SerializeField] private Transform down_M = default;
    [SerializeField] private Transform up_LTA = default;
    [SerializeField] private Transform up_RTA = default;
    [SerializeField] private Transform up_One = default;
    [SerializeField] private Transform up_M = default;
    [SerializeField] private float fadeDuration = 1f;

    private void FadeIn(Transform tsf)
    {
        tsf.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f);
        tsf.gameObject.SetActive(true);
        tsf.GetComponent<SpriteRenderer>().DOFade(1f, fadeDuration);
    }

    public void Down_TA()
    {
        FadeIn(down_TA);
    }

    public void Down_One()
    {
        FadeIn(down_One);
    } 

    public void Down_M()
    {
        FadeIn(down_M);
    }

    public void Up_LTA()
    {
        FadeIn(up_LTA);
    }

    public void Up_RTA()
    {
        FadeIn(up_RTA);
    } 

    public void Up_One()
    {
        FadeIn(up_One);
    }

    public void Up_M()
    {
        FadeIn(up_M);
    }
}
