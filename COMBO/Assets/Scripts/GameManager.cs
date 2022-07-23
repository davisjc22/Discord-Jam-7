using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    List<InteractableManager> interactables; // list of interactables in the scene

    [Range(1, 6)]
    public int timeRange; // the amount of time between interactable alerts

    private int score;

    //private int numInks = 0;

    private int combo = 0;

    public GameObject inkPrefab;

    public GameObject player;

    public TextMeshProUGUI comboText;

    public KeyCode Key;
 
    public float startTime = 0f;
    public float holdTime = 5.0f; // 5 seconds
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // when the player presses down i for a set amount of time, drop an ink
        // if(Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     makeInk();
        // }
        if(Input.GetKeyDown(Key))
        {
            startTime = Time.time;
            Debug.Log("Starting Timer");
        }
        else if (Input.GetKey(Key))
        {
            if (startTime + holdTime <= Time.time)
            {
                Debug.Log("It Works Great!");
                makeInk();
                startTime = Time.time;
            }
        }
        else if(Input.GetKeyUp(Key))
        {
            Debug.Log("Ending Timer");
            startTime = 0;
        }
    }

    private void makeInk()
    {
        Vector3 startPosition = new Vector3(player.transform.position.x + 2f, 2f, player.transform.position.z);
        GameObject droplet = Instantiate(inkPrefab, startPosition, Quaternion.identity);
        //oplet.GetComponent<Rigidbody>().velocity = player.transform.TransformDirection(Vector3.forward * 5);
        
    }

    public void incrementCombo()
    {
        combo++;
        comboText.text = "X" + combo;
    }
}
