using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleConShort : MonoBehaviour
{
    [SerializeField]
    Transform handlePoint;

    Transform basePoint;

    public bool isChoose1;
    public bool isChoose3;
    public bool isClick;

    private void OnMouseDown()
    {
        basePoint = handlePoint;
        isClick = true;
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

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        handlePoint = collision.transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isClick == false)
        {
            if (collision.gameObject.name == "1")
            {
                isChoose1 = true;
                isChoose3 = false;//新加的
            }
            if (collision.gameObject.name == "3")
            {
                isChoose3 = true;
                isChoose1 = false;//新加的
            }
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        handlePoint = basePoint;
        isChoose1 = false;
        isChoose3 = false;
    }
}
