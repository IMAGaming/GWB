using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private bool isDrag = false;
    private DraggingAction draggingAction;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            // 人物播放攀爬动画中
            if (PlayerController.Instance.isClimbing) return;
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitInfo.collider?.gameObject.tag == "DraggableObject")
            {
                isDrag = true;
                draggingAction = hitInfo.collider.GetComponent<DraggingAction>();
                draggingAction.OnDragStart();
            }
        }
        
        if(isDrag == true && DraggingAction.IsDragging)
        {
            draggingAction?.OnDragUpdate();
        }

        if(Input.GetMouseButtonUp(1))
        {
            if(draggingAction != null)
            {
                draggingAction.OnDragEnd();
                isDrag = false;
            }
        }
    }
}
