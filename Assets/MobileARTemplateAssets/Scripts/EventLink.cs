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
    /// 当前所在事件
    /// </summary>
    int m_CurrentEventIndex = -1;

    public int currentEventIndex
    {
        get => m_CurrentEventIndex;
        set => m_CurrentEventIndex = value;
    }

    /// <summary>
    /// 事件数量
    /// </summary>
    int m_EventCount = -1;

    public int eventCount
    {
        get => m_EventCount;
        set => m_EventCount = value;
    }


    /// <summary>
    ///在事件链最后添加一个事件
    /// </summary>
    void addEvent()
    {
        EventUnit newEvent = new EventUnit();
        m_Link.Add(newEvent);
        ++m_EventCount;
    }


    /// <summary>
    /// 编辑选中的事件
    /// </summary>
    /// <param name="eventIndex">事件序号[0..n]</param>
    void editEvent(int eventIndex)
    {
        m_CurrentEventIndex = eventIndex;
        m_Link[eventIndex].editEvent();
    }

    /// <summary>
    /// 删除选中的事件
    /// </summary>
    void deleteEvent()
    {
        m_Link.RemoveAt(m_CurrentEventIndex);
        m_CurrentEventIndex = -1;
        --m_EventCount;
    }

    /// <summary>
    /// 依次运行每个事件
    /// </summary>
    void play()
    {
        foreach (EventUnit eventUnit in m_Link)
        {
            eventUnit.play();
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
