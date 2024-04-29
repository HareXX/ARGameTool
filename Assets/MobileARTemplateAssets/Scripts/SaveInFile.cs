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
    

    private Save CreateSave()
    {
        Save save = new Save();
        GameObject spawner = GameObject.Find("Object Spawner");
        foreach (Transform child in spawner.transform)
        {
            save.position.Add(new SerVector3(child.position));
            save.scale.Add(new SerVector3(child.localScale));
            save.rotation.Add(new SerQuaternion(child.rotation));
            save.name.Add(child.name);
        }
        return save;
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

        GameObject spawner = GameObject.Find("Object Spawner");
        if (File.Exists(Application.persistentDataPath + "/gamesave.txt"))
        {
            foreach (Transform child in spawner.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("Clear");
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
        Debug.Log("Load");
    }
}

