using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private float detectionRadius = 0.05f; // ���뾶
    Transform grabObject;
    public GameObject objectSpawner;
    Transform preParent;
    public void Start()
    {
        preParent = objectSpawner.transform;
    }
    public void hold()
    {

        Debug.Log("hold");
        if (grabObject != null) return;
        //
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (hitColliders.Length == 0 || hitColliders.Length == 1) return;
        foreach (Collider collider in hitColliders)
        {
            //过滤掉Sphere和ARPlane
            if (collider.transform.gameObject.name != "Visuals") continue;
            Debug.Log("collider.transform.gameObject: " + collider.transform.gameObject);
            Debug.Log("collider.transform.parent: " + collider.transform.parent);

            Dragable flag = collider.transform.parent.GetComponent<Dragable>();
            if (flag == null)
            {
                continue;
            }
            else
            {
                collider.transform.parent.parent = transform;
                grabObject = collider.transform.parent;
                
                //Debug.Log("preParent: " + collider.transform.parent.parent);
                Debug.Log("collider.transform: " + collider.transform.gameObject);
                Debug.Log("grabObject: " + grabObject.gameObject);
                
                return;
            }
        }
        Debug.Log("hold完成");
    }
    public void release()
    {
        if(grabObject !=null )
        {
            Debug.Log("释放物体");
            //Debug.Log(grabObject.name + "|" + preParent.name);
        }
            
        if (grabObject != null)
        {
            
            grabObject.parent = preParent;
            grabObject = null;
        }
    }

}
