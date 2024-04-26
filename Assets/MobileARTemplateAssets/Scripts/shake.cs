using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class shake : MonoBehaviour
{
    public float duration = 5f; // 晃动持续时间
    public float strength = 5f; // 晃动强度

    void Start()
    {
        // 在Start方法中调用Shake方法来启动晃动
        Shake();
    }

    void Shake()
    {
        // 使用DOShakePosition方法来实现位置的晃动
        transform.DOShakePosition(duration, strength);
    }
}
