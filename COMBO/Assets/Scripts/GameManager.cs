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
 

    //Slider Bar
    public GameObject TimerBar;
    private Vector3 TimerBarScale;
    private GameObject bar;
    private Vector3 barStartScale;

    // Start is called before the first frame update
    void Start()
    {
        TimerBarScale = TimerBar.transform.localScale;
        bar = TimerBar.transform.GetChild(1).gameObject;
        barStartScale = bar.transform.localScale;
        bar.transform.localScale = new Vector3(0, barStartScale.y, barStartScale.z);
        Renderer barRenderer = bar.GetComponent<Renderer>();
        barRenderer.material.color = Color.magenta;
        resetTimerBar();
        hideTimerBar();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 barPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z);
        TimerBar.transform.position = barPosition;

        if(Input.GetKeyDown(Key))
        {
            startTime = Time.time;
            Debug.Log("Starting Timer");
            showTimerBar();
            animateTimerBar();
        }
        else if (Input.GetKey(Key))
        {
            if (startTime + holdTime <= Time.time)
            {
                Debug.Log("It Works Great!");
                makeInk();
                startTime = Time.time;
                LeanTween.cancel(bar);
                resetTimerBar();
                animateTimerBar();
            }
        }
        else if(Input.GetKeyUp(Key))
        {
            Debug.Log("Ending Timer");
            startTime = 0;
            
            LeanTween.cancel(bar);
            resetTimerBar();
            hideTimerBar();
        }
    }

    private void makeInk()
    {
        Vector3 startPosition = new Vector3(player.transform.position.x + 2f, 2f, player.transform.position.z);
        GameObject droplet = Instantiate(inkPrefab, startPosition, Quaternion.identity);
        
    }

    public void incrementCombo()
    {
        combo++;
        comboText.text = "X" + combo;
    }

    void resetTimerBar()
    {
        LeanTween.moveLocalX(bar, 0f, 0f);
        LeanTween.scaleX(bar,0f,0f);
    }
    void animateTimerBar()
    {
        LeanTween.moveLocalX(bar,0,holdTime);
        LeanTween.scaleX(bar,barStartScale.x,holdTime);
    }
    void hideTimerBar()
    {
        LeanTween.scale(TimerBar, Vector3.zero, .3f).setEase( LeanTweenType.easeInBack);
    }

    void showTimerBar()
    {
        LeanTween.scale(TimerBar, TimerBarScale, .5f).setEase( LeanTweenType.easeOutBack);;
    }
}
