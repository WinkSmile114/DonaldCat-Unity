using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GameController.Instance.VoiceInputControl("press");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameController.Instance.VoiceInputControl("release");
    }
}
