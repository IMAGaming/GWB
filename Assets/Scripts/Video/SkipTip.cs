using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkipTip : MonoBehaviour
{
    public GameObject videoPlayerObject;

    private CGPlayer cgPlayer;
    private AudioSource audioSource;
    private Image skipUI;

    private Sequence sequence;

    private void Start()
    {
        skipUI = GetComponent<Image>();
        cgPlayer = videoPlayerObject.GetComponent<CGPlayer>();
        audioSource = videoPlayerObject.GetComponent<AudioSource>();
        skipUI.color = new Vector4(1f, 1f, 1f, 0f);
        sequence = DOTween.Sequence();
        sequence.Append(skipUI.DOFade(1f, 1f));
        sequence.AppendInterval(8f);
        sequence.Append(skipUI.DOFade(0f, 1f));
        sequence.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.DOFade(0f, 1f);
            cgPlayer.SwitchScene();
        }
    }

}
