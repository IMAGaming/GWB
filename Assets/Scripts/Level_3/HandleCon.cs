using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCon : MonoBehaviour
{
    //把手的基类
    [SerializeField]
    protected Transform handlePoint;

    protected Transform basePoint;

    //public bool isChoose2;
    //public bool isChoose5;

    public bool isClick;
    public bool isCalculate;

    private void OnMouseDown()
    {
        basePoint = handlePoint;
        isClick = true;
        isCalculate = true;
    }

    private void OnMouseDrag()
    {
        transform.position = new Vector3(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x
            , transform.position.y,
            transform.position.z);
    }

    private void OnMouseUp()
    {
        transform.position = new Vector3(
            handlePoint.position.x
            , transform.position.y,
            transform.position.z);

        isClick = false;

        /*PuzzleCon.instance.CalculateTop();
        PuzzleCon.instance.CalculateMid();
        PuzzleCon.instance.CalculateBottom();*/

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        handlePoint = collision.transform;
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if(isClick == false)
        {
            if (collision.gameObject.name == "222")
            {
                isChoose2 = true;
                isChoose5 = false;
            }
                
            if (collision.gameObject.name == "555")
            {
                isChoose5 = true;
                isChoose2 = false;
            }
                
        }
        
    }*/

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        handlePoint = basePoint;
        for(int i = 0;i < 3; i++ )
        {
            PuzzleCon.instance.Solution[i] = 1;
        }
        //isChoose2 = false;
        //isChoose5 = false;
    }
}
