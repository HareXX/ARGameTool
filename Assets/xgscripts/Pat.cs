using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pat : MonoBehaviour
{
    private float detectionRadius = 0.05f; // 
    Transform patObject;
    //public GameObject objectSpawner;
    Transform preParent;
    public void Start()
    {

    }
    public void pat()
    {

        //Debug.Log("pat");
        if (patObject != null) return;
        //
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (hitColliders.Length < 2) return;
        foreach (Collider collider in hitColliders)
        {
            //过滤掉Sphere和ARPlane
            Debug.Log(collider.transform.gameObject);
            if (collider.transform.gameObject.tag != "Arrow") continue;
            Debug.Log("collider.transform.gameObject: " + collider.transform.gameObject);
            Debug.Log("collider.transform.parent: " + collider.transform.parent.gameObject);
            Debug.Log("摸到了");
            collider.transform.parent.gameObject.SetActive(false);
            EventLinkContentManager.Instance.nextEvent();
            return;

        }
        //Debug.Log("point完成");
    }
    public void release()
    {
        if (patObject != null)
        {
            Debug.Log("释放物体");
            //Debug.Log(grabObject.name + "|" + preParent.name);
        }

        if (patObject != null)
        {
            patObject = null;
        }
    }
}
