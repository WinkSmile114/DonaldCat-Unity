using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    public static ChatBox Instance = null;
    public GameObject scrollView;
    public TextMeshProUGUI question;
    public GameObject voiceTitle;

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

    public void Init(string promptType)
    {
        this.SetActive(true);
        this.ActiveQuestion(promptType);
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void ActiveQuestion(string promptType)
    {
        if (promptType == "text")
        {
            voiceTitle.SetActive(false);
            question.gameObject.SetActive(true);
        } else {
            question.gameObject.SetActive(false);
            voiceTitle.SetActive(true);
        }
    }

    public void SetTextTile(string quest)
    {
        question.text = quest;
    }

    public void ShowResponse()
    {
        TypeWriter.Instance.StopTyping();
        TypeWriter.Instance.StartTyping();
    }

    public void SetFullTextToType(string typeText)
    {
        if (this.gameObject.activeSelf)
        {
            TypeWriter.Instance.fullText = typeText;
        }
    }
}
