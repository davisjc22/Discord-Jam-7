using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 2.0f;
    public float gravityValue = -100f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public LayerMask groundMask;
    bool isGrounded;

    public ParticleSystem system;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {   
        groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //groundedPlayer = controller.isGrounded;
        if(groundedPlayer)
        {

        }
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(move.magnitude >= 0.1f && groundedPlayer)
        {
            anim.SetInteger ("AnimationPar", 1);
            system.Play();
        }
        else
        {
            anim.SetInteger ("AnimationPar", 0);
            system.Stop();
        }

    }
}