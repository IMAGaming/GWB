using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class openDoor : MonoBehaviour
{
    public int num1, num2, num3, num4;
    List<int> keyList = new List<int>();
    List<int> templateList = new List<int>();
    public int listMaxNum;

    //public Animator anim_FakeClock; //虚化的钟的animator
     List<GameObject> clockListOne = new List<GameObject>();//用来管理不在对象池中的对象
     List<GameObject> clockListTwo = new List<GameObject>();
     List<GameObject> clockListThree = new List<GameObject>();
     List<GameObject> clockListFour = new List<GameObject>();
     List<GameObject> clockListFive = new List<GameObject>();
    Dictionary<string, List<GameObject>> theDic = new Dictionary<string, List<GameObject>>();

    public Animator anim_Door;

    public GameObject fake1;
    public GameObject fake2;
    public GameObject fake3;
    public GameObject fake4;
    public GameObject fake5;
    public Transform father1;
    public Transform father2;
    public Transform father3;
    public Transform father4;
    public Transform father5;

    void Start()
    {
        setTemplateQueue();
    }

    void Update()
    {
        clickClockMgr();

        if(judgeList())
        {           
            anim_Door.SetTrigger("openDoor");
        }
    }

    void setTemplateQueue()
    {
        templateList.Add(num1);
        templateList.Add(num2);
        templateList.Add(num3);
        templateList.Add(num4);
        keyList.Add(0);
        keyList.Add(0);
        keyList.Add(0);
        keyList.Add(0);
        theDic.Add("1", clockListOne);
        theDic.Add("2", clockListTwo);
        theDic.Add("3", clockListThree);
        theDic.Add("4", clockListFour);
        theDic.Add("5", clockListFive);

        foreach (int numbers in templateList)
        {
            Debug.Log(numbers);
        }
    }

    void clickClockMgr()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //这里可以用switch语句改进一下
            if (hitInfo.collider?.gameObject.tag != null)
            {
                if (hitInfo.collider.gameObject.tag == "钟1")
                {
                    Debug.Log("这是钟1");
                    // 敲钟时玩家不允许行动
                    EventCenter.GetInstance().EventTrigger(GameEvent.StopPlayerMoving);
                    MusicMgr.Instance.PlaySound(MusicMgr.Instance.clockMusic[0], false);
                    PlayerController.Instance.isAllowMove = false;
                    keyList.Add(5);
                    StartCoroutine(clockPoolMgr(fake1,father1,"List1","1"));
                }
                else if (hitInfo.collider.gameObject.tag == "钟2")
                {
                    Debug.Log("这是钟2");
                    // 敲钟时玩家不允许行动
                    EventCenter.GetInstance().EventTrigger(GameEvent.StopPlayerMoving);
                    MusicMgr.Instance.PlaySound(MusicMgr.Instance.clockMusic[1], false);
                    PlayerController.Instance.isAllowMove = false;
                    keyList.Add(4);
                    StartCoroutine(clockPoolMgr(fake2, father2, "List2","2"));
                }
                else if (hitInfo.collider.gameObject.tag == "钟3")
                {
                    Debug.Log("这是钟3");
                    // 敲钟时玩家不允许行动
                    EventCenter.GetInstance().EventTrigger(GameEvent.StopPlayerMoving);
                    MusicMgr.Instance.PlaySound(MusicMgr.Instance.clockMusic[2], false);
                    PlayerController.Instance.isAllowMove = false;
                    keyList.Add(3);
                    StartCoroutine(clockPoolMgr(fake3, father3, "List3", "3"));
                }

                else if (hitInfo.collider.gameObject.tag == "钟4") 
                {
                    Debug.Log("这是钟4");
                    // 敲钟时玩家不允许行动
                    EventCenter.GetInstance().EventTrigger(GameEvent.StopPlayerMoving);
                    MusicMgr.Instance.PlaySound(MusicMgr.Instance.clockMusic[3], false);
                    PlayerController.Instance.isAllowMove = false;
                    keyList.Add(2);
                    StartCoroutine(clockPoolMgr(fake4, father4, "List4", "5"));
                }

                else if (hitInfo.collider.gameObject.tag == "钟5")
                {
                    Debug.Log("这是钟5");
                    // 敲钟时玩家不允许行动
                    EventCenter.GetInstance().EventTrigger(GameEvent.StopPlayerMoving);
                    MusicMgr.Instance.PlaySound(MusicMgr.Instance.clockMusic[4], false);
                    PlayerController.Instance.isAllowMove = false;
                    keyList.Add(1);
                    StartCoroutine(clockPoolMgr(fake5, father5, "List5", "5"));
                }
            }
            else
            { }
            
            if (keyList.Count > 4)
            {
                keyList.Remove(keyList[0]);
                /*Debug.Log("检查当前队列数字");
                foreach (int numbers in keyList)
                {
                    Debug.Log(numbers);
                }
                */
            }
           
        }
    }

    bool judgeList()
    {
        int j = 0;
        if(keyList.Count == templateList.Count)
        {
            for (int i =0;i<4;i++)
            {
                
                if(keyList[i] == templateList[i])
                j = j+1;
                
            }
        }

        if (j == 4)
            return true;
        else
            return false;
    }

    IEnumerator clockPoolMgr(GameObject fake,Transform father,string name,string name2)
    {
        GameObject clock = clockPool.ins.Pop(fake,father,name);
        //anim_FakeClock.SetTrigger("clickFakeClock");
        theDic[name2].Add(clock);
        clock.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        clockPool.ins.Push(clock,name);
        theDic[name2].Remove(clock);
        clock.SetActive(false);
        PlayerController.Instance.isAllowMove = true;
    }
}
