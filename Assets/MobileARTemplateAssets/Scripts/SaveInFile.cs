using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting.FullSerializer;
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

        GameObject spawner = GameObject.Find("Object Spawner");
        GameObject camera = GameObject.Find("Main Camera");
        save.sPosition = new SerVector3(spawner.transform.position - camera.transform.position);
        save.sRotation = new SerQuaternion(Quaternion.Inverse(camera.transform.rotation)*spawner.transform.rotation);

        eventLinkContentManager = FindObjectOfType<EventLinkContentManager>();
        eventLink = eventLinkContentManager.eventLink;

        save.eventLink = eventLink.eventCount+1;
        int count = 0;
        foreach(EventUnit eventUnit in eventLink.link)
        {
            save.eventType.Add(eventUnit.objectType);
            foreach(GameObject gameObject in eventUnit.objectList)
            {
                int ID = gameObject.GetHashCode();
                if (save.objectID.Contains(ID))
                {
                    save.eventCount[save.objectID.IndexOf(ID)].Add(count);
                    continue;
                }
                save.objectID.Add(ID);
                save.position.Add(new SerVector3(gameObject.transform.position));
                save.scale.Add(new SerVector3(gameObject.transform.localScale));
                save.rotation.Add(new SerQuaternion(gameObject.transform.rotation));
                save.name.Add(gameObject.name);
                List<int> goEventCount =new List<int>();
                goEventCount.Add(count);
                save.eventCount.Add(goEventCount);

                
            }

                if (eventUnit.objectType == 0 || eventUnit.objectType == 1)
                {
                    save.interactionType.Add(0);
                    save.animationType.Add(0);
                }
                else if (eventUnit.objectType == 2)
                {
                    save.interactionType.Add(0);//加交互类型
                    save.animationType.Add(0);
                }
                else if (eventUnit.objectType == 3)
                {
                    save.interactionType.Add(0);
                    save.animationType.Add(eventUnit.animationType);
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
        GameObject camera = GameObject.Find("Main Camera");

        spawner.transform.position = camera.transform.position + save.sPosition.GetVector3();
        spawner.transform.rotation = camera.transform.rotation * save.sRotation.GetQuaternion();

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
        List<GameObject> eventButtonList = new List<GameObject>();
        eventLinkContentManager = FindObjectOfType<EventLinkContentManager>();
        eventLinkContentManager.eventButtonList = eventButtonList;

        for (int i = 0; i < save.eventLink;i++)
        {
            EventUnit eventUnit = new EventUnit();
            eventUnit.objectType = save.eventType[i];
            eventLinkContentManager.newEvent(save.eventType[i]);
            if (save.interactionType[i] != 0)
            {
                        //添加交互
            }
            if (save.animationType[i] != 0)
            {
                eventUnit.animationType = save.animationType[i];
            }
            eventLink.Add(eventUnit);
        }

        Debug.Log(save.eventLink);
        Debug.Log(eventLink.Count);

        for(int j = 0; j < save.eventCount.Count; j++)
        {
            GameObject prefab = dictionary[save.name[j]];
            prefab.transform.localScale = save.scale[j].GetVector3();
            GameObject gameObject = Instantiate(prefab, save.position[j].GetVector3(), save.rotation[j].GetQuaternion(), spawner.transform);
            
            for(int k = 0; k < save.eventCount[j].Count; k++) 
            {
                Debug.Log(save.eventCount[j][k]);
                eventLink[save.eventCount[j][k]].objectList.Add(gameObject);

            }
                          
        }

        EventLink link = new EventLink();
        link.link = eventLink;
        link.eventCount = save.eventLink;
        eventLinkContentManager = FindObjectOfType<EventLinkContentManager>();
        eventLinkContentManager.eventLink = link;


    }

    public void SaveGame(string fileName = "000")
    {   
        BinaryFormatter binaryFormatter = new BinaryFormatter();      

        gameFiles games = new gameFiles();
        if (File.Exists(Application.persistentDataPath + "/games.save")) {
            FileStream fs = File.Open(Application.persistentDataPath + "/games.save", FileMode.Open);
            games = (gameFiles)binaryFormatter.Deserialize(fs);
            if (games.games.Contains(fileName))
            {
                Debug.Log("File Exists");
                return;
            }
            fs.Close();
        }
        
        FileStream fs1 = File.Create(Application.persistentDataPath + "/games.save");
        games.games.Add(fileName);
        binaryFormatter.Serialize(fs1, games);
        fs1.Close();
        
        Save save = CreateSave();
        
        FileStream fileStream = File.Create(Application.persistentDataPath + "/" + fileName + ".save");
        binaryFormatter.Serialize(fileStream, save);
        fileStream.Close();
        Debug.Log(Application.persistentDataPath);
        Debug.Log("Saved");

    }

    public void LoadGame(string fileName = "000")
    {
        if(!File.Exists(Application.persistentDataPath + "/games.save"))
        {
            Debug.Log("NoSave");
            return;
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/games.save", FileMode.Open);
        gameFiles games = (gameFiles)binaryFormatter.Deserialize(fs);
        if (!games.games.Contains(fileName))
        {
            Debug.Log("NoSave");
            return;
        }
        fs.Close();
        //GameObject spawner = GameObject.Find("Object Spawner");
        /*if (File.Exists(Application.persistentDataPath + "/" + fileName + ".save"))
        {
            *//*
            foreach (Transform child in spawner.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("Clear");
            *//*
        }
        else
        {
            Debug.Log("NoSave");
            return;
        }*/

        
        FileStream fileStream = File.Open(Application.persistentDataPath + "/" + fileName + ".save", FileMode.Open);

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

    public void DeleteGame(string fileName = "000")
    {
        if (!File.Exists(Application.persistentDataPath + "/games.save"))
        {
            Debug.Log("NoSave");
            return;
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/games.save", FileMode.Open);
        gameFiles games = (gameFiles)binaryFormatter.Deserialize(fs);
        if (!games.games.Contains(fileName))
        {
            Debug.Log("NoSave");
            return;
        }
        fs.Close();
        FileStream fs1 = File.Create(Application.persistentDataPath + "/games.save");
        games.games.Remove(fileName);
        binaryFormatter.Serialize(fs1, games);
        fs1.Close();

        File.Delete(Application.persistentDataPath + "/" + fileName + ".save");
    }
}



[System.Serializable]
public class gameFiles
{
   public List<string> games = new List<string>();
}