using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    public XRScreenSpaceController ScreenSpaceController;

    public GameObject targetObject;
    public GameObject targetTriggerObject;

    public int animationIndex;

    public GameObject content;

    public bool choosingObject = false;
    public bool choosingTriggerObject = false;

    public GameObject confirmBox;
    public GameObject confirmBox1;
    public GameObject triggerConfirmBox;


    public XRInteractionGroup m_InteractionGroup;


    private void Awake()
    {
        instance = this;
        //m_InteractionGroup.ClearGroupMembers();
    }

    /// <summary>
    ///     -1: 未选择
    ///     0 : 自带动画
    ///     1 : shake，晃动
    ///     2 : scale up，变大一下
    ///     3 : scale down, 变小一下
    /// </summary>
    public int animationType = -1;

    // 有选择物体和选择动画按钮
    // 点击选择物体按钮进入选择物体状态
    // 点击选择动画按钮，出现选择动画的滚动条，选择相应动画
    void Start()
    {
        //ClearFocusObject();
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

    public void chooseObject()
    {
        choosingObject = true;
        Debug.Log("开始选择物体");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            Debug.Log("选择物体A");
            if (eventUnit.objectType != 0) continue;
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                enableIntereaction(gameObject);
            }
        }
    }

    public void chooseTriggerObject()
    {
        choosingObject = true;
        choosingTriggerObject = true;
        Debug.Log("开始选择物体");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            Debug.Log("选择触发物体");
            if (eventUnit.objectType != 0) continue;
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                enableIntereaction(gameObject);
            }
        }
    }

    public void finishChoosingObject(bool ifConfirmed)
    {
        FocusExitEventArgs args = new FocusExitEventArgs();
        args.interactableObject = m_InteractionGroup.focusInteractable;
        m_InteractionGroup.OnFocusExiting(args);
        if (m_InteractionGroup.focusInteractable == null)
        {
            Debug.Log("未找到");
        }
        else
        {
            Debug.Log("找到了");
        }

        choosingObject = false;
        Debug.Log("结束选择物体");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            if (eventUnit.objectType != 0) continue;
            Debug.Log("找到物体事件");
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                Debug.Log("找到物体");
                disableIntereaction(gameObject);
            }
        }
        if (!ifConfirmed)
        {
            targetObject = null;
        }
    }

    public void finishChoosingTriggerObject(bool ifConfirmed)
    {
        FocusExitEventArgs args = new FocusExitEventArgs();
        args.interactableObject = m_InteractionGroup.focusInteractable;
        m_InteractionGroup.OnFocusExiting(args);
        if (m_InteractionGroup.focusInteractable == null)
        {
            Debug.Log("未找到");
        }
        else
        {
            Debug.Log("找到了xg");
        }

        choosingObject = false;
        choosingTriggerObject = false;
        Debug.Log("结束选择物体");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            if (eventUnit.objectType != 0) continue;
            Debug.Log("找到物体事件");
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                Debug.Log("找到物体");
                disableIntereaction(gameObject);
            }
        }
        if (!ifConfirmed)
        {
            targetTriggerObject = null;
        }
    }

    public void cancelChooseAnimation()
    {
        animationType = -1;
    }

    public void chooseAnimation(int type)
    {
        animationType = type;
    }

    //public void play(GameObject targetObject, int animationtype)
    //{
    //}

    // Update is called once per frame

    void Update()
    {
        if (choosingObject == true && m_InteractionGroup?.focusInteractable != null)
        {
            if (choosingTriggerObject == true)
                targetTriggerObject = m_InteractionGroup?.focusInteractable.transform.gameObject;
            else
                targetObject = m_InteractionGroup?.focusInteractable.transform.gameObject;
            if (content.GetComponent<EventLinkContentManager>().eventLink.link[content.GetComponent<EventLinkContentManager>().focusedEventIndex].objectType == 2)
            {
                //Interaction
                if (choosingTriggerObject == true)
                    triggerConfirmBox.SetActive(true);
                else
                    confirmBox1.SetActive(true);
            }
            else
            {
                //Animation
                confirmBox.SetActive(true);
            }
        }
        // 在选择物体状态下，参考ARTemplateManager，如果选中物体，则出现确定按钮
        //
    }

    public void playAnimation(GameObject gameObject, int animationType)
    {
        if (animationType == -1) return;
        if (animationType == 0)
        {
            Debug.Log("播放动画");
            if (gameObject.GetComponentInChildren<Animator>() == null) return;
            gameObject.GetComponentInChildren<Animator>().Play("Animation", 0, 0);
            //gameObject.GetComponent<Animator>().Play("Animation", 0, 0);
            //gameObject.GetComponent<Animator>().SetBool("isOpen", false);
            //gameObject.GetComponent<Animator>().
        }
        else if (animationType == 1)
        {
            gameObject.transform.DOShakePosition(10f, 0.03f);
        }
        else if (animationType == 2)
        {
            gameObject.transform.DOPunchScale(new Vector3(2f, 2f, 2f), 2.5f, 1, 0);
            //gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 5f);
        }
        else if (animationType == 3)
        {
            //gameObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2.5f);
            gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 2.5f, 1, 0);
            //gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 2.5f);

        }
    }

    // 这个方法将给定的脚本附加到指定的对象上
    public void addDragInteraction()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("targetObject为null");
            return;
        }
        // 检查目标对象是否已经有这个脚本
        if (targetObject.GetComponent<Dragable>() == null)
        {
            // 如果没有，则添加脚本
            targetObject.AddComponent<Dragable>();
            Debug.Log("Dragable 已成功附加到 " + targetObject.name);
        }
        else
        {
            Debug.LogWarning(targetObject.name + " 已经有 Dragable 脚本了");
        }
    }

    
    //TODO 添加被肘的脚本
    public void addPunchInteraction()
    {
        //
    }
}
