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
    }
    public Interaction selected_interaction;

    private Animator anim;

    public GameObject player;

    private Transform interactable;

    public AudioSource audioData;

    private string status = "pressing";
    private int stepCounter = 0;
    private int stepMax = 10;

    private bool isInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        interactable = gameObject.transform;
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
                isInteracting = true;
                ShowPlayerAnimation();
                audioData.Play();
                deactivateButton();
            }
        }
        ShowInteractibleAnimation();
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

    void ShowInteractibleAnimation()
    {

        if (isInteracting)
        {
            if (status == "pressing")
            {
                stepCounter++;
                interactable.transform.Translate(Vector3.down * 0.01f);
            }
            if (stepCounter >= stepMax)
            {
                status = "depressing";
            }
            if (status == "depressing")
            {
                stepCounter--;
                interactable.transform.Translate(Vector3.up * 0.01f);
            }
            if (stepCounter <= 0)
            {
                status = "pressing";
                anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
                isInteracting = false;
            }
        }
    }
}
