using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;

    public TypeWriter typeWriter;
    public string promptText;
    public string promptType;
    public Image vadDetector;
    

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPromptType(string promptType)
    {
        this.promptType = promptType;
    }

    public void SetPrompt(string prompt)
    {
        // Check if promptText is null
        if (prompt == null || prompt == "")
        {
            Debug.LogError("promptText is null or empty.");
            return;
        }

        this.promptText = prompt;
        Debug.Log("Type: " + this.promptType);
        Debug.Log("Prompt: " + this.promptText);

        this.PromptControl();
    }

    public void PromptControl()
    {
        Debug.Log("prompt in PromptControl: " + promptText);
        UIManager.Instance.HideModels();  // Hide the Models
        UIManager.Instance.SetInputField(promptText);

        UIManager.Instance.ShowChatBox(promptType);    // Active Chat Box
        UIManager.Instance.ShowResponse("");

        //  Go to the OpenAI and get response
        RestAPIController.Instance.SendPostRequest("openai/response", promptText);

        // Display the Question
        UIManager.Instance.ShowChatQuest(promptText);
        // Display the response from the OpenAI
    }
    

    public void VoiceInputControl(string actType)
    {
        if (!typeWriter.isTyping)
        {
            this.SetPromptType("voice");
            if (actType == "press")
            {
                this.vadDetector.gameObject.SetActive(true);
                MicrophoneDemo.Instance.OnButtonPressed();
                UIManager.Instance.SetPlaceholder("Keep pressing for record...");
                Debug.Log("Pressed");
            } else
            {
                this.vadDetector.gameObject.SetActive(false);
                MicrophoneDemo.Instance.OnButtonReleased();
                Debug.Log("Released");
            }
        }
    }

    public void TextInputControl(string inputText)
    {
        if (!typeWriter.isTyping)
        {
            this.SetPromptType("text");
            this.SetPrompt(inputText);
        }
    }

    public void ResponseControl(ServerResponse response)
    {
        this.AnimatControl("talk");
        UIManager.Instance.ShowResponse(response.error);
    }

    public void AnimatControl(string animType)
    {
        if (animType == "talk")
        {
            CatController.Instance.TalkAnimation();
        }
        else
        {
            CatController.Instance.IdleAnimation();
        }
    }


}
