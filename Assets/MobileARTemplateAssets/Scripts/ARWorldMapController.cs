using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using UnityEngine.UI;
using UnityEngine.XR.ARKit;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
public class ARWorldMapController : MonoBehaviour
{
    
    private ARSession mARSession;
    // Start is called before the first frame update
    void Start()
    {
        mARSession = GetComponent<ARSession>();
    }

    public void OnSaveButton()
    {
        Debug.Log("��ʼ����");
        StartCoroutine(Save());

    }

    public void OnLoadButton()
    {
        Debug.Log("��ʼ����");
        StartCoroutine(Load());

    }

    public void OnResetButton()
    {
        mARSession.Reset();
        Debug.Log("������");

    }
    IEnumerator Save()
    {
        var sessionSubsystem = (ARKitSessionSubsystem)mARSession.subsystem;
        if (sessionSubsystem == null)
        {
            Debug.Log("�豸��֧��");
            yield break;
        }
        var request = sessionSubsystem.GetARWorldMapAsync();
        while (!request.status.IsDone())
        {
            yield return null;
        }
        if (request.status.IsError())
        {
            Debug.Log(string.Format("Session���л����������룺{0}", request.status));
            yield break;
        }
        var worldMap = request.GetWorldMap();
        request.Dispose();
        SaveAndDisposeWorldMap(worldMap);
        Debug.Log("����ɹ�");
    }

    IEnumerator Load()
    {
        var sessionSubsystem = (ARKitSessionSubsystem)mARSession.subsystem;
        if (sessionSubsystem == null)
        {
            Debug.Log("�豸��֧��");
            yield break;
        }
        var file = File.Open(path, FileMode.Open);
        if (file == null)
        {
            Debug.Log(string.Format("Worldmap {0}�ļ�������", path));
            yield break;
        }
        int bytesPerFrame = 1024 * 10;
        var bytesRemaining = file.Length;
        var binaryReader = new BinaryReader(file);
        var allBytes = new List<byte>();
        while (bytesRemaining > 0)
        {
            var bytes = binaryReader.ReadBytes(bytesPerFrame);
            allBytes.AddRange(bytes);
            bytesRemaining -= bytesPerFrame;
            yield return null;
        }
        var data = new NativeArray<byte>(allBytes.Count, Allocator.Temp);
        data.CopyFrom(allBytes.ToArray());
        ARWorldMap worldMap;
        if (ARWorldMap.TryDeserialize(data, out worldMap))
        {
            data.Dispose();
        }
        if (!worldMap.valid)
        {
            Debug.Log("ARWorldMap��Ч");
            yield break;
        }
        sessionSubsystem.ApplyWorldMap(worldMap);
        Debug.Log("�������");

    }

    void SaveAndDisposeWorldMap(ARWorldMap worldMap)
    {
        var data = worldMap.Serialize(Allocator.Temp);
        var file = File.Open(path, FileMode.Create);
        var writer = new BinaryWriter(file);
        writer.Write(data.ToArray());
        writer.Close();
        data.Dispose();
        worldMap.Dispose();

    }

    string path
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "mySession.worldmap");
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
