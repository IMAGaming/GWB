using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickStartArea : MonoBehaviour
{
    // 是否只触发一次
    [SerializeField] private bool triggerOnce = true;
    // 触发事件
    [SerializeField] private UnityEvent triggerEvent = default;
    // 已触发过
    private bool hasTrigger = false;

    private void OnMouseDown()
    {
        if(triggerOnce)
        {
            if (!hasTrigger)
            {
                triggerEvent?.Invoke();
                hasTrigger = true;
            }
        }
        else
        {
            triggerEvent?.Invoke();
            hasTrigger = true;
        }
    }
}
