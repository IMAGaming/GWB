using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum TargetScene { OPEN = 0, LEVEL0 = 1, LEVEL1, LEVEL2, LEVEL3, SELECT }

public class SceneTransit : MonoSingleton<SceneTransit>
{
    [SerializeField] private Animator UI_Ani = default;
    [Tooltip("黑幕时间")] [SerializeField] public float blackDuration = 0.5f;
    private Transform cam;
    public TargetScene currentScene;
    public GameObject selectUI = default;

    private void Start()
    {
        cam = Camera.main.transform;
        currentScene = (TargetScene)SceneManager.GetActiveScene().buildIndex;
    }

    // 开启选关图片
    public void OpenSelectUI()
    {
        selectUI.SetActive(true);
    }

    // 关闭选关图片
    public void CloseSelectUI()
    {
        selectUI.SetActive(false);
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
        // 防止切场景后报空引用
        cam = Camera.main.transform;

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
    /// 真正的切场景 切回选关场景
    /// </summary>
    /// <param name="scene"></param>
    public void RealSwitchSceneCoroutine(int scene)
    {
        UnityAction action = null;
        string sceneName = "";
        switch((TargetScene)scene)
        {
            // TODO：完成对选关场景的下一关布尔值设置和关卡名设置
            case TargetScene.OPEN:
                sceneName = "OpenGame";
                break;
            case TargetScene.SELECT:
                sceneName = "Select";
                break;            
            case TargetScene.LEVEL0:
                sceneName = "Level_0";
                break;            
            case TargetScene.LEVEL1:
                sceneName = "Level_1";
                break;            
            case TargetScene.LEVEL2:
                sceneName = "Level_2";
                break;
            case TargetScene.LEVEL3:
                sceneName = "Level_3";
                break;
            default:
                break;
        }

        StartCoroutine(RealSwitchScene(sceneName, action));
    }

    private IEnumerator RealSwitchScene(string name, UnityAction action)
    {
        UI_Ani.SetTrigger("fadeOut");

        // 等动画播完
        yield return new WaitForSeconds(blackDuration);

        SceneMgr.GetInstance().LoadScene(name, action);

        yield return new WaitForSeconds(0.1f);

        UI_Ani.SetTrigger("fadeIn");

        yield return new WaitForSeconds(blackDuration);

        currentScene = (TargetScene)SceneManager.GetActiveScene().buildIndex;
        CloseSelectUI();

        MusicMgr.Instance.PlayBGM();
    }

}
