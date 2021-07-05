using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleConMidR : MonoBehaviour
{
    [SerializeField]
    Transform handlePoint;

    Transform basePoint;

    public bool isChoose4;
    public bool isChoose6;
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
        if (isClick == false)
        {
            if (collision.gameObject.name == "44")
            {
                isChoose4 = true;
                isChoose6 = false;
            }   
            if (collision.gameObject.name == "66")
            {
                isChoose6 = true;
                isChoose4 = false;
            }  
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        handlePoint = basePoint;
        isChoose4 = false;
        isChoose6 = false;
    }
}
