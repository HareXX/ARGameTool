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
using System;
using System.Threading.Tasks;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;
    public XRScreenSpaceController ScreenSpaceController;

    public GameObject targetObject;

    public int animationIndex;

    public GameObject content;

    public bool choosingObject = false;

    public GameObject confirmBox;

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
        
    }

    public void enableIntereaction(GameObject gameObject)
    {
        MeshCollider[] visuals = gameObject.transform.Find("Visuals").GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider visual in visuals)
        {
            visual.enabled = true;
        }
    }

    public void disableIntereaction(GameObject gameObject)
    {
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
        if (choosingObject == true &&  m_InteractionGroup?.focusInteractable != null)
        {
            targetObject = m_InteractionGroup?.focusInteractable.transform.gameObject;
            confirmBox.SetActive(true);
            Debug.Log("选中物体出现确认");
        }
        // 在选择物体状态下，参考ARTemplateManager，如果选中物体，则出现确定按钮
        //
    }

    public static void WaitFunctions(int waitTime)
    {
        if (waitTime <= 0) return;
        DateTime nowTimer = DateTime.Now;
        int interval = 0;
        while (interval < waitTime)
        {
            TimeSpan spand = DateTime.Now - nowTimer;
            interval = spand.Seconds;
        }

    }

    IEnumerator playDefault(GameObject gameObject)
    {
        gameObject.GetComponentInChildren<Animator>().Play("Animation", 0, 0);
        yield return new WaitForSeconds(10);
        content.GetComponent<EventLinkContentManager>().nextEvent();
    }

    IEnumerator playShake(GameObject gameObject)
    {
        gameObject.transform.DOShakePosition(10f, 0.03f);
        yield return new WaitForSeconds(10);
        content.GetComponent<EventLinkContentManager>().nextEvent();
    }

    IEnumerator playScaleUp(GameObject gameObject)
    {
        gameObject.transform.DOPunchScale(new Vector3(2f, 2f, 2f), 5f, 1, 0);
        yield return new WaitForSeconds(10);
        content.GetComponent<EventLinkContentManager>().nextEvent();
    }

    IEnumerator playScaleDown(GameObject gameObject)
    {
        gameObject.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 5f, 1, 0);
        yield return new WaitForSeconds(10);
        content.GetComponent<EventLinkContentManager>().nextEvent();
    }


    public void playAnimation(GameObject gameObject, int animationType)
    {
        if (animationType == -1) return;
        if (animationType == 0)
        {
            Debug.Log("播放default动画");
            StartCoroutine(playDefault(gameObject));
            
        }
        else if (animationType == 1)
        {
            Debug.Log("播放shake动画");
            StartCoroutine(playShake(gameObject));
        }
        else if (animationType == 2)
        {
            Debug.Log("播放变大动画");
            StartCoroutine(playScaleUp(gameObject));

        }
        else if (animationType == 3)
        {
            Debug.Log("播放缩小动画");
            StartCoroutine(playScaleDown(gameObject));
        }
    }
}
