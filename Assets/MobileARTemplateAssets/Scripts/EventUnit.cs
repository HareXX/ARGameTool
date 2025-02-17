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

    public string voiceInteractionSentence;

    public string voiceInteractionSentenceToCompare;

    public int gestureInteractionIndex;

    public float cosResult = -1;

    public Camera camera;

    public bool isEditing = false;

    public bool triggerDetection = false;

    //当前物体是否在摄像机范围内
    public bool objectIncamera = false;

    /// <summary>
    /// 当前事件的所有元素
    /// </summary>

    public List<GameObject> objectList = new List<GameObject>();

    public GameObject triggerObject;

    public void setType(int type)
    {
        m_ObjectType = type;
    }


    public void enableIntereaction(GameObject gameObject)
    {
        if (gameObject == null) return;
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }
        MeshCollider[] visuals = gameObject.transform.Find("Visuals").GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider visual in visuals)
        {
            visual.enabled = true;
        }
    }

    public void disableIntereaction(GameObject gameObject)
    {
        if (gameObject == null) return;
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
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
        isEditing = true;
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            //把每个物体设置成可交互
            foreach (GameObject element in objectList)
            {
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
        isEditing = true;
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
            objectList.Add(AnimationManager.instance.targetObject);
            if (AnimationManager.instance.targetTriggerObject != null)
            {
                triggerObject = AnimationManager.instance.targetTriggerObject;
                AnimationManager.instance.addDragInteractionToTrigger();
            }
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
        isEditing = false;
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            int currentIndex = EventLinkContentManager.Instance.focusedEventIndex;
            EventLinkContentManager.Instance.nextEvent();

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
            EventLinkContentManager.Instance.nextEvent();
        }
        else if (m_ObjectType == 2)
        {
            if(voiceInteractionSentence != null)
            {
                EventLinkContentManager.Instance.objectDetection = true;
                EventLinkContentManager.Instance.voiceUI.SetActive(true);
                //EditPage.instance.VoiceInteractionCanvas.SetActive(true);
                //SpeechScript.Instance.inputText = voiceInteractionSentence;
                
            }
            else if(triggerObject != null)
            {
                Debug.Log("开始触发事件");
                triggerObject.GetComponentInChildren<SphereCollider>().gameObject.SetActive(true);
                GameObject targetObject = objectList[0];
                triggerDetection = true;
            }
            
           
        }
        else if (m_ObjectType == 3)
        {
            AnimationManager.instance.playAnimation(objectList[0], animationType);
            if (EventLinkContentManager.Instance.isPlaying) EventLinkContentManager.Instance.nextEvent();
        }
    }

    public void addObject(GameObject newObject)
    {
        objectList.Add(newObject);
    }

    public void deleteEvent()
    {
        if (m_ObjectType == -1) return;
        if (m_ObjectType == 0)
        {
            int objectCnt = objectList.Count;
            for (int i = objectCnt - 1; i >= 0; --i)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
            return;
        }
        else if (m_ObjectType == 1)
        {
            int objectCnt = objectList.Count;
            for (int i = objectCnt - 1; i >= 0; --i)
            {
                Destroy(objectList[i]);
            }
            objectList.Clear();
            return;
        }
        else if (m_ObjectType == 2)
        {
            objectList.Clear();
            triggerObject = null;
        }
        else if (m_ObjectType == 3)
        {
            objectList.Clear();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

}
