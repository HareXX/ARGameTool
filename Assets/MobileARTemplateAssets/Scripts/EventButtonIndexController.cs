using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButtonIndexController : MonoBehaviour
{

    public int eventIndex = -1;

    public void Click()
    {
        transform.parent.GetComponent<EventLinkContentManager>().SendMessage("Click", eventIndex);
    }

    public void setIndex(int index)
    {
        eventIndex = index;
    }

    // Start is called before the first frame update
    void Start()
    {
        //去掉第一个作为template的Index
        eventIndex = transform.GetSiblingIndex() - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
