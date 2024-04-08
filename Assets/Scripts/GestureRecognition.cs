using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GestureRecognition : MonoBehaviour
{
    public string apiKey = "WceuaOmtBTLYDNg4lXaDuR0b";
    public string secretKey = "dvRVsbKVrqzAIPWBzEvxz3Ps0oUQOCoR";
    private string accessToken;

    TextMeshProUGUI resultText;
    Button uploadButton;

    public Image Image;

    void Start()
    {
        GameObject.Find("Result").SetActive(true);
        resultText = GameObject.Find("Result").GetComponent<TextMeshProUGUI>();
        if (resultText == null)
        {
            // ���δ�ҵ����������������Ϣ
            Debug.LogError("Result GameObject not found!");
        }
        GameObject.Find("Upload").SetActive(true);
        uploadButton = GameObject.Find("Upload").GetComponent<Button>();
        if (uploadButton == null)
        {
            // ���δ�ҵ����������������Ϣ
            Debug.LogError("uploadButton GameObject not found!");
        }
        StartCoroutine(GetAccessToken());
        uploadButton.onClick.AddListener(OnUploadButtonClicked);

#if UNITY_EDITOR
        // �ڱ༭��ģʽ��������׽��ʶ������
        CaptureAndRecognizeEditor();
#endif
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
                // ����UI��ʾʶ����
                resultText.text = "ʶ������" + www.downloadHandler.text;
            }
        }
    }

    void OnUploadButtonClicked()
    {
        string resourcePath = "Images/Ok";
        string base64Image = GetImageBase64FromResources(resourcePath);

        // ȷ��base64Image��Ϊnull
        if (!string.IsNullOrEmpty(base64Image))
        {
            // ��������ʶ���Coroutine
            StartCoroutine(GestureRecognize(base64Image));
        }
        else
        {
            Debug.LogError("Failed to get image base64 or image path is incorrect.");
        }
    }

    string GetImageBase64FromResources(string imagePath)
    {
        Texture2D texture = Resources.Load<Texture2D>(imagePath);
        if (texture != null)
        {
            // ������ص���ͼ
            Debug.Log($"Loaded texture: {imagePath}");
            // ���ͼƬ�Ĵ�С
            Debug.Log($"Image size: {texture.width} x {texture.height}");
            // ��Texture2D����ת��ΪBase64����
            byte[] bytes = texture.EncodeToJPG();
            string base64 = System.Convert.ToBase64String(bytes);
            return base64;
        }
        else
        {
            Debug.LogError($"Failed to load texture at {imagePath}");
            return null;
        }
    }

#if UNITY_EDITOR
    void CaptureAndRecognizeEditor()
    {
        // ÿ��2�벶׽����ͷ��ʶ������
        StartCoroutine(CaptureAndRecognizeRoutine());
    }

    IEnumerator CaptureAndRecognizeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // ��ȡ��ǰʱ����Ϊ��ͼ�ļ���
            string screenshotName = "screenshot_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            string screenshotPath = Path.Combine(Application.dataPath, "Resources", screenshotName);
            // ��׽��Ļ��ͼ
            ScreenCapture.CaptureScreenshot(screenshotPath);
            // �ȴ�1֡ȷ����ͼ���
            yield return null;

            byte[] fileData = System.IO.File.ReadAllBytes(screenshotPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // ����ͼƬ����
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            Image.GetComponent<Image>().sprite = sprite;

            // ��ȡ��ͼ��ת��ΪBase64����
            string base64Image = GetImageBase64FromFile(screenshotPath);
            // ����ɹ���ȡ��Base64���룬��ʶ������
            if (!string.IsNullOrEmpty(base64Image))
            {
                StartCoroutine(GestureRecognize(base64Image));
            }
            else
            {
                Debug.LogError("Failed to capture screenshot or convert to base64.");
            }
        }
    }

    string GetImageBase64FromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            string base64 = System.Convert.ToBase64String(bytes);
            return base64;
        }
        else
        {
            Debug.LogError($"File not found at {filePath}");
            return null;
        }
    }
#endif

    [System.Serializable]
    public class AccessTokenResponse
    {
        public string access_token;
        public string expires_in;
    }
}
