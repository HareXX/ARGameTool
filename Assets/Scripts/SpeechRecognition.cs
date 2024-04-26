using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;

public class SpeechScript : MonoBehaviour
{

    public string api_key = "ZO7xgi0yOSQ9BEEQXWDPSK3s";
    public string secret_Key = "sGKpDhjkcBRrrr3hYLsCXt2TZk7HNsXB";
    string accessToken = string.Empty;

    //bool ishaveMic = false; //����Ƿ�������˷�
    string currentDeviceName = string.Empty; //��ǰ¼���豸����(Ĭ��)
    int recordFrequency = 8000; //¼��Ƶ��
    int recordMaxTime = 20;//���¼��ʱ��
    AudioClip saveAudioClip;//�洢��ǰ¼����Ƭ��
    AudioSource source;

    string resulrStr;//�洢ʶ����
    TextMeshProUGUI resultText;//��ʾʶ����
    Button startBtn;//��ʼʶ��ť
    Button endBtn;//����ʶ��ť

    void Start()
    {
        saveAudioClip = transform.GetComponent<AudioClip>();
        source = transform.GetComponent<AudioSource>();
        GameObject.Find("Result").SetActive(true);
        resultText = GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
        if (resultText == null)
        {
            // ���δ�ҵ����������������Ϣ
            Debug.LogError("Result GameObject not found!");
        }

        GameObject.Find("Start").SetActive(true);
        startBtn = GameObject.Find("Start").GetComponent<Button>();
        GameObject.Find("End").SetActive(true);
        endBtn = GameObject.Find("End").GetComponent<Button>();

        StartCoroutine(_GetAccessToken());//��ȡaccessToken

        startBtn.onClick.AddListener(StartRecord);
        endBtn.onClick.AddListener(EndRecord);

        // ������п��õ�¼���豸����
        string[] devices = Microphone.devices;
        foreach (string device in devices)
        {
            Debug.Log("Available Microphone Device: " + device);
        }

        if (source == null)
        {
            Debug.LogError("AudioSource component not found!");
        }
        else
        {
            Debug.Log("AudioSource component found!");
        }
    }


    /// <summary>
    /// ��ʼ¼��
    /// </summary>
    void StartRecord()
    {
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            string currentDeviceName = devices[0];
            Debug.Log("Available Microphone Device: " + currentDeviceName);
        }
        else
        {
            Debug.LogError("No microphone devices available!");
        }
        saveAudioClip = Microphone.Start(currentDeviceName, false, recordMaxTime, recordFrequency);
        Debug.Log("��ʼִ��");
    }

    /// <summary>
    /// ����¼��
    /// </summary>
    void EndRecord()
    {
        Microphone.End(currentDeviceName);
        if (saveAudioClip != null)
        {
            source.PlayOneShot(saveAudioClip);
            StartCoroutine(DelayedRequestASR());
        }
        else
        {
            Debug.LogError("saveAudioClip is null.");
        }

        Debug.Log("����ִ��");
    }

    IEnumerator DelayedRequestASR()
    {
        // �ȴ�һ֡
        yield return null;

        StartCoroutine(RequestASR());
    }

    /// <summary>
    /// ��������ʶ��
    /// </summary>
    /// <returns></returns>
    IEnumerator RequestASR()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return _GetAccessToken();
        }
        //resulrStr = string.Empty;
        resulrStr = "Wating....";
        //����ǰ¼������ΪPCM16
        float[] samples = new float[recordFrequency * 10 * saveAudioClip.channels];
        saveAudioClip.GetData(samples, 0);
        var samplesShort = new short[samples.Length];
        for (var index = 0; index < samples.Length; index++)
        {
            samplesShort[index] = (short)(samples[index] * short.MaxValue);
        }
        byte[] datas = new byte[samplesShort.Length * 2];
        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("audio", datas);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

        yield return unityWebRequest.SendWebRequest();

        if (string.IsNullOrEmpty(unityWebRequest.error))
        {
            resulrStr = unityWebRequest.downloadHandler.text;
            if (Regex.IsMatch(resulrStr, @"err_msg.:.success"))
            {
                Match match = Regex.Match(resulrStr, "result.:..(.*?)..]");
                if (match.Success)
                {
                    resulrStr = match.Groups[1].ToString();
                    Debug.Log(resulrStr);
                }
            }
            else
            {
                resulrStr = "ʶ����Ϊ��";
            }
            resultText.text = resulrStr;
        }
    }

    /// <summary>
    /// ��ȡaccessToken��������
    /// </summary>
    /// <returns></returns>
    IEnumerator _GetAccessToken()
    {
        var uri =
            string.Format(
                "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}",
                api_key, secret_Key);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
            if (match.Success)
            {
                Debug.Log("Token�Ѿ�ƥ��");
                accessToken = match.Groups[1].ToString();
            }
            else
            {
                Debug.Log("��֤����,��ȡAccessTokenʧ��!!!");
            }
        }
    }
}

