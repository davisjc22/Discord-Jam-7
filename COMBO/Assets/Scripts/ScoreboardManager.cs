using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTextElement;
    public TextMeshProUGUI alertsTextElement;
    public TextMeshProUGUI comboTextElement;
    public TextMeshProUGUI timeTextElement;
    public RawImage life1;
    public RawImage life2;
    public RawImage life3;
    private bool timerEnabled;
    private float timerCounter = 0f;
    private int timerMin = 0;
    private int timerSec = 0;
    private string timerString = "";

    private string scoreString = "";
    private string comboString = "";
    private int numLives = 3;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(timerEnabled)
        {
            timerCounter += Time.deltaTime;
        }
        timerMin = (int)Mathf.Floor(Mathf.Floor(timerCounter) / 60);
        timerSec = (int)Mathf.Floor(timerCounter) % 60;
        timerString = timerMin.ToString() + ":" + timerSec.ToString().PadLeft(2,'0');
        timeTextElement.SetText(timerString);
        scoreTextElement.SetText(scoreString);
        comboTextElement.SetText(comboString);
        switch(numLives)
        {
            case 3:
            {
                life1.enabled = true;
                life2.enabled = true;
                life3.enabled = true;
                break;
            }
            case 2:
            {
                life1.enabled = false;
                life2.enabled = true;
                life3.enabled = true;
                break;
            }
            case 1:
            {
                life1.enabled = false;
                life2.enabled = false;
                life3.enabled = true;
                break;
            }
            case 0:
            {
                life1.enabled = false;
                life2.enabled = false;
                life3.enabled = false;
                break;
            }
            default:
            {
                life1.enabled = true;
                life2.enabled = true;
                life3.enabled = true;
                break;
            }
        }

    }

    public void SetScore(string value)
    {
        scoreString = value.ToString();
    }
    public void SetCombo(string value)
    {
        comboString = value.ToString();
    }
    public void SetLives(int _numLives)
    {
        numLives = _numLives;
    }
    public void EnableTimer(bool setEnableTimer)
    {
        timerEnabled = setEnableTimer;
    }
}
