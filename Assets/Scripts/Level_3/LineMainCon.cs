using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMainCon : MonoBehaviour
{

    float baseX = 8.06f;

    public Transform hp_a; //左右挂上的位置点
    public Transform hp_b;



    //
    float nx;//当前x轴差值

    float mpx;
    float msx;

    private void Update()
    {
        //根据两个控制柄设置位置和缩放

        nx = hp_b.position.x - hp_a.position.x;


        mpx = Mathf.Min(hp_b.position.x, hp_a.position.x) + Mathf.Abs(nx) / 2.0f;

        transform.position = new Vector3(mpx, transform.position.y, transform.position.z);

        msx = nx / baseX;

        transform.localScale = new Vector3(msx, transform.localScale.y, transform.localScale.z);

        

    }





    //public void resetScale() //根据左右point重置scale 和 位置
    //{

    //}
}
