using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveInFile : MonoBehaviour
{

    
    public GameObject Cube;
    public GameObject Chest;
    public GameObject Princess;
    public GameObject Dialog;
    public GameObject Goblin;

    private EventLinkContentManager eventLinkContentManager;
    private EventLink eventLink;

    private Save CreateSave()
    {
        Save save = new Save();

        eventLinkContentManager = FindObjectOfType<EventLinkContentManager>();
        eventLink = eventLinkContentManager.eventLink;

        save.eventLink = eventLink.eventCount;
        int count = 0;
        foreach(EventUnit eventUnit in eventLink.link)
        {
            save.eventType.Add(eventUnit.objectType);
            foreach(GameObject gameObject in eventUnit.objectList)
            {
                save.position.Add(new SerVector3(gameObject.transform.position));
                save.scale.Add(new SerVector3(gameObject.transform.localScale));
                save.rotation.Add(new SerQuaternion(gameObject.transform.rotation));
                save.name.Add(gameObject.name);
                save.eventCount.Add(count);           
                
                if(eventUnit.objectType == 0|| eventUnit.objectType == 1) 
                {
                    save.interactionType.Add(0);
                    save.animationType.Add(0);
                }else if(eventUnit.objectType == 2)
                {
                    save.interactionType.Add(0);//加交互类型
                    save.animationType.Add(0);
                }
                else if(eventUnit.objectType == 3)
                {
                    save.interactionType.Add(0);
                    save.animationType.Add(eventUnit.animationType);
                }
            }
            count++;
        }
        /*
        GameObject spawner = GameObject.Find("Object Spawner");
        foreach (Transform child in spawner.transform)
        {
            save.position.Add(new SerVector3(child.position));
            save.scale.Add(new SerVector3(child.localScale));
            save.rotation.Add(new SerQuaternion(child.rotation));
            save.name.Add(child.name);
        }
        */
        return save;
    }
    
    private void LoadSave(Save save)
    {
        GameObject spawner = GameObject.Find("Object Spawner");
        foreach (Transform child in spawner.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Clear");

        Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
        dictionary.Add("ChestVariant(Clone)", Chest);
        dictionary.Add("CubeVariant(Clone)", Cube);
        dictionary.Add("PrincessVariant(Clone)", Princess);
        dictionary.Add("GoblinVariant(Clone)", Goblin);
        dictionary.Add("DialogVariant(Clone)", Dialog);

        List<EventUnit> eventLink = new List<EventUnit>();

        int j = 0;
        for (int i = 0; i < save.eventLink;i++)
        {
            EventUnit eventUnit = new EventUnit();
            eventUnit.objectType = save.eventType[i];
            for(; j < save.eventCount.Count; j++)
            {
                if (save.eventCount[j] == i)
                {
                    GameObject prefab = dictionary[save.name[i]];
                    prefab.transform.localScale = save.scale[i].GetVector3();
                    GameObject gameObject = Instantiate(prefab, save.position[i].GetVector3(), save.rotation[i].GetQuaternion(), spawner.transform);
                    eventUnit.objectList.Add(gameObject);
                    if (save.interactionType[j] != 0)
                    {
                        //添加交互
                    }
                    if (save.animationType[j] != 0)
                    {
                        eventUnit.animationType = save.animationType[j];
                    }
                }
                else
                {
                    break;
                }
            }
            eventLink.Add(eventUnit);
        }

        EventLink link = new EventLink();
        link.link = eventLink;
        link.eventCount = save.eventLink;
        eventLinkContentManager = FindObjectOfType<EventLinkContentManager>();
        eventLinkContentManager.eventLink = link;


    }

    public void SaveGame()
    {
        Save save = CreateSave();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/gamesave.txt");
        binaryFormatter.Serialize(fileStream, save);
        fileStream.Close();
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Saved");

    }

    public void LoadGame()
    {

        //GameObject spawner = GameObject.Find("Object Spawner");
        if (File.Exists(Application.persistentDataPath + "/gamesave.txt"))
        {
            /*
            foreach (Transform child in spawner.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("Clear");
            */
        }
        else
        {
            Debug.Log("NoSave");
            return;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/gamesave.txt", FileMode.Open);

        Save save = (Save)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();

        LoadSave(save);
        /*
        Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
        dictionary.Add("ChestVariant(Clone)", Chest);
        dictionary.Add("CubeVariant(Clone)", Cube);
        dictionary.Add("PrincessVariant(Clone)", Princess);
        dictionary.Add("GoblinVariant(Clone)", Goblin);
        dictionary.Add("DialogVariant(Clone)", Dialog);
        


        for (int i = 0; i < save.position.Count; i++)
        {
            GameObject prefab = dictionary[save.name[i]];
            prefab.transform.localScale = save.scale[i].GetVector3();
            Instantiate(prefab, save.position[i].GetVector3(), save.rotation[i].GetQuaternion(), spawner.transform);
        }
        */
        Debug.Log("Load");
        
    }
}

