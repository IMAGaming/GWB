using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleConLongR : MonoBehaviour
{
    [SerializeField]
    Transform handlePoint;

    Transform basePoint;

    public bool isChoose2;
    public bool isChoose5;
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

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        handlePoint = basePoint;
        isChoose2 = false;
        isChoose5 = false;
    }
}
