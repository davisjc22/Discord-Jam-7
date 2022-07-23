using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public InstructionManager alertBubble;
    public InstructionManager instructionBubble;
    public GameObject TimerBar;
    private GameObject bar;
    private Vector3 barStartScale;
    public int time;
    private bool buttonIsActive;
    private bool playerInRange;
    public int triggerCount = 0;
    public float barFillRatio = 0;


    // Start is called before the first frame update
    void Start()
    {
        alertBubble.quickHideBubble();
        instructionBubble.quickHideBubble();
        bar = TimerBar.transform.GetChild(1).gameObject;
        barStartScale = bar.transform.localScale;
        bar.transform.localScale = new Vector3(0, barStartScale.y, barStartScale.z);
        activateButton();
    }
    void Update()
    {
        barFillRatio = bar.transform.localScale.x/barStartScale.x;
        Renderer barRenderer = bar.GetComponent<Renderer>();
        
        if (barFillRatio < .5)
        {
            barRenderer.material.color = Color.green;
        }
        else if (barFillRatio < .9)
        {
            barRenderer.material.color = Color.yellow;
        }
        else
        {
            barRenderer.material.color = Color.red;
        }

        if(buttonIsActive && playerInRange)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                deactivateButton();
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player")) // if the player is colliding
        {
            triggerCount++;
            playerInRange = true;
            if(buttonIsActive)
            {
                alertBubble.hideBubble();
                instructionBubble.showBubble(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        
        if(other.gameObject.CompareTag("Player"))
        {
            triggerCount--;
            playerInRange = false;
            if(buttonIsActive)
            {
                instructionBubble.hideBubble();
                alertBubble.showBubble(true);
            }
        }
    }

    void animateTimerBar()
    {
        LeanTween.moveLocalX(bar,0,time);
        LeanTween.scaleX(bar,barStartScale.x,time).setOnComplete(() => deactivateButton());
    }
    void hideTimerBar()
    {
        LeanTween.scale(TimerBar, Vector3.zero, .3f).setEase( LeanTweenType.easeInBack);
    }
    

    void activateButton()
    {
        buttonIsActive = true;
        alertBubble.showBubble();
        animateTimerBar();
    }

    void deactivateButton()
    {
        buttonIsActive = false;
        hideTimerBar();
        alertBubble.hideBubble();
        instructionBubble.hideBubble();
    }
}
