using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public ObjectSpawner ObjectSpawner;
    public Transform Trackables;
    public GameObject XROrigin;
    public List<List<GameObject>> ARGameObjectPackages = new List<List<GameObject>>();
    private void Awake()
    {
        instance = this;
    }

    public void RemoveGameObjects()
    {
        List<GameObject> ARGameObjectPackage = new List<GameObject>();

        for(int i=0; i < ObjectSpawner.transform.childCount; i++)
        {
            if (ObjectSpawner.transform.GetChild(i).gameObject.activeSelf)
            {
                ARGameObjectPackage.Add(ObjectSpawner.transform.GetChild(i).gameObject);
            }
            ObjectSpawner.transform.GetChild(i).gameObject.SetActive(false);
        }
        ARGameObjectPackages.Add(ARGameObjectPackage);

        int j = 1;
        foreach (var item in ARGameObjectPackages)
        {
            Debug.Log(j);
            j++;
            foreach (var ite in item)
            {
                Debug.Log(ite.transform.name);
            }
            Debug.Log("--------------------------------"    );
        }
    }
}
