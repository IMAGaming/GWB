using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour
{

    private void OnMouseDown()
    {
        SceneTransit.Instance.RealSwitchSceneCoroutine((int)TargetScene.SELECT);
    }

}
