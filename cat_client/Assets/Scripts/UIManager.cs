using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    public GameObject Models;
    public TMP_InputField inputField;
    public ChatBox chatBox;
    public Button recordButton;

    void Awake()
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

    //public void VoiceQuest()
    //{
    //    GameController.Instance.VoiceInputControl();
    //}

    public void TextQuest()
    {
        GameController.Instance.TextInputControl(inputField.text);
    }

    public void HideModels()
    {
        Models.SetActive(false);
    }

    public void ShowChatBox(string promptType)
    {
       chatBox.Init(promptType);
    }

    public void ShowChatQuest(string promptText)
    {
        chatBox.SetTextTile(promptText);
    }

    public void ShowResponse(string responseOpenAI)
    {
        //Debug.Log(responseOpenAI);
        chatBox.SetFullTextToType(responseOpenAI);

        chatBox.ShowResponse();
    }

    public void SetInputField(string promptText)
    {
        this.inputField.text = promptText;
    }

    public void SetPlaceholder(string placeholderText)
    {
        TMP_Text placeholder = inputField.placeholder as TMP_Text;

        if (placeholder != null)
        {
            // Change the placeholder text
            placeholder.text = placeholderText;
        }
        else
        {
            Debug.LogError("Placeholder TextMeshProUGUI component not found.");
        }
    }

    public void OnCatTouch()
    {
        CatController.Instance.TouchAnimation();
    }
}