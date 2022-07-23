using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimation : MonoBehaviour
{

    public enum Interaction
    {
        IDLE,
        PRESS_BUTTON,
        TYPE_KEYBOARD,
    }
    private Animator anim;

    private string status = "up";
    private int stepCounter = 0;
    private int stepMax = 10;

    private bool pressingButton = false;
    private Transform interactable;

    public Interaction selected_interaction;

    public AudioSource audioData;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
        interactable = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressingButton = true;
            // this is just to demo the behavior
            ShowPlayerAnimation();
            audioData.Play();
        }
        ShowInteractibleAnimation();
    }

    void ShowPlayerAnimation()
    {
        anim.SetInteger("InteractionBehavior", ((int)selected_interaction));
    }

    void ShowInteractibleAnimation()
    {

        if (pressingButton)
        {
            if (status == "up")
            {
                stepCounter++;
                interactable.transform.Translate(Vector3.forward * 0.01f);
            }
            if (stepCounter >= stepMax)
            {
                status = "down";
            }
            if (status == "down")
            {
                stepCounter--;
                interactable.transform.Translate(Vector3.back * 0.01f);
            }
            if (stepCounter <= 0)
            {
                status = "up";
                anim.SetInteger("InteractionBehavior", ((int)Interaction.IDLE));
                pressingButton = false;
            }
        }
    }
}
