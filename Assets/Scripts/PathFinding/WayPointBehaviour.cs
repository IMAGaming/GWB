using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class WayPointBehaviour : MonoBehaviour
{
    private static readonly HashSet<WayPointBehaviour> instances = new HashSet<WayPointBehaviour>();

    // 封装成属性 保证外界获取到元素都是拷贝
    public static HashSet<WayPointBehaviour> Instances => new HashSet<WayPointBehaviour>(instances);

    protected virtual void Awake()
    {
        instances.Add(this);
    }

    protected virtual void OnDestroy()
    {
        instances.Remove(this);
    }
}
