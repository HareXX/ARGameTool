using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private float detectionRadius = 0.05f; // 
    Transform pointObject;
    //public GameObject objectSpawner;
    Transform preParent;
    public void Start()
    {
        
    }
    public void point()
    {

        //Debug.Log("point");s
        if (pointObject != null) return;
        //
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (hitColliders.Length < 2) return;
        foreach (Collider collider in hitColliders)
        {
            //过滤掉Sphere和ARPlane
            if (collider.transform.gameObject.tag != "numKey") continue;
            Debug.Log("collider.transform.gameObject: " + collider.transform.gameObject);
            Debug.Log("collider.transform.parent: " + collider.transform.parent.gameObject);

            GameObject key = collider.transform.gameObject;
            key.GetComponent<NumKey>().Invoke("press",0);


            return;
            
        }
        //Debug.Log("point完成");
    }
    public void release()
    {
        if (pointObject != null)
        {
            Debug.Log("释放物体");
            //Debug.Log(grabObject.name + "|" + preParent.name);
        }

        if (pointObject != null)
        {
            pointObject = null;
        }
    }
}
