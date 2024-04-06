using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventLinkContentManager : MonoBehaviour
{
    public GameObject content;


    public EventLink eventLink;

    //具体事件按钮的List
    public List<GameObject> eventButtonList = new List<GameObject>();

    //具体事件按钮(预制体)
    public GameObject eventButton;

    //添加事件按钮
    public GameObject addButton;

    //添加物体按钮
    public GameObject addObjectButton;

    //添加交互按钮
    public GameObject addInteractionButton;

    //删除按钮
    public GameObject deleteButton;

    //编辑按钮
    public GameObject editButton;

    //TODO 保存按钮
    public GameObject saveButton;

    // 事件个数(1...n)，不是事件index
    public int eventCount;

    //当前选中事件
    public int focusedEventIndex = -1;

    public void Start()
    {
        eventLink = new EventLink();
        eventCount = 0;
    }

    /// <summary>
    /// 显示编辑、删除按钮
    /// TODO 保存
    /// </summary>
    public void showButtons()
    {
        deleteButton.SetActive(true);
        editButton.SetActive(true);
    }

    /// <summary>
    /// 隐藏编辑、删除按钮
    /// TODO 保存
    /// </summary>
    public void hideButtons()
    {
        deleteButton.SetActive(false);
        editButton.SetActive(false);
    }


    /// <summary>
    /// 点击某个事件按钮
    /// </summary>
    /// <param name="index">
    /// index = -1  : 点击了newEvent按钮
    /// index = k   ：点击了第k (0...n-1) 个事件
    /// </param>
    public void Click(int index)
    {
        for (int i = 0; i < eventCount; ++i)
        {
            if (i == index)
            {
                // 把第index个按钮高亮，并取消添加事件按钮的高亮
                eventButtonList[i].transform.Find("SelectionBox").gameObject.SetActive(true);
                addButton.transform.Find("SelectionBox").gameObject.SetActive(false);
            }
            else
            {
                // 取消其他事件按钮的高亮
                eventButtonList[i].transform.Find("SelectionBox").gameObject.SetActive(false);
            }
        }
        focusedEventIndex = index;  // 设置当前选中事件

        // 隐藏或者显示编辑、删除按钮等
        if (index != -1) showButtons();
        else hideButtons();
    }


    /// <summary>
    /// 新建事件
    /// </summary>
    /// <param name="type">
    ///     0: 点击添加物体按钮
    ///     1: 点击添加交互按钮
    /// </param>
    public void newEvent(int type)
    {
        GameObject newEvent = Instantiate(eventButton);         // 复制一个事件按钮
        Debug.Log(newEvent);
        eventButtonList.Add(newEvent);
        ++eventCount;
        newEvent.transform.parent = content.transform;          // 把新事件放到content下
        addButton.transform.SetSiblingIndex(eventCount + 1);    // 第一个固定是template，有eventCount个事件，所以是eventCount + 1
        if (type == 0)
        {
            TextMeshProUGUI text = newEvent.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();   //设置按钮内容
            text.SetText("Object");

            eventLink.addEvent(0);  // type 0 是物体
            //TODO 这里未将对话框和物体区分，统一是0
        }
        else
        {
            TextMeshProUGUI text = newEvent.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();   //设置按钮内容
            text.SetText("Interaction");

            eventLink.addEvent(2);  // type 2 是交互
        }
        newEvent.SetActive(true);
        addButton.transform.Find("SelectionBox").gameObject.SetActive(false);

        
    }

    /// <summary>
    /// 点击删除按钮删除事件
    /// </summary>
    public void deleteEvent()
    {
        for (int i = focusedEventIndex + 1; i < eventCount; ++i)
        {
            eventButtonList[i].GetComponent<EventButtonIndexController>().setIndex(i - 1);  //把当前事件之后的所有事件修改序号
        }
        eventLink.deleteEvent(focusedEventIndex);                                           //在事件链中删除当前选中事件
        GameObject target = eventButtonList[focusedEventIndex].transform.gameObject;        
        eventButtonList.RemoveAt(focusedEventIndex);                                        //在事件按钮中删除当前选中事件按钮
        Destroy(target);                                                                    //销毁对应事件按钮
        --eventCount;
        focusedEventIndex = -1;
    }

    /// <summary>
    /// 点击编辑按钮对事件编辑
    /// TODO 需要在eventLink中获取当前eventUnit的type，进行对应UI的展示，type = 0/1 需要展示添加物体的UI，type = 2 需要展示添加交互的UI
    /// </summary>
    public void editEvent()
    {

    }
}
