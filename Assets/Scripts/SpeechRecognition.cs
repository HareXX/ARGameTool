////using System.Collections;
////using System.Collections.Generic;
////using System.Text.RegularExpressions;
////using UnityEngine;
////using UnityEngine.UI;
////using UnityEngine.Networking;
////using System;
////using TMPro;

////public class SpeechScript : MonoBehaviour
////{
////    public static SpeechScript Instance;
////    public float cosResult;
////    public GameObject[] voiceToHide;

////    private void Awake()
////    {
////        Instance = this;
////    }
////    public string api_key = "ZO7xgi0yOSQ9BEEQXWDPSK3s";
////    public string secret_Key = "sGKpDhjkcBRrrr3hYLsCXt2TZk7HNsXB";
////    string accessToken = string.Empty;

////    public string inputText = "֥�鿪��";

////    //bool ishaveMic = false; //����Ƿ�������˷�
////    string currentDeviceName = string.Empty; //��ǰ¼���豸����(Ĭ��)
////    int recordFrequency = 8000; //¼��Ƶ��
////    int recordMaxTime = 20;//���¼��ʱ��
////    AudioClip saveAudioClip;//�洢��ǰ¼����Ƭ��
////    AudioSource source;

////    string resulrStr;//�洢ʶ����
////    TextMeshProUGUI resultText;//��ʾʶ����
////    Button startBtn;//��ʼʶ��ť
////    Button endBtn;//����ʶ��ť

////    void Start()
////    {
////        saveAudioClip = transform.GetComponent<AudioClip>();
////        source = transform.GetComponent<AudioSource>();
////        GameObject.Find("Result").SetActive(true);
////        resultText = GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
////        if (resultText == null)
////        {
////            // ���δ�ҵ����������������Ϣ
////            Debug.LogError("Result GameObject not found!");
////        }

////        GameObject.Find("Start").SetActive(true);
////        startBtn = GameObject.Find("Start").GetComponent<Button>();
////        GameObject.Find("End").SetActive(true);
////        endBtn = GameObject.Find("End").GetComponent<Button>();

////        StartCoroutine(_GetAccessToken());//��ȡaccessToken

////        startBtn.onClick.AddListener(StartRecord);
////        endBtn.onClick.AddListener(EndRecord);

////        // ������п��õ�¼���豸����
////        string[] devices = Microphone.devices;

////        foreach (string device in devices)
////        {
////            Debug.Log("Available Microphone Device: " + device);
////        }

////        if (source == null)
////        {
////            Debug.LogError("AudioSource component not found!");
////        }
////        else
////        {
////            Debug.Log("AudioSource component found!");
////        }
////    }


////    /// <summary>
////    /// ��ʼ¼��
////    /// </summary>
////    void StartRecord()
////    {
////        string[] devices = Microphone.devices;
////        if (devices.Length > 0)
////        {
////            string currentDeviceName = devices[0];
////            Debug.Log("Available Microphone Device: " + currentDeviceName);
////        }
////        else
////        {
////            Debug.LogError("No microphone devices available!");
////        }
////        saveAudioClip = Microphone.Start(currentDeviceName, false, recordMaxTime, recordFrequency);
////        Debug.Log("��ʼִ��");
////    }

////    /// <summary>
////    /// ����¼��
////    /// </summary>
////    void EndRecord()
////    {
////        Microphone.End(currentDeviceName);
////        if (saveAudioClip != null)
////        {
////            source.PlayOneShot(saveAudioClip);
////            StartCoroutine(DelayedRequestASR());
////        }
////        else
////        {
////            Debug.LogError("saveAudioClip is null.");
////        }

////        Debug.Log("����ִ��");
////    }

////    IEnumerator DelayedRequestASR()
////    {
////        // �ȴ�һ֡
////        yield return null;

////        StartCoroutine(RequestASR());
////    }

////    /// <summary>
////    /// ��������ʶ��
////    /// </summary>
////    /// <returns></returns>
////    IEnumerator RequestASR()
////    {
////        if (string.IsNullOrEmpty(accessToken))
////        {
////            yield return _GetAccessToken();
////        }
////        //resulrStr = string.Empty;
////        resulrStr = "Wating....";
////        //������ǰ¼������ΪPCM16
////        float[] samples = new float[recordFrequency * 10 * saveAudioClip.channels];
////        saveAudioClip.GetData(samples, 0);
////        var samplesShort = new short[samples.Length];
////        for (var index = 0; index < samples.Length; index++)
////        {
////            samplesShort[index] = (short)(samples[index] * short.MaxValue);
////        }
////        byte[] datas = new byte[samplesShort.Length * 2];
////        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

////        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

