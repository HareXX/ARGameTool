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

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;
    public XRScreenSpaceController ScreenSpaceController;

    public GameObject targetObject;

    public int animationIndex;

    public GameObject content;

    public bool choosingObject = false;

    public GameObject confirmBox;

    public XRInteractionGroup m_InteractionGroup;


    private void Awake()
    {
        Instance = this;
        //m_InteractionGroup.ClearGroupMembers();
    }

    /// <summary>
    ///     -1: δѡ��
    ///     0 : �Դ�����
    ///     1 : shake���ζ�
    ///     2 : scale up�����һ��
    ///     3 : scale down, ��Сһ��
    /// </summary>
    public int animationType = -1;

    // ��ѡ�������ѡ�񶯻���ť
    // ���ѡ�����尴ť����ѡ������״̬
    // ���ѡ�񶯻���ť������ѡ�񶯻��Ĺ�������ѡ����Ӧ����
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
        Debug.Log("��ʼѡ������");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            Debug.Log("Ovo");
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
            Debug.Log("wuhu!!!!!!!!");
        }
        else
        {
            Debug.Log("FFFFFFFFFF");
        }

        choosingObject = false;
        Debug.Log("����ѡ������");
        foreach (EventUnit eventUnit in content.GetComponent<EventLinkContentManager>().eventLink.link)
        {
            if (eventUnit.objectType != 0) continue;
            Debug.Log("�ҵ������¼�");
            foreach (GameObject gameObject in eventUnit.objectList)
            {
                Debug.Log("�ҵ�����");
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
        if (choosingObject == true && m_InteractionGroup?.focusInteractable != null)
        {
            targetObject = m_InteractionGroup?.focusInteractable.transform.gameObject;
            confirmBox.SetActive(true);
            Debug.Log("Damn!!!!!!!!!!");
        }
        // ��ѡ������״̬�£��ο�ARTemplateManager�����ѡ�����壬�����ȷ����ť
        //
    }

    public void playAnimation(GameObject gameObject, int animationType)
    {
        if (animationType == -1) return;
        if (animationType == 0)
        {
            Debug.Log("���Ŷ���");
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
}
