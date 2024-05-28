//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Networking;
//using System;
//using TMPro;

//public class SpeechScript : MonoBehaviour
//{
//    public static SpeechScript Instance;
//    public float cosResult;
//    public GameObject[] voiceToHide;

//    private void Awake()
//    {
//        Instance = this;
//    }
//    public string api_key = "ZO7xgi0yOSQ9BEEQXWDPSK3s";
//    public string secret_Key = "sGKpDhjkcBRrrr3hYLsCXt2TZk7HNsXB";
//    string accessToken = string.Empty;

//    public string inputText = "芝麻开门";

//    //bool ishaveMic = false; //检测是否连接麦克风
//    string currentDeviceName = string.Empty; //当前录音设备名称(默认)
//    int recordFrequency = 8000; //录音频率
//    int recordMaxTime = 20;//最大录音时长
//    AudioClip saveAudioClip;//存储当前录音的片段
//    AudioSource source;

//    string resulrStr;//存储识别结果
//    TextMeshProUGUI resultText;//显示识别结果
//    Button startBtn;//开始识别按钮
//    Button endBtn;//结束识别按钮

//    void Start()
//    {
//        saveAudioClip = transform.GetComponent<AudioClip>();
//        source = transform.GetComponent<AudioSource>();
//        GameObject.Find("Result").SetActive(true);
//        resultText = GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
//        if (resultText == null)
//        {
//            // 如果未找到对象，则输出错误消息
//            Debug.LogError("Result GameObject not found!");
//        }

//        GameObject.Find("Start").SetActive(true);
//        startBtn = GameObject.Find("Start").GetComponent<Button>();
//        GameObject.Find("End").SetActive(true);
//        endBtn = GameObject.Find("End").GetComponent<Button>();

//        StartCoroutine(_GetAccessToken());//获取accessToken

//        startBtn.onClick.AddListener(StartRecord);
//        endBtn.onClick.AddListener(EndRecord);

//        // 输出所有可用的录音设备名称
//        string[] devices = Microphone.devices;

//        foreach (string device in devices)
//        {
//            Debug.Log("Available Microphone Device: " + device);
//        }

//        if (source == null)
//        {
//            Debug.LogError("AudioSource component not found!");
//        }
//        else
//        {
//            Debug.Log("AudioSource component found!");
//        }
//    }


//    /// <summary>
//    /// 开始录音
//    /// </summary>
//    void StartRecord()
//    {
//        string[] devices = Microphone.devices;
//        if (devices.Length > 0)
//        {
//            string currentDeviceName = devices[0];
//            Debug.Log("Available Microphone Device: " + currentDeviceName);
//        }
//        else
//        {
//            Debug.LogError("No microphone devices available!");
//        }
//        saveAudioClip = Microphone.Start(currentDeviceName, false, recordMaxTime, recordFrequency);
//        Debug.Log("开始执行");
//    }

//    /// <summary>
//    /// 结束录音
//    /// </summary>
//    void EndRecord()
//    {
//        Microphone.End(currentDeviceName);
//        if (saveAudioClip != null)
//        {
//            source.PlayOneShot(saveAudioClip);
//            StartCoroutine(DelayedRequestASR());
//        }
//        else
//        {
//            Debug.LogError("saveAudioClip is null.");
//        }

//        Debug.Log("结束执行");
//    }

//    IEnumerator DelayedRequestASR()
//    {
//        // 等待一帧
//        yield return null;

//        StartCoroutine(RequestASR());
//    }

//    /// <summary>
//    /// 请求语音识别
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator RequestASR()
//    {
//        if (string.IsNullOrEmpty(accessToken))
//        {
//            yield return _GetAccessToken();
//        }
//        //resulrStr = string.Empty;
//        resulrStr = "Wating....";
//        //处理当前录音数据为PCM16
//        float[] samples = new float[recordFrequency * 10 * saveAudioClip.channels];
//        saveAudioClip.GetData(samples, 0);
//        var samplesShort = new short[samples.Length];
//        for (var index = 0; index < samples.Length; index++)
//        {
//            samplesShort[index] = (short)(samples[index] * short.MaxValue);
//        }
//        byte[] datas = new byte[samplesShort.Length * 2];
//        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

//        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

//        WWWForm wwwForm = new WWWForm();
//        wwwForm.AddBinaryData("audio", datas);

//        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

//        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

//        yield return unityWebRequest.SendWebRequest();