////        WWWForm wwwForm = new WWWForm();
////        wwwForm.AddBinaryData("audio", datas);

////        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

////        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

////        yield return unityWebRequest.SendWebRequest();

////        if (string.IsNullOrEmpty(unityWebRequest.error))
////        {
////            resulrStr = unityWebRequest.downloadHandler.text;
////            if (Regex.IsMatch(resulrStr, @"err_msg.:.success"))
////            {
////                Match match = Regex.Match(resulrStr, "result.:..(.*?)..]");
////                if (match.Success)
////                {
////                    resulrStr = match.Groups[1].ToString();
////                    Debug.Log(resulrStr);
////                    // ����TF-IDFֵ
////                    //float tfIdfValue = CalculateTFIDF(resulrStr, inputText);

////                    // �����������ƶ�
////                    float cosineSimilarity = CalculateCosineSimilarity(resulrStr, inputText);

////                    // ������ƶ�
////                    //Debug.Log("TF-IDFֵ��" + tfIdfValue);
////                    Debug.Log("�������ƶȣ�" + cosineSimilarity);

////                    // ����UI��ʾ
////                    resulrStr = resulrStr + "�������ƶȣ�" + cosineSimilarity;
////                    if (cosineSimilarity == 1)
////                    {
////                        resulrStr = resulrStr + "ƥ��ɹ���";
////                    }
////                    Debug.Log(resulrStr);
////                }
////            }
////            else
////            {
////                resulrStr = "ʶ����Ϊ��";
////            }
////            resultText.text = resulrStr;
////        }
////    }

////    /// <summary>
////    /// ��ȡaccessToken��������
////    /// </summary>
////    /// <returns></returns>
////    IEnumerator _GetAccessToken()
////    {
////        var uri =
////            string.Format(
////                "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}",
////                api_key, secret_Key);
////        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
////        yield return unityWebRequest.SendWebRequest();
////        if (unityWebRequest.isDone)
////        {
////            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
////            if (match.Success)
////            {
////                Debug.Log("Token�Ѿ�ƥ��");
////                accessToken = match.Groups[1].ToString();
////            }
////            else
////            {
////                Debug.Log("��֤����,��ȡAccessTokenʧ��!!!");
////            }
////        }
////    }

////    /*
////    // ����TFֵ
////    float CalculateTF(string[] words, string word)
////    {
////        int count = 0;
////        foreach (string w in words)
////        {
////            if (w.Equals(word, StringComparison.OrdinalIgnoreCase))
////            {
////                count++;
////            }
////        }
////        return (float)count / words.Length;
////    }

////    // ����IDFֵ
////    float CalculateIDF(List<string[]> documents, string word)
////    {
////        int count = 0;
////        foreach (string[] doc in documents)
////        {
////            foreach (string w in doc)
////            {
////                if (w.Equals(word, StringComparison.OrdinalIgnoreCase))
////                {
////                    count++;
////                    break;
////                }
////            }
////        }
////        return Mathf.Log(documents.Count / (float)(count + 1));
////    }

////    // ����TF-IDFֵ
////    float CalculateTFIDF(string document, string inputText)
////    {
////        // ������ʶ�������û����������ת��Ϊ���б�
////        string[] docWords = document.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
////        string[] inputWords = inputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

////        List<string[]> documents = new List<string[]> { docWords, inputWords };

////        // ����TF-IDFֵ
////        float tfidfSum = 0f;
////        foreach (string word in inputWords)
////        {
////            float tf = CalculateTF(docWords, word);
////            float idf = CalculateIDF(documents, word);
////            float tfidf = tf * idf;
////            tfidfSum += tfidf;
////        }
////        return tfidfSum;
////    }
////    */

////    // �����������ƶ�
////    float CalculateCosineSimilarity(string text1, string text2)
////    {
////        string[] words1 = text1.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
////        string[] words2 = text2.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

////        Dictionary<string, int> vector1 = new Dictionary<string, int>();
////        Dictionary<string, int> vector2 = new Dictionary<string, int>();

////        // ͳ�ƴ�Ƶ
////        foreach (string word in words1)
////        {
////            if (vector1.ContainsKey(word))
////            {
////                vector1[word]++;
////            }
////            else
////            {
////                vector1[word] = 1;
////            }
////        }

////        foreach (string word in words2)
////        {
////            if (vector2.ContainsKey(word))
////            {
////                vector2[word]++;
////            }
////            else
////            {
////                vector2[word] = 1;
////            }
////        }

////        // �����������ƶ�
////        double dotProduct = 0.0;
////        double magnitude1 = 0.0;
////        double magnitude2 = 0.0;

