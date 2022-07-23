using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{   
    private Animator anim;
    private CharacterController controller;
    private Vector3 velocity;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent <CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");        
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if(direction.magnitude >= 0.1f)
        {
            anim.SetInteger ("AnimationPar", 1);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            // direction.y -= gravity * Time.deltaTime;

            controller.Move(direction * speed * Time.deltaTime);
        }
        else
        {
            anim.SetInteger ("AnimationPar", 0);
        }
    }
}
