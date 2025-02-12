using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class HandPoint : MonoBehaviour
{
    public GameObject visibleBall;
    private float smoothSpeed = 15f; // ƽ���ٶȣ����ڸ�ֵ���Կ���ƽ���ĳ̶�
    private Vector3 targetPosition; // Ŀ��λ��
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandInfo handInfo = ManomotionManager.Instance.Hand_infos[0].hand_info;

        if (handInfo.gesture_info.hand_side == HandSide.None)
        {

            if (visibleBall.activeSelf)
            {
                visibleBall.SetActive(false);
                visibleBall.GetComponent<Drag>().release();
            }
            return;
        }
        else
        {
            if (!visibleBall.activeSelf)
            {
                visibleBall.SetActive(true);
            }
        }

        if(handInfo.gesture_info.mano_gesture_trigger == ManoGestureTrigger.CLICK)
        {
            Debug.Log("click");
            visibleBall.GetComponent<Drag>().hold();
        }else if(handInfo.gesture_info.mano_gesture_trigger == ManoGestureTrigger.DROP)
        {
            Debug.Log("drop");
            visibleBall.GetComponent<Drag>().release();
        }
        Vector3 dir = Camera.main.ViewportPointToRay(handInfo.tracking_info.palm_center).direction;
        Vector3 palmCenterPos = Camera.main.ViewportToWorldPoint(handInfo.tracking_info.palm_center);
        Vector3 newPosition= palmCenterPos + handInfo.tracking_info.depth_estimation* dir *0.5f;
        // ʹ��Lerp��ƽ�����ɵ��µ�Ŀ��λ��
        //targetPosition = Vector3.Lerp(targetPosition, newPosition, smoothSpeed * Time.deltaTime);
        // ���������λ��
        visibleBall.transform.position = newPosition;
    }
}
