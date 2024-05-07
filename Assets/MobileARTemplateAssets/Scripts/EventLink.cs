using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLink : MonoBehaviour
{

    /// <summary>
    /// 事件链
    /// </summary>
    List<EventUnit> m_Link = new List<EventUnit>(); 

    public List<EventUnit> link
    {
        get => m_Link;
        set => m_Link = value;
    }

    /// <summary>
    /// 事件数量
    /// </summary>
    int m_EventCount = -1; // 0...n-1

    public int eventCount
    {
        get => m_EventCount;
        set => m_EventCount = value;
    }

    /// <summary>
    ///在事件链最后添加一个事件
    /// </summary>
    public void addEvent(int type)
    {
        EventUnit newEvent = new EventUnit();
        newEvent.setType(type);
        m_Link.Add(newEvent);
        ++m_EventCount;
    }


    /// <summary>
    /// 编辑选中的事件
    /// </summary>
    /// <param name="eventIndex">事件序号[0..n]</param>
    public void editEvent(int eventIndex)
    {
        m_Link[eventIndex].editEvent();
    }

    /// <summary>
    /// 删除选中的事件
    /// </summary>
    public void deleteEvent(int eventIndex)
    {
        m_Link[eventIndex].deleteEvent();
        m_Link.RemoveAt(eventIndex);
        --m_EventCount;
    }

    public void saveEvent(int eventIndex, List<GameObject> objects)
    {
        Debug.Log("进入EventLink");
        m_Link[eventIndex].saveEvent(objects);
    }

    /// <summary>
    /// 依次运行每个事件
    /// </summary>
    public void play(int eventIndex)
    {
        m_Link[eventIndex].play();
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
