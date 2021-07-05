using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{

    public static sceneManager instance;

    public Animator UI_Ani;

    public bool isScene1, isScene2, isScene3;
    //public Transform cam;

    int Number;
    public GameObject ui1;
    public GameObject ui2;

    void Awake()
    {
        if (instance != null)
        {
            instance.ui1 = GameObject.Find("下方图片");
            instance.ui2 = GameObject.Find("上方图片");
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this); //使得切换场景后这个manager不会消失
    }

    void Start()
    {
        //Number = GameObject.Find("旋转点").GetComponent<RotateImage>().sceneNumber;
    }

    void Update()
    {
        //StartCoroutine(switchScene());
    }

    public void UseIEnumerator()
    {
        StartCoroutine(SwitchScene());  
    }

    IEnumerator SwitchScene()
    {
        Number = GameObject.Find("旋转点").GetComponent<RotateImage>().sceneNumber;

        //播放动画
        UI_Ani.SetTrigger("fadeOut");

        //暂停一会儿，让动画播放完.这里等了1S等动画播完
        yield return new WaitForSeconds(1);

            /*switch(Number)
            {
                case 1://按对应按键的时候要切换图片和旋转图片

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
            }*/

        //播放完后切换场景
        SceneManager.LoadScene(Number);
        yield return new WaitForSeconds(1.1f);
        Debug.Log("开始自动旋转");

        //播放完之后移动摄像机
        //cam.position = new Vector3(19.2f, 4.515f,-10);


    }

   
}