////        foreach (var kvp in vector1)
////        {
////            string word = kvp.Key;
////            int count1 = kvp.Value;
////            magnitude1 += Math.Pow(count1, 2);
////            if (vector2.ContainsKey(word))
////            {
////                dotProduct += count1 * vector2[word];
////            }
////        }

////        foreach (var kvp in vector2)
////        {
////            int count2 = kvp.Value;
////            magnitude2 += Math.Pow(count2, 2);
////        }

////        magnitude1 = Math.Sqrt(magnitude1);
////        magnitude2 = Math.Sqrt(magnitude2);

////        if (magnitude1 == 0 || magnitude2 == 0)
////        {
////            return 0;
////        }

////        cosResult = (float)(dotProduct / (magnitude1 * magnitude2));
////        if (cosResult < 0.3)
////        {
////            EditPage.instance.VoiceInteractionCanvas.SetActive(false);
////            EventLinkContentManager.Instance.nextEvent();
////        }
////        //EventLinkContentManager.Instance.eventLink.link[EventLinkContentManager.Instance.focusedEventIndex].cosResult = cosResult;
////        return cosResult;
////    }

////}

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

//    private void Awake()
//    {
//        Instance = this;
//    }

//    public string api_key = "ZO7xgi0yOSQ9BEEQXWDPSK3s";
//    public string secret_Key = "sGKpDhjkcBRrrr3hYLsCXt2TZk7HNsXB";
//    string accessToken = string.Empty;
//    public float cosResult;

//    public string inputText;

//    //bool ishaveMic = false; //����Ƿ�������˷�
//    string currentDeviceName = string.Empty; //��ǰ¼���豸����(Ĭ��)
//    int recordFrequency = 8000; //¼��Ƶ��
//    int recordMaxTime = 20;//���¼��ʱ��
//    AudioClip saveAudioClip;//�洢��ǰ¼����Ƭ��
//    AudioSource source;

//    string resulrStr;//�洢ʶ����
//    TextMeshProUGUI resultText;//��ʾʶ����
//    Button startBtn;//��ʼʶ��ť
//    Button endBtn;//����ʶ��ť


//    void Start()
//    {
//        saveAudioClip = transform.GetComponent<AudioClip>();
//        source = transform.GetComponent<AudioSource>();
//        GameObject.Find("Result").SetActive(true);
//        resultText = GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
//        if (resultText == null)
//        {
//            // ���δ�ҵ����������������Ϣ
//            Debug.LogError("Result GameObject not found!");
//        }

//        GameObject.Find("Start").SetActive(true);
//        startBtn = GameObject.Find("Start").GetComponent<Button>();
//        GameObject.Find("End").SetActive(true);
//        endBtn = GameObject.Find("End").GetComponent<Button>();

//        StartCoroutine(_GetAccessToken());//��ȡaccessToken

//        startBtn.onClick.AddListener(StartRecord);
//        endBtn.onClick.AddListener(EndRecord);

//        // ������п��õ�¼���豸����
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
//    /// ��ʼ¼��
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
//        Debug.Log("��ʼִ��");
//    }

//    /// <summary>
//    /// ����¼��
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

//        Debug.Log("����ִ��");
//    }

//    IEnumerator DelayedRequestASR()
//    {
//        // �ȴ�һ֡
//        yield return null;

//        StartCoroutine(RequestASR());
//    }

//    /// <summary>
//    /// ��������ʶ��
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
//        //������ǰ¼������ΪPCM16
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
//                    // ����TF-IDFֵ
//                    //float tfIdfValue = CalculateTFIDF(resulrStr, inputText);

//                    // �����������ƶ�
//                    float cosineSimilarity = CalculateCosineSimilarity(resulrStr, inputText);

//                    // ������ƶ�
//                    //Debug.Log("TF-IDFֵ��" + tfIdfValue);
//                    Debug.Log("�������ƶȣ�" + cosineSimilarity);

//                    // ����UI��ʾ
//                    resulrStr = resulrStr + "\n�������ƶȣ�" + cosineSimilarity;
//                    if (cosineSimilarity >= 0.5)
//                    {
//                        resulrStr = resulrStr + "\nƥ��ɹ���";
//                    }
//                    Debug.Log(resulrStr);
//                }
//            }
//            else
//            {
//                resulrStr = "ʶ����Ϊ��";
//            }
//            resultText.text = resulrStr;
//        }
//    }

//    /// <summary>
//    /// ��ȡaccessToken��������
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
//                Debug.Log("Token�Ѿ�ƥ��");
//                accessToken = match.Groups[1].ToString();
//            }
//            else
//            {
//                Debug.Log("��֤����,��ȡAccessTokenʧ��!!!");
//            }
//        }
//    }

//    /*
//    // ����TFֵ
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

