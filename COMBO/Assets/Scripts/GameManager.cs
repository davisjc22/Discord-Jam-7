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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // when the player presses down i for a set amount of time, drop an ink
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            makeInk();
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
