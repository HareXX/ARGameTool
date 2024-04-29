using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    //虚拟物体数据
    public List<SerVector3> position = new List<SerVector3>();
    public List<SerQuaternion> rotation = new List<SerQuaternion>();
    public List<SerVector3> scale = new List<SerVector3>();
    public List<string> name = new List<string>();

    //该物体属于第几个event
    public List<int> eventCount = new List<int>();

    //该物体的交互类型
    public List <int> interactionType = new List<int>();

    //该物体的动画类型
    public List<int> animationType = new List<int>();

    //事件链中事件数量
    public int eventLink;
    
    //事件链中事件类型
    public List<int> eventType = new List<int>();
}

[System.Serializable]
public struct SerVector3
{
    public float x; public float y; public float z;

    public SerVector3(float x_, float y_, float z_)
    {
        x = x_; y = y_; z = z_;
    }

    public SerVector3(Vector3 vector3)
    {
        x = vector3.x; y = vector3.y; z = vector3.z;
    }

    public static SerVector3 Zero
    {
        get => new SerVector3(0, 0, 0);
    }

    public void Set(float x_, float y_, float z_)
    {
        x = x_; y = y_; z = z_;
    }

    public void Set(Vector3 vector3)
    {
        x = vector3.x; y = vector3.y; z = vector3.z;
    }

    public Vector3 GetVector3()
    {
        Vector3 vector3 = Vector3.zero;
        vector3.x = x; vector3.y = y; vector3.z = z;
        return vector3;
    }
}

[System.Serializable]
public struct SerQuaternion
{
    public float x; public float y; public float z; public float w;

    public SerQuaternion(float x_, float y_, float z_, float w_)
    {
        x = x_; y = y_; z = z_; w = w_;
    }

    public SerQuaternion(Quaternion quaternion)
    {
        x = quaternion.x; y = quaternion.y; z = quaternion.z; w = quaternion.w;
    }

    public static SerQuaternion Zero
    {
        get => new SerQuaternion(0, 0, 0, 0);
    }

    public void Set(float x_, float y_, float z_, float w_)
    {
        x = x_; y = y_; z = z_; w = w_;
    }

    public void Set(Quaternion quaternion)
    {
        x = quaternion.x; y = quaternion.y; z = quaternion.z; w = quaternion.w;
    }

    public Quaternion GetQuaternion()
    {
        Quaternion quaternion = Quaternion.identity;
        quaternion.x = x; quaternion.y = y; quaternion.z = z; quaternion.w = w;
        return quaternion;
    }
}