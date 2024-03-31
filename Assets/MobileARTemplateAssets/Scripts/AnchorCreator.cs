using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class AnchorCreator : MonoBehaviour
{
    [SerializeField]
    GameObject m_Prefab;
    public GameObject prefab
    {
        get => m_Prefab;
        set => m_Prefab = value;
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_Anchors = new List<ARAnchor>();

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
    }

    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        ARAnchor anchor = null;
        if (hit.trackable is ARPlane plane)
        {
            var planeManager = GetComponent<ARPlaneManager>();
            if (planeManager)
            {
                Debug.Log("Creating anchor attachment");
                var oldPrefab = m_AnchorManager.anchorPrefab;
                m_AnchorManager.anchorPrefab = prefab;
                anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                m_AnchorManager.anchorPrefab = oldPrefab;
                return anchor;
            }
        }
        Debug.Log("Creating regular anchor");
        var gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
        anchor = gameObject.GetComponent<ARAnchor>();
        if(anchor == null)
        {
            anchor = gameObject.AddComponent<ARAnchor>();
        }
        return anchor;
    }

    public void RemoveAllAnchors()
    {
        foreach(var anchor in m_Anchors)
        {
            Destroy(anchor.gameObject);
        }
        m_Anchors.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        var touch = Input.GetTouch(0);
        if(touch.phase != TouchPhase.Began)
        {
            return;
        }
        const TrackableType trackableTypes = TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon;
        if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
        {
            var hit = s_Hits[0];
            var anchor = CreateAnchor(hit);
            if (anchor)
            {
                m_Anchors.Add(anchor);
            }
            else
            {
                Debug.Log("Error creating anchor");
            }
        }
    }
}
