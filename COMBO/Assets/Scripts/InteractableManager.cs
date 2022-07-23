using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public InstructionManager alertBubble;
    public InstructionManager instructionBubble;
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
        activateButton();
    }

    void Update()
    {
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


    void activateButton()
    {
        buttonIsActive = true;
        alertBubble.showBubble();
        // do a dance and a jig
    }

    void deactivateButton()
    {
        buttonIsActive = false;
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

        LeanTween.moveLocalY(interactable, -0.1f, 0.5f).setOnComplete(() =>
        {
            LeanTween.moveLocalY(interactable, 0.1f, 0.5f).setOnComplete(() =>
            {
                anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
            });
        });

    }
    void SpinWheel()
    {

        LeanTween.rotateZ(interactable, 1080, 4).setOnComplete(() =>
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
