using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public GameManager manager;
    public InstructionManager alertBubble;
    public InstructionManager instructionBubble;
    public GameObject TimerBar;
    private GameObject bar;
    private Vector3 barStartPosition;
    private Vector3 barStartScale;
    private Vector3 TimerBarScale;
    public int time;

    //interactable timer
    public float startTime = 0f;

    private bool buttonIsActive;
    private bool playerInRange;
    public KeyCode trigger;
    public enum Interaction
    {
        IDLE,
        PRESS_BUTTON,
        TYPE_KEYBOARD,
        PULL_LEVER,
        SPIN_WHEEL
    }
    public Interaction selected_interaction;
    private Animator anim;
    public GameObject player;
    public GameObject interactable;
    public AudioSource audioData;

    public Color32 barGreen = new Color32(0, 245, 149, 255);
    public Color32 barYellow = new Color32(239, 129, 78, 255);
    public Color32 barRed = new Color32(181, 37, 37, 255);


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
        alertBubble.quickHideBubble();
        instructionBubble.quickHideBubble();
        bar = TimerBar.transform.Find("bar").gameObject;
        barStartScale = bar.transform.localScale;
        barStartPosition = bar.transform.localPosition;
        TimerBarScale = TimerBar.transform.localScale;
        bar.transform.localScale = new Vector3(0, barStartScale.y, barStartScale.z);

        hideTimerBar();
        //activateButton();
    }
    void Update()
    {
        float barFillRatio = bar.transform.localScale.x / barStartScale.x;
        Renderer barRenderer = bar.GetComponent<Renderer>();

        if (barFillRatio < .5)
        {
            barRenderer.material.color = barGreen;
        }
        else if (barFillRatio < .9)
        {
            barRenderer.material.color = barYellow;
        }
        else
        {
            barRenderer.material.color = barRed;
        }

        if (buttonIsActive && playerInRange)
        {
            if (Input.GetKeyDown(trigger))
            {
                
                ShowPlayerAnimation();
                ShowInteractibleAnimation();
                audioData.Play();
                LeanTween.cancel(bar);
                deactivateButton(true);
                hideTimerBar();
            }
        }

        if(startTime + time <= Time.time && buttonIsActive)
        {
            deactivateButton(false);
        }

    }

    void ShowPlayerAnimation()
    {
        anim.SetInteger("InteractionBehavior", ((int)selected_interaction));
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // if the player is colliding
        {
            playerInRange = true;
            if (buttonIsActive)
            {
                alertBubble.hideBubble();
                instructionBubble.showBubble(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            if (buttonIsActive)
            {
                instructionBubble.hideBubble();
                alertBubble.showBubble(true);
            }
        }
    }

    void animateTimerBar()
    {
        LeanTween.moveLocalX(bar, 0, time);
        // LeanTween.scaleX(bar, barStartScale.x, time);
        LeanTween.scaleX(bar, barStartScale.x, time).setOnComplete(() => hideTimerBar());
    }
    void hideTimerBar()
    {
        LeanTween.scale(TimerBar, Vector3.zero, .3f).setEase(LeanTweenType.easeInBack).setOnComplete(() => resetTimerBar());
    }

    void showTimerBar()
    {
        LeanTween.scale(TimerBar, TimerBarScale, .5f).setEase( LeanTweenType.easeOutBack);
    }

    void resetTimerBar()
    {
        bar.transform.localScale = new Vector3(0, barStartScale.y, barStartScale.z);
        bar.transform.localPosition = barStartPosition;
        // LeanTween.moveLocalX(bar, 0f, 0f);
        // LeanTween.scaleX(bar,0f,0f);
    }

    public void activateButton()
    {
        buttonIsActive = true;
        alertBubble.showBubble();
        showTimerBar();
        animateTimerBar();
        startTime = Time.time;
    }

    void deactivateButton(bool completed)
    {
        buttonIsActive = false;
        // hideTimerBar();
        alertBubble.hideBubble();
        instructionBubble.hideBubble();
        
        if(completed)
        {
            manager.updateScore();
            Debug.Log("Good job");
        }
        else
        {
            manager.missedAlert();
            Debug.Log("You suck ");
        }
    }

    void resetAnimation()
    {
        LeanTween.moveY(interactable, interactable.transform.position.y, 0.5f).setOnComplete(() =>
        {
        anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
        });
    }

    void PressButton()
    {
        LeanTween.moveY(interactable, interactable.transform.position.y - 0.1f, 0.5f).setOnComplete(() =>
        {
            LeanTween.moveY(interactable, interactable.transform.position.y + 0.1f, 0.5f).setOnComplete(() =>
            {
                anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
            });
        });

    }

    void PullLever()
    {

        LeanTween.rotateX(interactable, 45, 0.5f).setOnComplete(() =>
        {
            LeanTween.rotateX(interactable, 0, 0.5f).setOnComplete(() =>
            {
                anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
            });
        });

    }
    void SpinWheel()
    {

        LeanTween.rotateZ(interactable, 1080, 2).setDelay(1.0f).setOnComplete(() =>
        {
            anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
        });

    }

    void ShowInteractibleAnimation()
    {

        switch (selected_interaction)
        {
            case Interaction.PRESS_BUTTON:
                PressButton();
                break;
            case Interaction.PULL_LEVER:
                PullLever();
                break;
            case Interaction.SPIN_WHEEL:
                SpinWheel();
                break;
            default:
                resetAnimation();
                break;
        }

    }
}