//    // ����IDFֵ
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

//    // ����TF-IDFֵ
//    float CalculateTFIDF(string document, string inputText)
//    {
//        // ������ʶ�������û����������ת��Ϊ���б�
//        string[] docWords = document.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
//        string[] inputWords = inputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

//        List<string[]> documents = new List<string[]> { docWords, inputWords };

//        // ����TF-IDFֵ
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

//    // �����������ƶ�
//    public float CalculateCosineSimilarity(string text1, string text2)
//    {
//        var words1 = text1.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
//        var words2 = text2.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

//        var vector1 = new Dictionary<string, int>();
//        var vector2 = new Dictionary<string, int>();

//        // ͳ�ƴ�Ƶ
//        foreach (var word in words1)
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

//        foreach (var word in words2)
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

//        // �����������ƶ�
//        double dotProduct = 0.0;
//        double magnitude1 = 0.0;
//        double magnitude2 = 0.0;

//        var allWords = new HashSet<string>(vector1.Keys);
//        allWords.UnionWith(vector2.Keys);

//        foreach (var word in allWords)
//        {
//            int count1 = vector1.ContainsKey(word) ? vector1[word] : 0;
//            int count2 = vector2.ContainsKey(word) ? vector2[word] : 0;

//            dotProduct += count1 * count2;
//            magnitude1 += Math.Pow(count1, 2);
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

    public string inputText = "������������";

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
        //������ǰ¼������ΪPCM16
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
            Debug.Log("1111111111111111111111111111");
            Debug.Log(inputText);
            Debug.Log(resulrStr);

            resulrStr = unityWebRequest.downloadHandler.text;
            if (Regex.IsMatch(resulrStr, @"err_msg.:.success"))
            {
                Match match = Regex.Match(resulrStr, "result.:..(.*?)..]");
                if (match.Success)
                {
                    resulrStr = match.Groups[1].ToString();
                    Debug.Log(resulrStr);
                    // ����TF-IDFֵ
                    //float tfIdfValue = CalculateTFIDF(resulrStr, inputText);

                    // �����������ƶ�
                    float cosineSimilarity = CalculateCosineSimilarity(resulrStr, inputText);

                    // ������ƶ�
                    //Debug.Log("TF-IDFֵ��" + tfIdfValue);
                    Debug.Log("cos" + cosineSimilarity);

                    // ����UI��ʾ
                    //resulrStr = resulrStr + "\n余弦相似度" + cosineSimilarity;
                    Debug.Log(inputText + "****");
                   
                    Debug.Log(resulrStr + "****");
                    if (string.Equals(resulrStr, "关闭"))
                    //if (cosineSimilarity >= 0.5)
                    {
                        EventLinkContentManager.Instance.nextEvent();
                        resulrStr = "口令正确!";
                    }
                    else
                    {
                        Debug.Log("1111111111111111111111111111");
                        Debug.Log(inputText);
                        Debug.Log(resulrStr);
                        resulrStr = "好像不太对呢~";
                    }
                    Debug.Log(resulrStr);
                }
            }
            else
            {
                resulrStr = "null";
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

    /*
    // ����TFֵ
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

    // ����IDFֵ
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

    // ����TF-IDFֵ
    float CalculateTFIDF(string document, string inputText)
    {
        // ������ʶ�������û����������ת��Ϊ���б�
        string[] docWords = document.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        string[] inputWords = inputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        List<string[]> documents = new List<string[]> { docWords, inputWords };

        // ����TF-IDFֵ
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

    // �����������ƶ�
    public static float CalculateCosineSimilarity(string text1, string text2)
    {
       
        var words1 = text1.Split(new char[] { ' ', ',', '.', '!', '?', '的', '是' }, StringSplitOptions.RemoveEmptyEntries);
        var words2 = text2.Split(new char[] { ' ', ',', '.', '!', '?', '的', '是' }, StringSplitOptions.RemoveEmptyEntries);

        var vector1 = new Dictionary<string, int>();
        var vector2 = new Dictionary<string, int>();

        // ͳ�ƴ�Ƶ
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

        // Debug �����Ƶ����
        Debug.Log("Vector1: " + string.Join(", ", vector1));
        Debug.Log("Vector2: " + string.Join(", ", vector2));

        // �����������ƶ�
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

        // Debug �����������е�ֵ
        Debug.Log("Dot Product: " + dotProduct);
        Debug.Log("Magnitude1: " + magnitude1);
        Debug.Log("Magnitude2: " + magnitude2);

        if (magnitude1 == 0 || magnitude2 == 0)
        {
            return 0;
        }

        
        return (float)(dotProduct / (magnitude1 * magnitude2));
    }

}


