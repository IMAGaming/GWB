using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCon : MonoBehaviour
{
    public static PuzzleCon instance;
    public GameObject[] Handle;
    public Animator BG_Anim;
    public Transform nextCamPos;
    public Transform nextPlayerPos;

    //public bool[] Key;
    [Header("上部悬挂点")]
    public bool[] Top;

    [Header("中部悬挂点")]
    public bool[] Mid;

    [Header("下部悬挂点")]
    public bool[] Bottom;

    [Header("解谜列表")]
    public List<int> Solution = new List<int>(); //配对的数字存进来

    // 是否完成
    private bool isComplete = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        Solution.Add(1);
        Solution.Add(1);
        Solution.Add(1);

    }
    void Update()
    {
        if (Solution.Count > 3)
        {
            Solution.Remove(Solution[0]);
        }

        CalculateTop();
        CalculateMid();
        CalculateBottom();
        if(!isComplete)
        {
            Decode();
        }
    }


    //计算上部悬挂点的值
    public void CalculateTop()
    {
        if (Top[0] == true && Top[2] == true)
        {
            int isChoose13 = 13;
            Solution[0] = isChoose13;
            //Solution.Add(isChoose13);
        }

        if (Top[1] == true && Top[4] == true)
        {
            int isChoose25 = 25;
            Solution[0] = isChoose25;
            //Solution.Add(isChoose25);
        }

        if (Top[3] == true && Top[5] == true)
        {
            int isChoose46 = 46;
            Solution[0] = isChoose46;
            //Solution.Add(isChoose46);
        }
    }

    public void CalculateMid()
    {
        if (Mid[0] == true && Mid[2] == true)
        {
            int isChoose13 = 13;
            Solution[1] = isChoose13;
            //Solution.Add(isChoose13);
        }

        if (Mid[1] == true && Mid[4] == true)
        {
            int isChoose25 = 25;
            Solution[1] = isChoose25;
            //Solution.Add(isChoose25);
        }

        if (Mid[3] == true && Mid[5] == true)
        {
            int isChoose46 = 46;
            Solution[1] = isChoose46;
            //Solution.Add(isChoose46);
        }
    }

    public void CalculateBottom()
    {
        if (Bottom[0] == true && Bottom[2] == true)
        {
            int isChoose13 = 13;
            Solution[2] = isChoose13;
            //Solution.Add(isChoose13);
        }

        if (Bottom[1] == true && Bottom[4] == true)
        {
            int isChoose25 = 25;
            Solution[2] = isChoose25;
            //Solution.Add(isChoose25);
        }

        if (Bottom[3] == true && Bottom[5] == true)
        {
            int isChoose46 = 46;
            Solution[2] = isChoose46;
            //Solution.Add(isChoose46);
        }
    }

    //解开谜题的函数
    void Decode()
    {
        /*if (Key[0] == true && Key[1] == true && Key[2] == true && Key[3] == true && Key[4] == true && Key[5] == true)
        {
            Debug.Log("OK");
            BG_Anim.SetTrigger("SolveOut");
        }*/

        int answer = Solution[0] + Solution[1] + Solution[2];

        if (answer == 84)
        {
            Debug.Log("OK");
            BG_Anim.SetTrigger("SolveOut");
            Invoke("SolveOut", 1.5f);
            isComplete = true;
        }


    }

    void SolveOut()
    {
        SceneTransit.Instance.SwitchSceneCoroutine(nextCamPos.position, nextPlayerPos.position);
    }
}
