using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransit : MonoBehaviour
{
    public static SceneTransit Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = (SceneTransit)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Animator UI_Ani = default;
    [Tooltip("黑幕时间")] [SerializeField] private float blackDuration = 0.5f;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        //StartCoroutine(switchScene());
    }

    public void SwitchSceneCoroutine(Vector3 camPos, Vector3 playerPos)
    {
        StartCoroutine(SwitchScene(camPos, playerPos));
    }

    IEnumerator SwitchScene(Vector3 camPos, Vector3 playerPos)
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


}
