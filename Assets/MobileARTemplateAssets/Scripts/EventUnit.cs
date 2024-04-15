using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventUnit : MonoBehaviour
{
    public int m_ObjectType = -1;

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

    public int animationType;

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


    public void enableIntereaction(GameObject gameObject)
    {
        if (gameObject == null) return;
        MeshCollider[] visuals = gameObject.transform.Find("Visuals").GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider visual in visuals)
        {
            visual.enabled = true;
        }
    }

    public void disableIntereaction(GameObject gameObject)
    {
        if (gameObject == null) return;
        MeshCollider[] visuals = gameObject.transform.Find("Visuals").GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider visual in visuals)
        {
            visual.enabled = false;
        }
    }

    /// <summary>
    /// 允许修改当前事件，支持和组件交互，如果是对话框的话选择编辑之后将可见
    /// </summary>
    public void editEvent()
    {
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            //把每个物体设置成可交互
            foreach (GameObject element in objectList)
            {
                Debug.Log("Oooooooooaoooo");
                enableIntereaction(element);
            }
        }
        else if (m_ObjectType == 1)
        {
            foreach (GameObject element in objectList)
            {
                element.SetActive(true);
            }
        }
        else if (m_ObjectType == 2)
        {
            //TODO 添加交互
        }
        else if (m_ObjectType == 2)
        {
            return;
        }
    }

    /// <summary>
    /// 保存当前事件，禁止修改,如果是对话框的话保存之后将不可见
    /// </summary>
    public void saveEvent(List<GameObject> objects)
    {
        Debug.Log("进入EventUnit");
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            foreach(GameObject gameObject in objects)
            {
                objectList.Add(gameObject);
                disableIntereaction(gameObject);
                //gameObject.transform.Find("Interaction Affordance").gameObject.SetActive(false);
            }

            //foreach (GameObject element in objectList)
            //{
            //    element.transform.Find("Interaction Affordance").gameObject.SetActive(false);
            //}
        }
        else if (m_ObjectType == 1)
        {
            foreach (GameObject element in objectList)
            {
                element.SetActive(false);
            }
        }
        else if (m_ObjectType == 2)
        {
            //TODO 禁止交互相关功能
            // 把关键词列表存下来, InteractionManger.getList()
        }
        else
        {
            objectList.Add(AnimationManager.instance.targetObject);
            animationType = AnimationManager.instance.animationType;
            //动画
        }
    }

    /// <summary>
    /// 运行时的操作
    /// </summary>
    public void play()
    {
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            return;
        }
        else if (m_ObjectType == 1)
        {
            foreach (GameObject element in objectList)
            {
                element.SetActive(true);
                disableIntereaction(element);
                element.transform.Find("Content Affordance").gameObject.SendMessage("play");
            }
        }
        else if (m_ObjectType == 2)
        {
            //TODO 交互相关操作
            // while(1)
            // {
            //      List2 = InteractionManager.getWords();
            //      if (InteractionManager.compare(List1, List2)) break;
            // }
        }
        else if (m_ObjectType == 3)
        {
            AnimationManager.instance.playAnimation(objectList[0], animationType);
            
        }
    }

    public void addObject(GameObject newObject)
    {
        objectList.Add(newObject);
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
