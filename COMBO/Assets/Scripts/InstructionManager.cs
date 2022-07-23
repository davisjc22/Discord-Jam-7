using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using LeanTween;

public class InstructionManager : MonoBehaviour
{

    private Vector3 originalScale = new Vector3(1,1,1);

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

    public void showBubble()
    {
        LeanTween.scale(gameObject, originalScale, 1f).setEase( LeanTweenType.easeOutBack );
    }

    public void hideBubble()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEase( LeanTweenType.easeInBack );
    }
}
