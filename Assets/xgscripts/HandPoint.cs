using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ManoMotion;

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
        //Debug.Log("ManoMotionManager");
        //Debug.Log(ManoMotionManager.Instance);
        //Debug.Log(ManoMotionManager.Instance.HandInfos);
        //if (ManoMotionManager.Instance.HandInfos[0] == HandInfo) return;
        HandInfo handInfo = ManoMotionManager.Instance.HandInfos[0];
        //Debug.Log("handInfo: ");
        //Debug.Log(handInfo);

        //HandInfo handInfo = HandSide.None;
        if (handInfo.gestureInfo.manoClass != ManoClass.NO_HAND)
            Debug.Log(handInfo.gestureInfo.manoClass);

        if (handInfo.gestureInfo.handSide == HandSide.None)
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

        //if (handInfo.gestureInfo.manoGestureTrigger == ManoGestureTrigger.PICK)
        //{
        //    Debug.Log("click");
        //    visibleBall.GetComponent<Drag>().hold();
        //}
        //else if (handInfo.gestureInfo.manoGestureContinuous == ManoGestureContinuous.POINTER_GESTURE)
        //{
        //    Debug.Log("Point");

        //}
        //else
        //{
        //    Debug.Log("drop");
        //    visibleBall.GetComponent<Drag>().release();
        //    visibleBall.GetComponent<Point>().release();
        //}

        //Vector3 dir = Camera.main.ViewportPointToRay(handInfo.trackingInfo.skeleton.jointPositions[0]).direction;
        //Vector3 palmCenterPos = Camera.main.ViewportToWorldPoint(handInfo.trackingInfo.skeleton.jointPositions[0]);
        //Vector3 newPosition = palmCenterPos + handInfo.trackingInfo.depthEstimation * dir * 0.5f;
        //// ʹ��Lerp��ƽ�����ɵ��µ�Ŀ��λ��
        ////targetPosition = Vector3.Lerp(targetPosition, newPosition, smoothSpeed * Time.deltaTime);
        //// ���������λ��
        //visibleBall.transform.position = newPosition;
    }
}
