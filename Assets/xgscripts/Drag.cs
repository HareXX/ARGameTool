using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private float detectionRadius = 0.025f; // 检测半径
    Transform grabObject;
    Transform preParent;
    public void hold()
    {

        Debug.Log("hold开始执行");
        if (grabObject != null) return;
        // 获取当前物体周围的所有碰撞器
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (hitColliders.Length == 0) return;
        foreach (Collider collider in hitColliders)
        {
            Dragable flag = collider.GetComponent<Dragable>();
            if (flag == null)
            {
                continue;
            }
            else
            {
                preParent = collider.transform.parent;
                collider.transform.parent = transform;
                grabObject = collider.transform;
                Debug.Log("抓住");
                return;
            }
        }
        Debug.Log("没东西可抓");
    }
    public void release()
    {
        if(grabObject!=null&&preParent!=null)
            Debug.Log(grabObject.name + "|" + preParent.name);
        if (grabObject != null)
        {
            
            grabObject.parent = preParent;
            grabObject = null;
        }
    }

}
