using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    public static TypeWriter Instance = null;
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;

    void Awake()
    {
        Instance = this;
    }

    public float delay = 0.05f;
    public TextMeshProUGUI chatText;
    private string currentText = "";
    public string fullText;
    public bool isTyping = false;   

    // Start is called before the first frame update
    void Start()
    {
        if (scrollRect != null && scrollbar != null)
        {
            // Set the vertical scrollbar's value to the bottom (1.0f)
            scrollbar.value = 1.0f;

            // Optional: If you want to instantly scroll to the bottom
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopTyping()
    {
        isTyping = false;
        chatText.text = "";
        StopAllCoroutines();
    }

    public void StartTyping()
    {
        isTyping = true;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            chatText.text = currentText;
            ScrollToBottom();
            yield return new WaitForSeconds(delay);
        }
        this.isTyping = false;
        GameController.Instance.AnimatControl("idle");
    }

    void ScrollToBottom()
    {
        if (scrollbar != null)
        {
            // Ensure scrollbar is set to the bottom
            scrollbar.value = 0.0f;

            // Optionally, adjust scroll rect position
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
}
