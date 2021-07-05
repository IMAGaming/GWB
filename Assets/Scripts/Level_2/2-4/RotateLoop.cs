using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoop : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private RotateDirection rotateDirection = RotateDirection.CW;

    public enum RotateDirection{
        CW, // 顺时针
        CCW // 逆时针
    };

    void Update()
    {
        float value = rotateDirection == RotateDirection.CCW ? speed * Time.deltaTime : -speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                              transform.rotation.eulerAngles.y,
                                              transform.rotation.eulerAngles.z + value);
    }
}
