using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public GameManager manager;
    public InstructionManager alertBubble;
    public InstructionManager instructionBubble;
    public GameObject interactableTimerBarPrefab;
    // the game object we control the color of
    private GameObject interactableTimerBar;
    private Vector3 interactableTimerBarStartPosition;
    private Vector3 interactableTimerBarStartScale;
    private Vector3 interactableTimerBarScale;
    public int time;

    //interactable timer
    public float startTime = 0f;


    // key holding
    public float pressStartTime;
    public float pressHoldTime = 3f;

    private GameObject playerTimerBar;

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

    public Color32 interactionInProgress = new Color32(48, 123, 140, 255);

    public Color32 barGreen = new Color32(0, 245, 149, 255);
    public Color32 barYellow = new Color32(239, 129, 78, 255);
    public Color32 barRed = new Color32(181, 37, 37, 255);

    public ParticleSystem interactionParticle;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
        alertBubble.quickHideBubble();
        instructionBubble.quickHideBubble();
        interactableTimerBar = interactableTimerBarPrefab.transform.Find("bar").gameObject;
        interactableTimerBarStartScale = interactableTimerBar.transform.localScale;
        interactableTimerBarStartPosition = interactableTimerBar.transform.localPosition;
        interactableTimerBarScale = interactableTimerBarPrefab.transform.localScale;
        interactableTimerBar.transform.localScale = new Vector3(0, interactableTimerBarStartScale.y, interactableTimerBarStartScale.z);
        pressStartTime = 0;
        pressHoldTime = 3f;
        hideInteractableTimerBar();

        playerTimerBar = GameObject.Find("PlayerTimerBar");
        //activateButton();
    }
    void Update()
    {
        bool holdingButton = false;
        float barFillRatio = interactableTimerBar.transform.localScale.x / interactableTimerBarStartScale.x;
        Renderer barRenderer = interactableTimerBar.GetComponent<Renderer>();

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
            // Debug.Log("pressStartTime: " + pressStartTime);
            // Debug.Log("pressHoldTime: " + pressHoldTime);
            // Debug.Log("Time: " + Time.time);
            if (Input.GetKeyDown(trigger)) //when the key is first pressed
            {
                pressStartTime = Time.time;
                //Debug.Log("Starting Timer");
                pauseInteractableTimerBar();
                holdingButton = true;

                //start the player timer
                startPlayerTimerBar();
            }
            else if (Input.GetKey(trigger))
            {
                if (pressStartTime + pressHoldTime <= Time.time)
                {
                    deactivateButton(); // successfully performed operation
                    manager.updateScore();
                    Debug.Log("Good job");
                    LeanTween.cancel(interactableTimerBar);
                    hideInteractableTimerBar();
                    resetInteractableTimerBar();
                    pressStartTime = 0;
                    stopPlayerTimerBar();
                    Instantiate(interactionParticle, transform.position, Quaternion.identity);
                    ShowInteractibleAnimation();
                    ShowPlayerAnimation();
                    audioData.Play();
                }
                holdingButton = true;
            }
            else if (Input.GetKeyUp(trigger))
            {
                pressStartTime = 0;
                holdingButton = false;

                stopPlayerTimerBar();
            }
        }

        if (buttonIsActive && (startTime + time <= Time.time && buttonIsActive) && !holdingButton)
        {
            deactivateButton();
            manager.missedAlert();
            Debug.Log("You suck ");
        }
    }

    void startPlayerTimerBar()
    {
        manager.colorPlayerTimerBar(interactionInProgress);
        manager.showPlayerTimerBar();
        manager.animatePlayerTimerBar(pressHoldTime);

    }

    void stopPlayerTimerBar()
    {
        manager.hidePlayerTimerBar();
        manager.resetPlayerTimerBar();
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

    void animateInteractableTimerBar()
    {
        LeanTween.moveLocalX(interactableTimerBar, 0, time);
        // LeanTween.scaleX(bar, barStartScale.x, time);
        LeanTween.scaleX(interactableTimerBar, interactableTimerBarStartScale.x, time).setOnComplete(() => hideInteractableTimerBar());
    }

    void pauseInteractableTimerBar()
    {
        LeanTween.pause(interactableTimerBar);

    }

    void hideInteractableTimerBar()
    {
        LeanTween.scale(interactableTimerBarPrefab, Vector3.zero, .3f).setEase(LeanTweenType.easeInBack).setOnComplete(() => resetInteractableTimerBar());
    }

    void showInteractableTimerBar()
    {
        LeanTween.scale(interactableTimerBarPrefab, interactableTimerBarScale, .5f).setEase(LeanTweenType.easeOutBack);
    }

    void resetInteractableTimerBar()
    {
        interactableTimerBar.transform.localScale = new Vector3(0, interactableTimerBarStartScale.y, interactableTimerBarStartScale.z);
        interactableTimerBar.transform.localPosition = interactableTimerBarStartPosition;
    }

    public void activateButton()
    {
        buttonIsActive = true;
        if (playerInRange)
        {
            instructionBubble.showBubble();
        }
        else
        {
            alertBubble.showBubble();
        }
        showInteractableTimerBar();
        animateInteractableTimerBar();
        startTime = Time.time;
    }

    public void deactivateButton()
    {
        buttonIsActive = false;
        alertBubble.hideBubble();
        instructionBubble.hideBubble();
        hideInteractableTimerBar();
    }

    void resetPlayerAnimation()
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
                resetPlayerAnimation();
                break;
        }

    }
}
