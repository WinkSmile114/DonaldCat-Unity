using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

public class ServerResponse
{
    public string error;
}

public class RestAPIController : MonoBehaviour
{
    public static RestAPIController Instance = null;

    public string baseURL = "http://65.108.27.142:5000/api/";

    private void Awake()
    {
        Instance = this;
    }

    public class PostData
    {
        public string prompt;
    }

    // Start is called before the first frame update
    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendGetRequest(string uri)
    {
        string fullUri = baseURL + uri;

        StartCoroutine(GetRequest(fullUri));
    }

    public void SendPostRequest(string uri, string prompt)
    {
        // Initiate Post Data
        PostData postData = new PostData
        {
            prompt = prompt,
        };

        // Convert Post Data to JSON
        string jsonData = JsonUtility.ToJson(postData);

        string fullUri = baseURL + uri;
        StartCoroutine(PostRequest(fullUri, jsonData, OnPostRequestCompleted));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator PostRequest(string uri, string jsonData, System.Action<ServerResponse> callback)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                Debug.LogError("Error: " + webRequest.downloadHandler.text);
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(webRequest.downloadHandler.text);
                callback?.Invoke(response);
            }
            else
            {
                Debug.Log("Response: " + webRequest.downloadHandler.text);
                ServerResponse response = JsonUtility.FromJson<ServerResponse>(webRequest.downloadHandler.text);
                callback?.Invoke(response);
            }
        }
    }

    // Callback function to handle the response
    void OnPostRequestCompleted(ServerResponse response)
    {
        // Process the response here
        GameController.Instance.ResponseControl(response);
    }
}

