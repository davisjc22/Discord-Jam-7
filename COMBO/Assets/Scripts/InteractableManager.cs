using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public InstructionManager alertBubble;
    public InstructionManager instructionBubble;
    private bool buttonIsActive;
    private bool playerInRange;
    public int triggerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        alertBubble.quickHideBubble();
        instructionBubble.quickHideBubble();
        activateButton();
    }

    void Update()
    {

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
}
