using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLSystem : MonoBehaviour
{
    TargetScene prevScene = TargetScene.OPEN;

    private void Awake()
    {
        prevScene = SceneTransit.Instance.currentScene;

        // 若通关，则更改解锁进度并存储
        switch(prevScene)
        {
            case TargetScene.LEVEL0:
                GameManager.instance.locks[1] = true;
                GameManager.instance.SaveButton();
                break;
            case TargetScene.LEVEL1:
                GameManager.instance.locks[2] = true;
                GameManager.instance.SaveButton();
                break;
            case TargetScene.LEVEL2:
                GameManager.instance.locks[3] = true;
                GameManager.instance.SaveButton();
                break;
            default:
                break;
        }

        // 读取进度
        GameManager.instance.LoadButton();
    }
}
