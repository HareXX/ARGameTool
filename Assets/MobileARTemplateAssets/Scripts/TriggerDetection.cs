using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventLinkContentManager.Instance == null) return;
        if (EventLinkContentManager.Instance.isPlaying && EventLinkContentManager.Instance.eventCount > 0 &&
            EventLinkContentManager.Instance.focusedEventIndex < EventLinkContentManager.Instance.eventCount)
        {
            
            EventUnit eventUnit = EventLinkContentManager.Instance.eventLink.link[EventLinkContentManager.Instance.focusedEventIndex];

            if (eventUnit.triggerDetection == false) return;
            Debug.Log("true!!!!!!!!!!!!!!!");

            Collider[] hitColliders = Physics.OverlapSphere(eventUnit.triggerObject.transform.position, 0.05f);
            foreach (Collider collider in hitColliders)
            {
                //过滤掉Sphere和ARPlane
                Debug.Log("collider: " + collider);
                if (collider.transform.gameObject.name != "Visuals") continue;

                Debug.Log("object: " + eventUnit.objectList[0]);
                if (collider.transform.parent == null) continue;
                Debug.Log("collider.transform.parent.gameObject: " + collider.transform.parent.gameObject);
                if (eventUnit.objectList[0] == collider.transform.parent.gameObject)
                {
                    Debug.Log("接触成功了");
                    eventUnit.triggerDetection = false;
                    EventLinkContentManager.Instance.nextEvent();
                }


            }
        }
    }
}
