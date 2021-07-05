using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCon : MonoBehaviour
{

    public GameObject[] Handle;
    public Animator BG_Anim;
    public Transform nextCamPos;
    public Transform nextPlayerPos;
    private bool isComplete = false;

    public bool[] Key; 
    void Start()
    {
        Key[0] = Handle[0].GetComponent<HandleCon>().isChoose2;
        Key[1] = Handle[1].GetComponent<HandleConLongR>().isChoose5;
        Key[2] = Handle[2].GetComponent<HandleConMid>().isChoose4;
        Key[3] = Handle[3].GetComponent<HandleConMidR>().isChoose6;
        Key[4] = Handle[4].GetComponent<HandleConShort>().isChoose1;
        Key[5] = Handle[5].GetComponent<HandleConShortR>().isChoose3;
    }
    void Update()
    {
        TheseAreShit();
        Decode();
        
    }

    void TheseAreShit() //先堆点重复的，考完试再改成更好的写法，先实现
    {
        if (Handle[0].GetComponent<HandleCon>().isChoose2 == true)
            Key[0] = Handle[0].GetComponent<HandleCon>().isChoose2;
        else
            Key[0] = Handle[0].GetComponent<HandleCon>().isChoose5;

        if (Handle[1].GetComponent<HandleConLongR>().isChoose2 == true)
            Key[1] = Handle[1].GetComponent<HandleConLongR>().isChoose2;
        else
            Key[1] = Handle[1].GetComponent<HandleConLongR>().isChoose5;

        if (Handle[2].GetComponent<HandleConMid>().isChoose4 == true)
            Key[2] = Handle[2].GetComponent<HandleConMid>().isChoose4;
        else
            Key[2] = Handle[2].GetComponent<HandleConMid>().isChoose6;

        if (Handle[3].GetComponent<HandleConMidR>().isChoose4 == true)
            Key[3] = Handle[3].GetComponent<HandleConMidR>().isChoose4;
        else
            Key[3] = Handle[3].GetComponent<HandleConMidR>().isChoose6;

        if (Handle[4].GetComponent<HandleConShort>().isChoose1 == true)
            Key[4] = Handle[4].GetComponent<HandleConShort>().isChoose1;
        else
            Key[4] = Handle[4].GetComponent<HandleConShort>().isChoose3;

        if (Handle[5].GetComponent<HandleConShortR>().isChoose1 == true)
            Key[5] = Handle[5].GetComponent<HandleConShortR>().isChoose1;
        else
            Key[5] = Handle[5].GetComponent<HandleConShortR>().isChoose3;

    }
    void Decode()
    {
        if (Key[0] == true && Key[1] == true && Key[2] == true && Key[3] == true && Key[4] == true && Key[5] == true && !isComplete)
        {
            Debug.Log("OK");
            BG_Anim.SetTrigger("SolveOut");
            isComplete = true;
            Invoke("SolveOut", 1.5f);
        }

    }

    void SolveOut()
    {
        SceneTransit.Instance.SwitchSceneCoroutine(nextCamPos.position, nextPlayerPos.position);
    }
}
