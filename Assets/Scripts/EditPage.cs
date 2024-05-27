using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPage : MonoBehaviour
{
    public static EditPage instance;
    public GameObject Content;
    public GameObject PrefabFile;
    public List<GameObject> ToHide;
    public GameObject PlaymodeManager;
    public GameObject ButtonContinue;

    public GameObject VoiceInteractionCanvas;
    public GameObject GestureInteractionCanvas;

    private void Awake()
    {
        instance = this;
    }

    public void NavigateToEditPage()
    {
        for (int j = 0; j < Content.transform.childCount; j++)
        {
            Destroy(Content.transform.GetChild(j).gameObject);
        }
        int i = 1;
        foreach(List<GameObject> gameObjects in SceneManager.instance.ARGameObjectPackages)
        {
            GameObject file = Instantiate(PrefabFile);
            file.GetComponent<PrefabFile>().Objection.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            file.GetComponent<PrefabFile>().SelectionBox.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            file.transform.parent = Content.transform;
            file.GetComponent<PrefabFile>().textMeshProUGUI.text = "File" + i;
            file.GetComponent<PrefabFile>().index = i;
            i++;
            file.GetComponent<PrefabFile>().gameObjects = gameObjects;
        }
    }

}
