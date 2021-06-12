﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransit : MonoSingleton<SceneTransit>
{

    [SerializeField] private Animator UI_Ani = default;
    [Tooltip("黑幕时间")] [SerializeField] private float blackDuration = 0.5f;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    /// <summary>
    /// 关卡内切换画面
    /// </summary>
    /// <param name="camPos">相机位置</param>
    /// <param name="playerPos">人物目标位置</param>
    public void SwitchSceneCoroutine(Vector3 camPos, Vector3 playerPos)
    {
        StartCoroutine(SwitchScene(camPos, playerPos));
    }

    private IEnumerator SwitchScene(Vector3 camPos, Vector3 playerPos)
    {
        //播放动画
        UI_Ani.SetTrigger("fadeOut");

        //暂停一会儿，让动画播放完.这里等了1S等动画播完
        yield return new WaitForSeconds(blackDuration);

        //播放完之后移动摄像机和人物位置
        cam.position = new Vector3(camPos.x, camPos.y, cam.position.z);
        PlayerController.Instance.transform.position = new Vector3(playerPos.x, playerPos.y,
            PlayerController.Instance.transform.position.z);

        yield return new WaitForSeconds(0.1f);

        UI_Ani.SetTrigger("fadeIn");
    }

    /// <summary>
    /// 真正的切换场景
    /// </summary>
    /// <param name="name">场景名字</param>
    /// <param name="action">回调函数</param>
    public void RealSwitchSceneCoroutine(string name, UnityAction action)
    {
        StartCoroutine(RealSwitchScene(name, action));
    }

    private IEnumerator RealSwitchScene(string name, UnityAction action)
    {
        UI_Ani.SetTrigger("fadeOut");

        // 等动画播完
        yield return new WaitForSeconds(blackDuration);

        SceneMgr.GetInstance().LoadScene(name, action);

        yield return new WaitForSeconds(0.1f);

        UI_Ani.SetTrigger("fadeIn");
    }

}