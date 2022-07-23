using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using LeanTween;

public class InstructionManager : MonoBehaviour
{

    private Vector3 originalScale;
    public TextMesh text;

    public float showTime = .75f;
    public float hideTime = .3f;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        //hideBubble();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void showBubble(bool delay = false)
    {
        float delayTime = 0f;
        if(delay)
        {
            delayTime = hideTime;
        }
        
        LeanTween.scale(gameObject, originalScale, showTime).setEase( LeanTweenType.easeOutBack ).setDelay(delayTime);
    }

    public void hideBubble(bool delay = false)
    {
        if(LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }

        float delayTime = 0f;
        if(delay)
        {
            delayTime = showTime;
        }

        LeanTween.scale(gameObject, Vector3.zero, hideTime).setEase( LeanTweenType.easeInBack ).setDelay(delayTime);
    }

       public void quickHideBubble(float delay = 0f)
    {
        LeanTween.scale(gameObject, Vector3.zero, 0f).setDelay(delay);
    }
}
