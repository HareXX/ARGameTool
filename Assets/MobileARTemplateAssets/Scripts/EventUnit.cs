using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventUnit : MonoBehaviour
{
    int m_ObjectType = -1;

    /// <summary>
    /// 元素类型
    /// 0: 物体
    /// 1: 对话框
    /// 2: 交互
    /// </summary>
    public int objectType
    {
        get => m_ObjectType;
        set => m_ObjectType = value;
    }

    /// <summary>
    /// 当前事件的所有元素
    /// </summary>
    List<GameObject> m_ObjectList = new List<GameObject>();

    public List<GameObject> objectList
    {
        get => m_ObjectList;
        set => m_ObjectList = value;
    }

    public void setType(int type)
    {
        m_ObjectType = type;
    }

    
    /// <summary>
    /// 允许修改当前事件，支持和组件交互
    /// </summary>
    public void editEvent()
    {
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            //把每个物体设置成不可交互
            foreach (GameObject element in objectList)
            {
                element.transform.Find("Interaction Affordance").gameObject.SetActive(true);
            }
        }
        else if (m_ObjectType == 1)
        {
            foreach (GameObject element in objectList)
            {
                element.transform.Find("Interaction Affordance").gameObject.SetActive(true);
            }
        }
        else
        {
            //TODO 添加交互
        }
    }

    /// <summary>
    /// 保存当前时间，禁止修改
    /// </summary>
    public void saveEvent()
    {
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            foreach (GameObject element in objectList)
            {
                element.transform.Find("Interaction Affordance").gameObject.SetActive(false);
            }
        }
        else if (m_ObjectType == 1)
        {
            foreach (GameObject element in objectList)
            {
                element.transform.Find("Interaction Affordance").gameObject.SetActive(false);
            }
        }
        else
        {
            //TODO 禁止交互相关功能
        }
    }

    /// <summary>
    /// 运行时相关操作
    /// </summary>
    public void play()
    {
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {

        }
        else if (m_ObjectType == 1)
        {

        }
        else
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
