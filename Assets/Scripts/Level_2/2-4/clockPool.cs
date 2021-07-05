using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockPool : MonoBehaviour
{
    public static clockPool ins;

    //public Transform fatherClock;
    //public GameObject fakeClock;
    List<GameObject> clockList = new List<GameObject>();
    List<GameObject> clockList2 = new List<GameObject>();
    List<GameObject> clockList3 = new List<GameObject>();
    List<GameObject> clockList4 = new List<GameObject>();
    List<GameObject> clockList5 = new List<GameObject>();

    Dictionary<string, List<GameObject>> clockList_Dic = new Dictionary<string, List<GameObject>>();
    public int listNum = 5;

    void Awake()
    {
        ins = this;
    }

    void Start()
    {
        clockList_Dic.Add("List1", clockList);
        clockList_Dic.Add("List2", clockList2);
        clockList_Dic.Add("List3", clockList3);
        clockList_Dic.Add("List4", clockList4);
        clockList_Dic.Add("List5", clockList5);
    }
    void Update()
    {
        
    }

    public void Push(GameObject clock,string name)
    {
        if(clockList_Dic[name].Count < listNum)
        {
            clockList_Dic[name].Add(clock);
        }
        else
        {
            Destroy(clock);
        }
        
    }

    //传参数进去，让Instantiate()里面的两个物体是可变的
    public GameObject Pop(GameObject fake,Transform father,string name)
    {
        
        if(clockList_Dic[name].Count > 0)
        {
            GameObject clock = clockList_Dic[name][0];
            clockList_Dic[name].Remove(clockList_Dic[name][0]);
            return clock;
        }
        else 
        {
            return Instantiate(fake, father); //我想让子类控制生成我指定的物体，引用我指定的物体。感觉是应该让子类继承这个类然后重写pop和push函数？
            
        }
        
    }

    //当切换场景的时候，清空对象池
    public void Clear()
    {
        clockList.Clear();
    }
}
