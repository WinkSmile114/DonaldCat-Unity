using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public static CatController Instance = null;
    public Animator catAnimator;
    private Coroutine timeoutCoroutine;

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

    public void TalkAnimation()
    {
        catAnimator.SetBool("isTalking", true);
    }

    public void IdleAnimation()
    {
        catAnimator.SetBool("isTalking", false);
    }

    public void TouchAnimation()
    {
        catAnimator.SetBool("isTouched", true);
        timeoutCoroutine = StartCoroutine(DelayedAction(0.2f));
    }

    IEnumerator DelayedAction(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Perform the action after the delay
        catAnimator.SetBool("isTouched", false);
    }
}
