using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SceneSerializer : MonoBehaviour
{

    public GameObject prefab;
    List<Vector3> position = new List<Vector3>();
    List<Quaternion> quaternions = new List<Quaternion>();
    List<Vector3> scale = new List<Vector3>();

    public void save()
    {
        position.Clear();
        quaternions.Clear();
        scale.Clear();
        GameObject spawner = GameObject.Find("Object Spawner");
        foreach(Transform child in spawner.transform)
        {
            position.Add(child.position);
            quaternions.Add(child.rotation);
            scale.Add(child.localScale);
        }
    }

    public void load()
    {
        GameObject spawner = GameObject.Find("Object Spawner");
        for(int i = 0; i < position.Count; i++)
        {
            prefab.transform.localScale = scale[i];
            Instantiate(prefab, position[i], quaternions[i], spawner.transform);
        }

        
    }


}
