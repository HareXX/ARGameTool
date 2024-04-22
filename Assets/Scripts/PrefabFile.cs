using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrefabFile : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI textMeshProUGUI;
    public List<GameObject> gameObjects = new List<GameObject>();
    public GameObject Objection;
    public GameObject SelectionBox;

    public void OnClick()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(true);
        }
        SceneManager.instance.gameObject.SetActive(true);
        SceneManager.instance.currentScene = index;
        EditPage.instance.GetComponent<Canvas>().enabled = false;
    }
}
