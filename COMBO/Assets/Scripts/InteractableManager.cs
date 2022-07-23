using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public InstructionManager alertBubble;
    public InstructionManager instructionBubble;
    private bool buttonIsActive;

    // Start is called before the first frame update
    void Start()
    {
        alertBubble.quickHideBubble();
        instructionBubble.quickHideBubble();
        activateButton();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Trigger Enter");
        if(buttonIsActive && other.gameObject.CompareTag("Player"))
        {
            alertBubble.hideBubble();
            instructionBubble.showBubble(1f);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        Debug.Log("Trigger Exit");
        if(buttonIsActive && other.gameObject.CompareTag("Player"))
        {
            alertBubble.showBubble(1f);
            instructionBubble.hideBubble();
        }
    }

    void activateButton()
    {
        buttonIsActive = true;
        alertBubble.showBubble();
        // do a dance and a jig

    }
}
