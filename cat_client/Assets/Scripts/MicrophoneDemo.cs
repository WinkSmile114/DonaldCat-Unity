using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using Whisper;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;
using TMPro;

/// <summary>
/// Record audio clip from microphone and make a transcription.
/// </summary>
public class MicrophoneDemo : MonoBehaviour
{
    public static MicrophoneDemo Instance = null;

    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public bool streamSegments = true;
    public bool printLanguage = true;

    [Header("UI")] 
    public Button recordButton;
    //public Text buttonText;
    //public Text outputText;
    public TextMeshProUGUI timeText;
    //public Dropdown languageDropdown;
    //public Toggle translateToggle;
    //public Toggle vadToggle;
    //public ScrollRect scroll;

    private string _buffer;

    private void Awake()
    {
        Instance = this;

        whisper.OnNewSegment += OnNewSegment;
        whisper.OnProgress += OnProgressHandler;
            
        microphoneRecord.OnRecordStop += OnRecordStop;

        //recordButton.onClick.AddListener(OnButtonPressed);
        //languageDropdown.value = languageDropdown.options
        //    .FindIndex(op => op.text == whisper.language);
        //languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        //translateToggle.isOn = whisper.translateToEnglish;
        //translateToggle.onValueChanged.AddListener(OnTranslateChanged);

        //vadToggle.isOn = microphoneRecord.vadStop;
        //vadToggle.onValueChanged.AddListener(OnVadChanged);
    }

    //private void OnVadChanged(bool vadStop)
    //{
    //    microphoneRecord.vadStop = vadStop;
    //}

    void ChangeButtonColor(float targetOpacity)
    {
        Image buttonImage = recordButton.GetComponent<Image>();

        // Check if the button has an Image component
        if (buttonImage != null)
        {
            // Get the current color of the button
            Color color = buttonImage.color;

            // Change the alpha value to the target opacity
            color.a = targetOpacity;

            // Set the new color back to the button's Image component
            buttonImage.color = color;
        }
    }

    public void OnButtonPressed()
    {
        //if (!microphoneRecord.IsRecording)
        //{
            microphoneRecord.StartRecord();
            this.ChangeButtonColor(0.5f);
            //buttonText.text = "Stop";
        //}
        //else
        //{
        //    microphoneRecord.StopRecord();
        //    this.ChangeButtonColor(1f);
        //    //buttonText.text = "Record";
        //}
    }

    public void OnButtonReleased()
    {
        microphoneRecord.StopRecord();
        this.ChangeButtonColor(1f);
    }
        
    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        //buttonText.text = "Record";
        this.ChangeButtonColor(1f);
        _buffer = "";

        var sw = new Stopwatch();
        sw.Start();
            
        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null) 
            return;

        var time = sw.ElapsedMilliseconds;
        var rate = recordedAudio.Length / (time * 0.001f);
        timeText.text = $"{time} ms";

        var text = res.Result;
        //if (printLanguage)
        //    text += $"\n\nLanguage: {res.Language}";

        //UnityEngine.Debug.Log("------" + text + "------");
        if (text.Contains("[BLANK_AUDIO]"))
        {
            UIManager.Instance.SetPlaceholder("Speech Not Recognized. Try again or Use Text Input!");

        }
        else
        {
            GameController.Instance.SetPrompt(text);
        }
        
        //UnityEngine.Debug.Log("recorded text" + text);
            
        //outputText.text = text;
        //UiUtils.ScrollDown(scroll);
    }
        
    //private void OnLanguageChanged(int ind)
    //{
    //    var opt = languageDropdown.options[ind];
    //    whisper.language = opt.text;
    //}
        
    //private void OnTranslateChanged(bool translate)
    //{
    //    whisper.translateToEnglish = translate;
    //}

    private void OnProgressHandler(int progress)
    {
        //if (!timeText)
        //    return;
        //timeText.text = $"Progress: {progress}%";
    }

    private void OnNewSegment(WhisperSegment segment)
    {
        if (!streamSegments)
            return;

        _buffer += segment.Text;

        UnityEngine.Debug.Log(_buffer + "...");
        //outputText.text = _buffer + "...";
        //UiUtils.ScrollDown(scroll);
    }

}