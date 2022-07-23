using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using LeanTween;

public class InstructionManager : MonoBehaviour
{

    private Vector3 originalScale;

    public TextMesh text;

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

    public void showBubble(float delay = 0f)
    {
        LeanTween.scale(gameObject, originalScale, 1f).setEase( LeanTweenType.easeOutBack ).setDelay(delay);
    }

    public void hideBubble(float delay = 0f)
    {
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEase( LeanTweenType.easeInBack ).setDelay(delay);
    }

       public void quickHideBubble(float delay = 0f)
    {
        LeanTween.scale(gameObject, Vector3.zero, 0f).setDelay(delay);
    }
}
