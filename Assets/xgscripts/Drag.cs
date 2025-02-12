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

        Debug.Log("hold");
        if (grabObject != null) return;
        //
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
                Debug.Log("抓到物体了");
                Debug.Log(gameObject.gameObject);
                return;
            }
        }
        Debug.Log("成功抓住物体");
    }
    public void release()
    {
        if(grabObject!=null && preParent!=null)
        {
            Debug.Log("释放物体");
            Debug.Log(grabObject.name + "|" + preParent.name);
        }
            
        if (grabObject != null)
        {
            
            grabObject.parent = preParent;
            grabObject = null;
        }
    }

}
