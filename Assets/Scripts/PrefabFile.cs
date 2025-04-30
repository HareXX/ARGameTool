using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        if (GameStatusManager.instance.isEditing)
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(true);
            }
            SceneManager.instance.gameObject.SetActive(true);
            SceneManager.instance.currentScene = index;
            EditPage.instance.GetComponent<Canvas>().enabled = false;

            GameObject[] Visuals = GameObject.FindGameObjectsWithTag("Arrow");
            foreach (GameObject visual in Visuals)
            {
                visual.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(true);
            }
            SceneManager.instance.gameObject.SetActive(true);
            SceneManager.instance.currentScene = index;
            EditPage.instance.GetComponent<Canvas>().enabled = false;
            foreach (GameObject go in EditPage.instance.ToHide)
            {
                go.SetActive(false);
            }

            GameObject[] Visuals = GameObject.FindGameObjectsWithTag("Arrow");
            foreach (GameObject visual in Visuals)
            {
                visual.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }

            EditPage.instance.PlaymodeManager.SetActive(true);
            PlaymodeManager.instance.PlayObjects = gameObjects;
            EventLinkContentManager.Instance.playEvent();   //播放事件



            //EditPage.instance.ButtonContinue.SetActive(true);
        }

    }
}
