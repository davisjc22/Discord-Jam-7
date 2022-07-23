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



    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        alertBubble.quickHideBubble();
        instructionBubble.quickHideBubble();
        bar = TimerBar.transform.GetChild(1).gameObject;
        barStartScale = bar.transform.localScale;
        bar.transform.localScale = new Vector3(0, barStartScale.y, barStartScale.z);
        activateButton();
    }
    void Update()
    {
        float barFillRatio = bar.transform.localScale.x / barStartScale.x;
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

        if (buttonIsActive && playerInRange)
        {
            if (Input.GetKeyDown(trigger))
            {
                ShowPlayerAnimation();
                ShowInteractibleAnimation();
                audioData.Play();
                deactivateButton();
            }
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
        LeanTween.scaleX(bar, barStartScale.x, time).setOnComplete(() => deactivateButton());
    }
    void hideTimerBar()
    {
        LeanTween.scale(TimerBar, Vector3.zero, .3f).setEase(LeanTweenType.easeInBack);
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
        }

    }
}
