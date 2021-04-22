using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{

    [SerializeField]private DraggingAction draggingAction;


    private void Awake()
    {
        draggingAction = GetComponent<DraggingAction>();
    }

    private void OnMouseDown()
    {
        Debug.Log("MouseDown");
        draggingAction.OnDragStart();
    }

    private void OnMouseDrag()
    {
        Debug.Log("MouseDrag");
        draggingAction.OnDragUpdate();
    }

    private void OnMouseUp()
    {
        Debug.Log("MouseUp");
        draggingAction.OnDragEnd();
    }
}
