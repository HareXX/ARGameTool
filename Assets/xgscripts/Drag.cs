using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private float detectionRadius = 0.025f; // ���뾶
    Transform grabObject;
    Transform preParent;
    public void hold()
    {

        Debug.Log("hold��ʼִ��");
        if (grabObject != null) return;
        // ��ȡ��ǰ������Χ��������ײ��
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
                Debug.Log("ץס");
                return;
            }
        }
        Debug.Log("û������ץ");
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
