using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{

    private Animator anim;

    private string status = "up";
    private int stepCounter = 0;
    private int stepMax = 10;
    public Transform lamp;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();

        // PressButton();
    }

    // Update is called once per frame
    void Update()
    {
        PressButton();
    }

    void PressButton()
    {

        if (status == "up")
        {
            stepCounter++;
            lamp.transform.Translate(Vector3.forward * 0.01f);
        }
        if (stepCounter >= stepMax)
        {
            anim.SetBool("PressingButton", true);
            anim.SetBool("TypingKeyboard", false);
            status = "down";
        }
        if (status == "down")
        {
            stepCounter--;
            lamp.transform.Translate(Vector3.back * 0.01f);
        }
        if (stepCounter <= 0)
        {
            anim.SetBool("TypingKeyboard", true);
            anim.SetBool("PressingButton", false);
            status = "up";
        }
    }
}