//        if (string.IsNullOrEmpty(unityWebRequest.error))
//        {
//            resulrStr = unityWebRequest.downloadHandler.text;
//            if (Regex.IsMatch(resulrStr, @"err_msg.:.success"))
//            {
//                Match match = Regex.Match(resulrStr, "result.:..(.*?)..]");
//                if (match.Success)
//                {
//                    resulrStr = match.Groups[1].ToString();
//                    Debug.Log(resulrStr);
//                    // 计算TF-IDF值
//                    //float tfIdfValue = CalculateTFIDF(resulrStr, inputText);

//                    // 计算余弦相似度
//                    float cosineSimilarity = CalculateCosineSimilarity(resulrStr, inputText);

//                    // 输出相似度
//                    //Debug.Log("TF-IDF值：" + tfIdfValue);
//                    Debug.Log("余弦相似度：" + cosineSimilarity);

//                    // 更新UI显示
//                    resulrStr = resulrStr + "余弦相似度：" + cosineSimilarity;
//                    if (cosineSimilarity == 1)
//                    {
//                        resulrStr = resulrStr + "匹配成功！";
//                    }
//                    Debug.Log(resulrStr);
//                }
//            }
//            else
//            {
//                resulrStr = "识别结果为空";
//            }
//            resultText.text = resulrStr;
//        }
//    }

//    /// <summary>
//    /// 获取accessToken请求令牌
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator _GetAccessToken()
//    {
//        var uri =
//            string.Format(
//                "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}",
//                api_key, secret_Key);
//        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
//        yield return unityWebRequest.SendWebRequest();
//        if (unityWebRequest.isDone)
//        {
//            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
//            if (match.Success)
//            {
//                Debug.Log("Token已经匹配");
//                accessToken = match.Groups[1].ToString();
//            }
//            else
//            {
//                Debug.Log("验证错误,获取AccessToken失败!!!");
//            }
//        }
//    }

//    /*
//    // 计算TF值
//    float CalculateTF(string[] words, string word)
//    {
//        int count = 0;
//        foreach (string w in words)
//        {
//            if (w.Equals(word, StringComparison.OrdinalIgnoreCase))
//            {
//                count++;
//            }
//        }
//        return (float)count / words.Length;
//    }

//    // 计算IDF值
//    float CalculateIDF(List<string[]> documents, string word)
//    {
//        int count = 0;
//        foreach (string[] doc in documents)
//        {
//            foreach (string w in doc)
//            {
//                if (w.Equals(word, StringComparison.OrdinalIgnoreCase))
//                {
//                    count++;
//                    break;
//                }
//            }
//        }
//        return Mathf.Log(documents.Count / (float)(count + 1));
//    }

//    // 计算TF-IDF值
//    float CalculateTFIDF(string document, string inputText)
//    {
//        // 将语音识别结果和用户输入的文字转换为词列表
//        string[] docWords = document.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
//        string[] inputWords = inputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

//        List<string[]> documents = new List<string[]> { docWords, inputWords };

//        // 计算TF-IDF值
//        float tfidfSum = 0f;
//        foreach (string word in inputWords)
//        {
//            float tf = CalculateTF(docWords, word);
//            float idf = CalculateIDF(documents, word);
//            float tfidf = tf * idf;
//            tfidfSum += tfidf;
//        }
//        return tfidfSum;
//    }
//    */

//    // 计算余弦相似度
//    float CalculateCosineSimilarity(string text1, string text2)
//    {
//        string[] words1 = text1.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
//        string[] words2 = text2.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

//        Dictionary<string, int> vector1 = new Dictionary<string, int>();
//        Dictionary<string, int> vector2 = new Dictionary<string, int>();

//        // 统计词频
//        foreach (string word in words1)
//        {
//            if (vector1.ContainsKey(word))
//            {
//                vector1[word]++;
//            }
//            else
//            {
//                vector1[word] = 1;
//            }
//        }

//        foreach (string word in words2)
//        {
//            if (vector2.ContainsKey(word))
//            {
//                vector2[word]++;
//            }
//            else
//            {
//                vector2[word] = 1;
//            }
//        }

//        // 计算余弦相似度
//        double dotProduct = 0.0;
//        double magnitude1 = 0.0;
//        double magnitude2 = 0.0;

//        foreach (var kvp in vector1)
//        {
//            string word = kvp.Key;
//            int count1 = kvp.Value;
//            magnitude1 += Math.Pow(count1, 2);
//            if (vector2.ContainsKey(word))
//            {
//                dotProduct += count1 * vector2[word];
//            }
//        }

//        foreach (var kvp in vector2)
//        {
//            int count2 = kvp.Value;
//            magnitude2 += Math.Pow(count2, 2);
//        }

//        magnitude1 = Math.Sqrt(magnitude1);
//        magnitude2 = Math.Sqrt(magnitude2);

