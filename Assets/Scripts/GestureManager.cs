using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    public GameObject[] toHide;
    public GameObject[] toShow;
    public void OnChangeUnitGestureInteractionIndex(int index)
    {
        EventLinkContentManager.Instance.eventLink.link[EventLinkContentManager.Instance.focusedEventIndex].gestureInteractionIndex = index;
        foreach (GameObject go in toHide) { go.SetActive(false); }
        foreach (GameObject go in toShow) {  go.SetActive(true); }
    }
}
