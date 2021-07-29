using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateImage : MonoBehaviour
{
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;

    public float Speed = -20;   
    public GameObject Image1;
    public GameObject Image2;
    public bool isRotate;
    public bool isStop;
    public bool isChapter0End;
    public bool isChapter1End;
    public bool isChapter2End;
    public bool isChapter3End;
    //public bool isPressed;
    public float Precision = 0.005f;
    public int countingNumber = 2;

    public GameObject uiImage1;
    public GameObject uiImage2;
    public GameObject uiImage3;
    public GameObject uiImage4;

    public GameObject uiBG1;
    public GameObject uiBG2;
    public GameObject uiBG3;
    public GameObject uiBG4;

    public GameObject Front;
    public GameObject Back;

    public Animator animBG1;
    public Animator animBG2;

    public int sceneNumber;
    public int repeatNumber;
    public GameObject uiSwitchSceneImage;
    public GameObject SwitchSceneImage0;
    public GameObject SwitchSceneImage1;
    public GameObject SwitchSceneImage2;
    public GameObject SwitchSceneImage3;

    // Fixed：重复进入关卡
    private bool isChoose = false;

    Vector3 standardPoint = new Vector3(0, 0, 180);
    Vector3 secondPoint = new Vector3(0, 0, 180);

    // 场景缓存
    TargetScene loadScene = TargetScene.OPEN;

    // 若关卡通关后进入该场景，转动到下一关
    private void RotateChapter()
    {
        isStop = true;
        switch (loadScene)
        {
            case TargetScene.LEVEL0:
                isChapter1End = true;
                break;
            case TargetScene.LEVEL1:
                isChapter2End = true;
                break;
            case TargetScene.LEVEL2:
                isChapter3End = true;
                break;
        }
    }

    void Start()
    {
        //isStop = true;//在一开始的时候把isStop设为true，这样第一次点击按钮，也会换图片
        isStop = false;
        repeatNumber = sceneNumber;
        uiSwitchSceneImage = SceneTransit.Instance.selectUI;
        loadScene = SceneTransit.Instance.currentScene;

        Invoke("RotateChapter", 1.2f);
    }

    void Update()
    {
        RotatePoint();
        ButtonClick();
        ImageClick();
    }

    //麻烦的方法，留着备用吧
    /*void RotateToCorrect()
    {
        Vector3 standardPoint = new Vector3(0, -1.33f, 0);
        Vector3 standardPointDown = new Vector3(0, -9.51f, 0);
        Vector3 rotatePoint = new Vector3(0, Point, 0);

        if (Mathf.Abs(gameObject.transform.position.y - standardPoint.y) >= 0.00005f)
        {
            this.transform.RotateAround(rotatePoint, Vector3.forward, Speed * Time.deltaTime);
            //直接填一个角度，会直接瞬间转到这个角度。但这不是我们想要的，我们想要的是慢慢转到这个填入的角度。                                                        
        }
        else
            this.transform.position = standardPoint;

        if (Mathf.Abs(ui1.transform.position.y - standardPointDown.y) >= 0.00005f)//涉及到跟帧数有关的浮点运算在update里面不能直接判等于，因为浮点运算有误差，不一定能等于，故要改为相减的一个范围小于某个很小的数
        {
            ui1.transform.RotateAround(rotatePoint, Vector3.forward, Speed * Time.deltaTime);
            
        }
        else
            ui1.transform.position = standardPointDown;

    }*/

    public void StartRotate(int Number)
    {
        if(isStop == true)
        {
            sceneNumber = Number;
        }
        if(repeatNumber != sceneNumber)
        {
            isRotate = true;
        }
    }

    public void RotatePoint() //旋转点旋转
    {
        
        if (isRotate == true )
        {
            if (Mathf.Abs(this.transform.rotation.eulerAngles.z - standardPoint.z) > Precision)
            {
                gameObject.transform.Rotate(0, 0, Speed * Time.deltaTime, Space.World);
                //Debug.Log("位置信息："+Image1.transform.position+"旋转信息："+Image1.transform.rotation.eulerAngles);
                isStop = false;
            }
            else
            {
                isRotate = false;//加上去试试
                isStop = true;
                isChapter0End = false;
                isChapter1End = false;
                isChapter2End = false;
                isChapter3End = false;
                repeatNumber = sceneNumber; //停下来的时候，让repeatNumber指向当前的场景数字
                this.transform.eulerAngles = standardPoint;
            }                       
        }    
    }

    public void CountAndSwitchPoint()
    {
        //countingNumber = countingNumber +1;
        //这样写就可以防止在旋转过程中计数了
        if(isStop == true && repeatNumber != sceneNumber)
        countingNumber++;
        //Debug.Log(countingNumber);

        if (countingNumber % 2 == 0)
        {
            standardPoint = new Vector3(0, 0, 0);
        }
        else
            standardPoint = new Vector3(0, 0, 180);

        //Debug.Log(standardPoint);
    }

    public void SwitchImage(int Number)
    {
        //sceneNumber = Number;
        if(isStop == true && countingNumber > 2 && repeatNumber != sceneNumber) //第一次点击按钮或者在某一关时点击某一关时的按钮是否要切图片，可能也要关注这个if语句
        {
            if (Mathf.Abs(Image1.transform.position.y + 9.51f) < 0.05 || Mathf.Abs(Image2.transform.position.y + 9.51f) < 0.05)
            {
                switch (Number)
                {
                    case 1://按对应按键的时候要切换ui图片和背景图片,并播放对应动画
                        if(Image1.transform.position.y>Image2.transform.position.y)
                        {
                            Image2.GetComponent<SpriteRenderer>().sprite = uiImage1.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                            Image1.GetComponent<SpriteRenderer>().sprite = uiImage1.GetComponent<SpriteRenderer>().sprite;

                        //Debug.Log("切换到新手关的图片");
                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG1.SetTrigger("BGSwitch1");
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG2.SetTrigger("BGSwitch2");

                        if(Front.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Front.GetComponent<SpriteRenderer>().sprite = uiBG1.GetComponent<SpriteRenderer>().sprite;
                            Debug.Log("???");
                        }
                        else if(Back.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Back.GetComponent<SpriteRenderer>().sprite = uiBG1.GetComponent<SpriteRenderer>().sprite;
                            Debug.Log("?");
                        }

                        //SceneLoader里面ui图片换为指定关卡的图片
                        uiSwitchSceneImage.GetComponent<Image>().sprite = SwitchSceneImage0.GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 2:
                        if (Image1.transform.position.y > Image2.transform.position.y)
                        {
                            Image2.GetComponent<SpriteRenderer>().sprite = uiImage2.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                            Image1.GetComponent<SpriteRenderer>().sprite = uiImage2.GetComponent<SpriteRenderer>().sprite;
                        
                        Debug.Log("切换到第一关的图片");
                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG1.SetTrigger("BGSwitch1");
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG2.SetTrigger("BGSwitch2");

                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Front.GetComponent<SpriteRenderer>().sprite = uiBG2.GetComponent<SpriteRenderer>().sprite;
                        }
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Back.GetComponent<SpriteRenderer>().sprite = uiBG2.GetComponent<SpriteRenderer>().sprite;
                        }

                        uiSwitchSceneImage.GetComponent<Image>().sprite = SwitchSceneImage1.GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 3:
                        if (Image1.transform.position.y > Image2.transform.position.y)
                        {
                            Image2.GetComponent<SpriteRenderer>().sprite = uiImage3.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                            Image1.GetComponent<SpriteRenderer>().sprite = uiImage3.GetComponent<SpriteRenderer>().sprite;

                        Debug.Log("切换到第二关的图片");
                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG1.SetTrigger("BGSwitch1");
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG2.SetTrigger("BGSwitch2");

                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Front.GetComponent<SpriteRenderer>().sprite = uiBG3.GetComponent<SpriteRenderer>().sprite;
                        }
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Back.GetComponent<SpriteRenderer>().sprite = uiBG3.GetComponent<SpriteRenderer>().sprite;
                        }

                        uiSwitchSceneImage.GetComponent<Image>().sprite = SwitchSceneImage2.GetComponent<SpriteRenderer>().sprite;
                        break;
                    case 4:
                        if (Image1.transform.position.y > Image2.transform.position.y)
                        {
                            Image2.GetComponent<SpriteRenderer>().sprite = uiImage4.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                            Image1.GetComponent<SpriteRenderer>().sprite = uiImage4.GetComponent<SpriteRenderer>().sprite;

                        Debug.Log("切换到第三关的图片");
                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG1.SetTrigger("BGSwitch1");
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 3)
                            animBG2.SetTrigger("BGSwitch2");

                        if (Front.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Front.GetComponent<SpriteRenderer>().sprite = uiBG4.GetComponent<SpriteRenderer>().sprite;
                        }
                        else if (Back.GetComponent<SpriteRenderer>().sortingOrder == 2)
                        {
                            Back.GetComponent<SpriteRenderer>().sprite = uiBG4.GetComponent<SpriteRenderer>().sprite;
                        }

                        uiSwitchSceneImage.GetComponent<Image>().sprite = SwitchSceneImage3.GetComponent<SpriteRenderer>().sprite;
                        break;
                }
            }
        }
    }

    public void ButtonClick()
    {
        if (isChapter0End == true)
        {
            Button1.onClick.Invoke();
        }
        if (isChapter1End == true)
        {
            Button2.onClick.Invoke();
        }
        if(isChapter2End == true)
        {
            Button3.onClick.Invoke();
        }
        if (isChapter3End == true)
        {
            Button4.onClick.Invoke();
        }
    }

    public void ImageClick()
    {
        if(isStop == true && !isChoose)
        {
            // Fixed: UI交互时会触发射线检测的Bug
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hitInfo.collider?.gameObject.tag != null)
                {
                    if (hitInfo.collider.gameObject.tag == "关卡图")
                    {
	                    MusicMgr.Instance.PlaySound(MusicMgr.Instance.clickMusic, false);
                        SceneTransit.Instance.OpenSelectUI();
                        SceneTransit.Instance.RealSwitchSceneCoroutine(sceneNumber);
                        isChoose = true;
                    }
                }
            }
        }
       
    }
}
