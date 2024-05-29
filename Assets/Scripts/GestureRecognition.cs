using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System.IO;


public class GestureRecognition : MonoBehaviour
{
    public static GestureRecognition Instance;

    private void Awake()
    {
        Instance = this;
    }
    public string apiKey = "WceuaOmtBTLYDNg4lXaDuR0b";
    public string secretKey = "dvRVsbKVrqzAIPWBzEvxz3Ps0oUQOCoR";
    private string accessToken;
    private WebCamTexture webCamTexture;

    // ��ǰ����������ʶ��Ϊ���壬������չ����Ϊ������ʶ��Ϊ�գ�����������ʱ��ת�����й���ͨ�õ�ͼ��ʶ����ֲ�����Ϊ��Ҫ�������󣬽�ʶ����������������ģ�ͣ����м���������

    // �����ֵ�
    public Dictionary<string, string> recognizedGestures = new Dictionary<string, string>
    {
        { "1", "One" },
        { "2", "Five" },
        { "3", "Fist" },
        { "4", "Ok" },
        { "5", "Prayer" },
        { "6", "Congratuation" },
        { "7", "Honour" },
        { "8", "Heart_single" },
        { "9", "Thumb_up" },
        { "10", "Thumb_down" },
        { "11", "ILY" },
        { "12", "Palm_up" },
        { "13", "Heart_1" },
        { "14", "Heart_2" },
        { "15", "Heart_3" },
        { "16", "Two" },
        { "17", "Three" },
        { "18", "Four" },
        { "19", "Six" },
        { "20", "Seven" },
        { "21", "Eight" },
        { "22", "Nine" },
        { "23", "Rock" },
        { "24", "Insult" }
    };

    // ��������Э�̵�����
    private Coroutine captureCoroutine;

    /*
    // ����¼�
    public Button EditButton;
    public Button FinishButton;
    */

    // ����ͷʵʱ���
    public Image Image;

    // ���ֿ�����
    public MusicPlayer musicPlayer;

    // �洢ʶ�𵽵���������
    private List<string> gestureSequence = new List<string>();

    // Ŀ��������������������
    private string targetOpenSequence;
    // Ŀ�������������ر�����
    private string targetStopSequence;
    // Ŀ�������������л�����
    private string targetNextSongSequence;
    // Ŀ������������󳤶�
    private int sequenceMaxLength = 10;
    // ���������б�(���������Ƶ��趨�������û�������UI����༭ʱ�����û�ѡ������ӵ�������) 
    private List<string> startGestures = new List<string>();

    /*
    // Ŀǰʵ�����ı����룬֮�󽫵���Ϊѡ��ť����ͼƬ
    public TMP_InputField openMusicInput;
    public TMP_InputField stopMusicInput;
    public TMP_InputField nextSongInput;
    */
    // �����ֲ�ģ�͵�Transform���
    public Transform handModelTransform;

    // ��ʱ��������
    public Transform TestModleTransform;

    // ������һ����ײ����������
    private GameObject lastCollidedObject;

    void Start()
    {
        StartCoroutine(GetAccessToken());

        // ��ʼ��WebCamTexture����ʼ����
        webCamTexture = new WebCamTexture();
        // ������ͷ���������ΪUI Image������
        Image.material.mainTexture = webCamTexture;
        webCamTexture.Play();

        StartGestureRecognition();
        /*
        // �󶨰�ť����¼�
        EditButton.onClick.AddListener(StartGestureRecognition);
        FinishButton.onClick.AddListener(StopRecognitionCoroutine);
        */
    }

