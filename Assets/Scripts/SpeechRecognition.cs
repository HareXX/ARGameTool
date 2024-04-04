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

    //bool ishaveMic = false; //检测是否连接麦克风
    string currentDeviceName = string.Empty; //当前录音设备名称(默认)
    int recordFrequency = 8000; //录音频率
    int recordMaxTime = 20;//最大录音时长
    AudioClip saveAudioClip;//存储当前录音的片段
    AudioSource source;

    string resulrStr;//存储识别结果
    TextMeshProUGUI resultText;//显示识别结果
    Button startBtn;//开始识别按钮
    Button endBtn;//结束识别按钮

    void Start()
    {
        saveAudioClip = transform.GetComponent<AudioClip>();
        source = transform.GetComponent<AudioSource>();
        GameObject.Find("Result").SetActive(true);
        resultText = GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
        if (resultText == null)
        {
            // 如果未找到对象，则输出错误消息
            Debug.LogError("Result GameObject not found!");
        }

        GameObject.Find("Start").SetActive(true);
        startBtn = GameObject.Find("Start").GetComponent<Button>();
        GameObject.Find("End").SetActive(true);
        endBtn = GameObject.Find("End").GetComponent<Button>();

        StartCoroutine(_GetAccessToken());//获取accessToken

        startBtn.onClick.AddListener(StartRecord);
        endBtn.onClick.AddListener(EndRecord);

        // 输出所有可用的录音设备名称
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
    /// 开始录音
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
        Debug.Log("开始执行");
    }

    /// <summary>
    /// 结束录音
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

        Debug.Log("结束执行");
    }

    IEnumerator DelayedRequestASR()
    {
        // 等待一帧
        yield return null;

        StartCoroutine(RequestASR());
    }

    /// <summary>
    /// 请求语音识别
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
        //处理当前录音数据为PCM16
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
                resulrStr = "识别结果为空";
            }
            resultText.text = resulrStr;
        }
    }

    /// <summary>
    /// 获取accessToken请求令牌
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
                Debug.Log("Token已经匹配");
                accessToken = match.Groups[1].ToString();
            }
            else
            {
                Debug.Log("验证错误,获取AccessToken失败!!!");
            }
        }
    }
}

