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
            // 如果未找到对象，则输出错误消息
            Debug.LogError("Result GameObject not found!");
        }
        GameObject.Find("Upload").SetActive(true);
        uploadButton = GameObject.Find("Upload").GetComponent<Button>();
        if (uploadButton == null)
        {
            // 如果未找到对象，则输出错误消息
            Debug.LogError("uploadButton GameObject not found!");
        }
        StartCoroutine(GetAccessToken());
        uploadButton.onClick.AddListener(OnUploadButtonClicked);

#if UNITY_EDITOR
        // 在编辑器模式下启动捕捉和识别手势
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
                // 解析返回的JSON，提取手势识别结果
                Debug.Log(www.downloadHandler.text);
                // 更新UI显示识别结果
                resultText.text = "识别结果：" + www.downloadHandler.text;
            }
        }
    }

    void OnUploadButtonClicked()
    {
        string resourcePath = "Images/Ok";
        string base64Image = GetImageBase64FromResources(resourcePath);

        // 确保base64Image不为null
        if (!string.IsNullOrEmpty(base64Image))
        {
            // 调用手势识别的Coroutine
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
            // 输出加载的贴图
            Debug.Log($"Loaded texture: {imagePath}");
            // 输出图片的大小
            Debug.Log($"Image size: {texture.width} x {texture.height}");
            // 将Texture2D对象转换为Base64编码
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
        // 每隔2秒捕捉摄像头并识别手势
        StartCoroutine(CaptureAndRecognizeRoutine());
    }

    IEnumerator CaptureAndRecognizeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // 获取当前时间作为截图文件名
            string screenshotName = "screenshot_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            string screenshotPath = Path.Combine(Application.dataPath, "Resources", screenshotName);
            // 捕捉屏幕截图
            ScreenCapture.CaptureScreenshot(screenshotPath);
            // 等待1帧确保截图完成
            yield return null;

            byte[] fileData = System.IO.File.ReadAllBytes(screenshotPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); // 加载图片数据
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            Image.GetComponent<Image>().sprite = sprite;

            // 读取截图并转换为Base64编码
            string base64Image = GetImageBase64FromFile(screenshotPath);
            // 如果成功获取到Base64编码，则识别手势
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
