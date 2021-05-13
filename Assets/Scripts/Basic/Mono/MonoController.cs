using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(updateEvent!=null)
        {
            updateEvent();
        }
    }

    private void FixedUpdate()
    {
        if(fixedUpdateEvent!=null)
        {
            fixedUpdateEvent();
        }
    }

    /// <summary>
    /// 添加帧更新监听
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }
    
    /// <summary>
    /// 添加FixedUpdate帧更新监听
    /// </summary>
    /// <param name="action"></param>
    public void AddFixedUpdateListener(UnityAction action)
    {
        fixedUpdateEvent += action;
    }

    /// <summary>
    /// 移除帧更新监听
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

    /// <summary>
    /// 移除FixedUpdate帧更新监听
    /// </summary>
    public void RemoveFixedUpdateListener(UnityAction action)
    {
        fixedUpdateEvent -= action;
    }
}
