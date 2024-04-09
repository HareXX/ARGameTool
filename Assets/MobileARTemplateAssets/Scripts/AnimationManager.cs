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

    public int animationIndex;

    public GameObject content;

    public bool choosingObject = false;

    public GameObject confirmBox;

    public XRInteractionGroup m_InteractionGroup;


    private void Awake()
    {
        instance = this;

    }

    /// <summary>
    ///     -1: 未选择
    ///     0 : shake，晃动
    ///     1 : scale up，变大一下
    ///     2 : scale down, 变小一下
    /// </summary>
    public int animationTyoe = -1;

    // 有选择物体和选择动画按钮
    // 点击选择物体按钮进入选择物体状态
    // 点击选择动画按钮，出现选择动画的滚动条，选择相应动画
    void Start()
    {
        
    }

    public void chooseObject()
    {
        choosingObject = true;
        Debug.Log("开始选择物体");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            Debug.Log("Ovo");
            if (eventUnit.objectType != 0) continue;
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                gameObject.transform.Find("Visuals").GetComponent<MeshCollider>().enabled = true;
            }
        }
    }

    public void finishChoosingObject(bool ifConfirmed)
    {
        choosingObject = false;
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            if (eventUnit == null || eventUnit.objectType != 0) continue;
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                gameObject.transform.Find("Visuals").GetComponent<MeshCollider>().enabled = false;
            }
        }
        if (!ifConfirmed)
        {
            targetObject = null;
        }
    }

    public void cancelChooseAnimation()
    {
        animationTyoe = -1;
    }

    public void chooseAnimation(int type)
    {
        animationTyoe = type;
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
            Debug.Log("Damn!!!!!!!!!!");
        }
        // 在选择物体状态下，参考ARTemplateManager，如果选中物体，则出现确定按钮
        //
    }

    public void playAnimation(GameObject gameObject, int animationType)
    {
        if (animationType == 0)
        {
            gameObject.transform.DOShakePosition(10f, 0.03f);
        }
        else if (animationType == 1)
        {
            // TODO
        }
        else if (animationType == 2)
        {

        }
    }
}
