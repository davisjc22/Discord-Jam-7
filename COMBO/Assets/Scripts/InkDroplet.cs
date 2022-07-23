using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDroplet : MonoBehaviour
{
    GameManager manager;

    private Vector3 tempPosition;

    public float verticalSpeed;
    public float amplitude;

    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.01f;
    bool grounded = false;

    private float gravityValue = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tempPosition = transform.position;

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), .5f).setEase( LeanTweenType.easeOutBack );
    }

    // Update is called once per frame
    void Update()
    {
        //If the ink hit the ground, start hovering
        // if(grounded)
        // {
        //     Debug.Log("GROUNDED");
        //     tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        //     transform.position = tempPosition;
        // }
        // else // try to hit the ground
        // {
        //     Debug.Log("NOT GROUNDED");
        //     grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //     tempPosition.y += gravityValue * Time.deltaTime;
        //     transform.position = tempPosition;
        // }

        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        transform.position = tempPosition;
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player")) // if the player is colliding
        {
            manager.incrementCombo();
            Destroy(gameObject);
        }
    }
}