//        if (magnitude1 == 0 || magnitude2 == 0)
//        {
//            return 0;
//        }

//        cosResult = (float)(dotProduct / (magnitude1 * magnitude2));
//        if (cosResult < 0.3)
//        {
//            EditPage.instance.VoiceInteractionCanvas.SetActive(false);
//            EventLinkContentManager.Instance.nextEvent();
//        }
//        //EventLinkContentManager.Instance.eventLink.link[EventLinkContentManager.Instance.focusedEventIndex].cosResult = cosResult;
//        return cosResult;
//    }

//}

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
    public static SpeechScript Instance;

    private void Awake()
    {
        Instance = this;
    }

    public string api_key = "ZO7xgi0yOSQ9BEEQXWDPSK3s";
    public string secret_Key = "sGKpDhjkcBRrrr3hYLsCXt2TZk7HNsXB";
    string accessToken = string.Empty;
    public float cosResult;

    public string inputText;

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
                    // 计算TF-IDF值
                    //float tfIdfValue = CalculateTFIDF(resulrStr, inputText);

                    // 计算余弦相似度
                    float cosineSimilarity = CalculateCosineSimilarity(resulrStr, inputText);

                    // 输出相似度
                    //Debug.Log("TF-IDF值：" + tfIdfValue);
                    Debug.Log("余弦相似度：" + cosineSimilarity);

                    // 更新UI显示
                    resulrStr = resulrStr + "\n余弦相似度：" + cosineSimilarity;
                    if (cosineSimilarity >= 0.5)
                    {
                        resulrStr = resulrStr + "\n匹配成功！";
                    }
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

    /*
    // 计算TF值
    float CalculateTF(string[] words, string word)
    {
        int count = 0;
        foreach (string w in words)
        {
            if (w.Equals(word, StringComparison.OrdinalIgnoreCase))
            {
                count++;
            }
        }
        return (float)count / words.Length;
    }

    // 计算IDF值
    float CalculateIDF(List<string[]> documents, string word)
    {
        int count = 0;
        foreach (string[] doc in documents)
        {
            foreach (string w in doc)
            {
                if (w.Equals(word, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                    break;
                }
            }
        }
        return Mathf.Log(documents.Count / (float)(count + 1));
    }

    // 计算TF-IDF值
    float CalculateTFIDF(string document, string inputText)
    {
        // 将语音识别结果和用户输入的文字转换为词列表
        string[] docWords = document.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        string[] inputWords = inputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        List<string[]> documents = new List<string[]> { docWords, inputWords };

        // 计算TF-IDF值
        float tfidfSum = 0f;
        foreach (string word in inputWords)
        {
            float tf = CalculateTF(docWords, word);
            float idf = CalculateIDF(documents, word);
            float tfidf = tf * idf;
            tfidfSum += tfidf;
        }
        return tfidfSum;
    }
    */

    // 计算余弦相似度
    public float CalculateCosineSimilarity(string text1, string text2)
    {
        var words1 = text1.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var words2 = text2.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        var vector1 = new Dictionary<string, int>();
        var vector2 = new Dictionary<string, int>();

        // 统计词频
        foreach (var word in words1)
        {
            if (vector1.ContainsKey(word))
            {
                vector1[word]++;
            }
            else
            {
                vector1[word] = 1;
            }
        }

        foreach (var word in words2)
        {
            if (vector2.ContainsKey(word))
            {
                vector2[word]++;
            }
            else
            {
                vector2[word] = 1;
            }
        }

        // 计算余弦相似度
        double dotProduct = 0.0;
        double magnitude1 = 0.0;
        double magnitude2 = 0.0;

        var allWords = new HashSet<string>(vector1.Keys);
        allWords.UnionWith(vector2.Keys);

        foreach (var word in allWords)
        {
            int count1 = vector1.ContainsKey(word) ? vector1[word] : 0;
            int count2 = vector2.ContainsKey(word) ? vector2[word] : 0;

            dotProduct += count1 * count2;
            magnitude1 += Math.Pow(count1, 2);
            magnitude2 += Math.Pow(count2, 2);
        }

        magnitude1 = Math.Sqrt(magnitude1);
        magnitude2 = Math.Sqrt(magnitude2);

        if (magnitude1 == 0 || magnitude2 == 0)
        {
            return 0;
        }

        cosResult = (float)(dotProduct / (magnitude1 * magnitude2));
        if (cosResult < 0.3)
        {
            EditPage.instance.VoiceInteractionCanvas.SetActive(false);
            EventLinkContentManager.Instance.nextEvent();
        }

        return cosResult;
    }

}