    IEnumerator GetAccessToken()
    {
        string authUrl = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}";
        using (UnityWebRequest www = UnityWebRequest.Get(authUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                accessToken = JsonUtility.FromJson<AccessTokenResponse>(www.downloadHandler.text).access_token;
            }
        }
    }

    IEnumerator GestureRecognize(string base64Image)
    {
        string gestureUrl = "https://aip.baidubce.com/rest/2.0/image-classify/v1/gesture";
        string urlWithAccessToken = $"{gestureUrl}?access_token={accessToken}";

        WWWForm form = new WWWForm();
        form.AddField("image", base64Image);

        using (UnityWebRequest www = UnityWebRequest.Post(urlWithAccessToken, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // �������ص�JSON����ȡ����ʶ����
                Debug.Log(www.downloadHandler.text);
                // ����API���ذ������Ʊ�ŵ�JSON����
                GestureResponse response = JsonUtility.FromJson<GestureResponse>(www.downloadHandler.text);
                if (response.result != null && response.result.Length > 0)
                {
                    string gestureName = response.result[0].classname;
                    ProcessGesture(gestureName);
                    GestureResult hand = response.result[0];
                    UpdateVirtualHand(hand);
                }
                else
                {
                    Debug.Log("No gesture result found in response.");
                }
            }
        }
    }

    // ������������ǲ����������������Ұ����
    public bool IsCameraPointingAtObject(GameObject obj)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(obj.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen && (screenPoint - new Vector3(0.5f, 0.5f, 0)).magnitude < 0.1;
    }


    void UpdateVirtualHand(GestureResult hand)
    {
        // ���������ֲ���λ�ú���ת
        float screenX = hand.left + hand.width / 2f;
        float screenY = hand.top + hand.height / 2f;

        // ת��Ϊ��������
        // 10f��z���������ͷ�ľ���
        Vector3 worldPos = new Vector3(screenX / Screen.width, 1f - screenY / Screen.height, 10f);
        Vector3 handPosition = Camera.main.ViewportToWorldPoint(worldPos);

        // ���������ֲ���λ��
        handModelTransform.position = handPosition;

        // ��ȡ�ֵ���������
        Vector3 handWorldPosition = handModelTransform.position;

        Vector3 Testposition = TestModleTransform.position;

        // ������������λ����Ϣ
        Debug.Log("Virtual Object Name: " + TestModleTransform.name + ", Position: " + Testposition);

        // ����һ�������������ڼ����ײ
        float interactionRange = 150f; // ��ײ��ⷶΧ

        // ��ȡλ�����������ڵ�������ײ��
        Collider[] hitColliders = Physics.OverlapSphere(handWorldPosition, interactionRange);

        // ������⵽����ײ���������������������ײ
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("VirtualObject"))
            {
                // �������һ����ײ������������ͬ�������²�����Ч
                if (lastCollidedObject != hitCollider.gameObject)
                {
                    // ִ�н�������
                    hitCollider.GetComponent<Renderer>().material.color = Color.red;
                    Debug.Log("Successful Touch!!!");

                    // �������⵽��������������ƺ�λ����Ϣ
                    Debug.Log("Detected Virtual Object: " + hitCollider.gameObject.name);
                    Debug.Log("Virtual Object Position: " + hitCollider.transform.position);

                    // ������ײ��Ч
                    musicPlayer.PlayCollisionSound();

                    // ����ǰ��ײ�����������¼Ϊ��һ����ײ������
                    lastCollidedObject = hitCollider.gameObject;
                }
            }
        }

        // ���ݰٶ�AI���ص��������������ֲ�����ת
        // ...

        Debug.Log($"Updated virtual hand position to {handPosition}");
    }

    void CaptureAndRecognizeCamera()
    {
        if (captureCoroutine == null)
        {
            captureCoroutine = StartCoroutine(CaptureAndRecognizeRoutine());
            Debug.Log("Recognition Coroutine Started.");
        }
    }

    void StopRecognitionCoroutine()
    {
        if (captureCoroutine != null)
        {
            StopCoroutine(captureCoroutine); // ʹ�ñ����������ֹͣЭ��
            captureCoroutine = null; // �������
            Debug.Log("Recognition Coroutine Stopped.");
        }
    }

    [System.Serializable]
    public class GestureResponse
    {
        public GestureResult[] result;
    }

    [System.Serializable]
    public class GestureResult
    {
        public string classname;
        public int top;
        public int left;
        public float probability;
        public int width;
        public int height;
    }



    // ��������Ƿ�����ض��������
    bool CheckGestureSequence(string targetSequence)
    {
        string currentSequence = string.Join("-", gestureSequence);
        return currentSequence.Contains(targetSequence);
    }

    void ProcessGestureSequence(List<string> gestureSequence)
    {
        AudioSource musicPlayerAudioSource = musicPlayer.GetAudioSource();

        string sequence = string.Join("-", gestureSequence);

        // ����Ƿ�ʶ����Ŀ�꿪������
        if (CheckGestureSequence(targetOpenSequence))
        {
            // ���ʶ�����У��򲥷�����
            if (musicPlayerAudioSource != null && !musicPlayerAudioSource.isPlaying)
            {
                Debug.Log("Open Match!");
                musicPlayer.PlaySong();
            }
            gestureSequence.Clear(); // ������У������ظ�����
        }

        // ����Ƿ�ʶ����Ŀ��ر�����
        if (CheckGestureSequence(targetStopSequence))
        {
            // ���ʶ�����У���ֹͣ��������
            if (musicPlayerAudioSource != null && musicPlayerAudioSource.isPlaying)
            {
                Debug.Log("Close Match!");
                musicPlayer.StopSong();
            }
            gestureSequence.Clear(); // ������У������ظ��ر�
        }

        // ��������Ƿ�����л��������������
        if (CheckGestureSequence(targetNextSongSequence))
        {
            // �������ƥ��
            if (musicPlayerAudioSource != null && musicPlayerAudioSource.isPlaying)
            {
                Debug.Log("Switching song!");
                musicPlayer.NextSong();
            }
            gestureSequence.Clear(); // �������
        }
    }

    void ProcessGesture(string gestureName)
    {
        // �ȼ�������Ƿ�Ϊ���������б��е�һ��
        if (startGestures.Contains(gestureName))
        {
            // ��յ�ǰ����
            gestureSequence.Clear();
            // ����µ��������Ƶ�������
            gestureSequence.Add(gestureName);
            // ��ӡ���������е���Ϣ
            Debug.Log("Started new gesture sequence with: " + gestureName);
        }
        else if (gestureName != null && gestureName != "Face")
        {
            // �������������������Ч����ӵ�������
            gestureSequence.Add(gestureName);
            // ������г����Ƿ�ﵽ���ޣ�����ﵽ��Ҳ������¿�ʼ
            if (gestureSequence.Count >= sequenceMaxLength)
            {
                Debug.Log("Gesture sequence reached max length. Starting new sequence.");
                gestureSequence.Clear();
                gestureSequence.Add(gestureName);
            }
        }

        // ��ӡ��ǰ��������
        Debug.Log("Current gesture sequence: " + string.Join("-", gestureSequence));

        ProcessGestureSequence(gestureSequence);
    }

    void StartGestureRecognition()
    {
        // ����Ŀ����������
        targetOpenSequence = "Six-Five";
        targetStopSequence = "Ok-Two";
        targetNextSongSequence = "Seven-Four";

        Debug.Log("targetOpenSequence: " + targetOpenSequence);
        Debug.Log("targetStopSequence: " + targetStopSequence);
        Debug.Log("targetNextSongSequence: " + targetNextSongSequence);

        // ��¼��������
        startGestures.Add(targetOpenSequence.Split("-")[0]);
        startGestures.Add(targetStopSequence.Split("-")[0]);
        startGestures.Add(targetNextSongSequence.Split("-")[0]);

        Debug.Log("Gesture recognition started.");
        CaptureAndRecognizeCamera(); // ��ʼ�����ʶ������
    }

    // �������������������ⲿ����
    public void BeginGestureRecognition()
    {
        StartGestureRecognition();
    }

    IEnumerator CaptureAndRecognizeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            Texture2D capturedTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
            capturedTexture.SetPixels(webCamTexture.GetPixels());
            capturedTexture.Apply();

            byte[] bytes = capturedTexture.EncodeToJPG();
            string base64Image = System.Convert.ToBase64String(bytes);

            // ����ɹ���ȡ��Base64���룬��ʶ������
            if (!string.IsNullOrEmpty(base64Image))
            {
                StartCoroutine(GestureRecognize(base64Image));
            }
            else
            {
                Debug.LogError("Failed to capture image from camera.");
            }

            // �ͷ�Texture2D���󣬱����ڴ�й¶
            Destroy(capturedTexture);
        }
    }

    [System.Serializable]
    public class AccessTokenResponse
    {
        public string access_token;
        public string expires_in;
    }

}
