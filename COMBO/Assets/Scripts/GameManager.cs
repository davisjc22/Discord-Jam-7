using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    InteractableManager[] interactables; // list of interactables in the scene
    public List<GameObject> inkSpawners;

    public int interval = 10; // the amount of time between interactable alerts

    public float startInterval = 0;

    private int combo = 1;
    private int score = 0;
    public int points = 100;

    private int numLives = 3;

    public GameObject inkPrefab;

    public GameObject player;

    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverText;

    public ScoreboardManager sbm;

    public EnvironmentManager environmentManager;

    public KeyCode Key;

    public float startTime = 0f;
    public float holdTime = 5.0f; // 5 seconds

    //Slider Bar
    public GameObject TimerBar;
    private Vector3 TimerBarScale;
    private GameObject bar;
    private Vector3 barStartScale;
    public Color32 barInk = new Color32(166, 31, 81, 255);

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactables = FindObjectsOfType<InteractableManager>();
        TimerBarScale = TimerBar.transform.localScale;
        bar = TimerBar.transform.GetChild(1).gameObject;
        barStartScale = bar.transform.localScale;
        bar.transform.localScale = new Vector3(0, barStartScale.y, barStartScale.z);
        Renderer barRenderer = bar.GetComponent<Renderer>();
        barRenderer.material.color = barInk;
        resetTimerBar();
        hideTimerBar();
        environmentManager.ZoomToScoreboard();
        ResetGame(true);
        startInterval = Time.time; // start the clock for the interactions
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 barPosition = new Vector3(player.transform.position.x, player.transform.position.y + 5f, player.transform.position.z);
        TimerBar.transform.position = barPosition;

        if (!isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame(false);
            }
        }
        if (Input.GetKeyDown(Key))
        {
            Renderer barRenderer = bar.GetComponent<Renderer>();
            barRenderer.material.color = barInk;
            startTime = Time.time;
            //Debug.Log("Starting Timer");
            showTimerBar();
            animateTimerBar(holdTime);
        }
        else if (Input.GetKey(Key))
        {
            if (startTime + holdTime <= Time.time)
            {
               // Debug.Log("It Works Great!");
                makeInk();
                startTime = Time.time;
                LeanTween.cancel(bar);
                resetTimerBar();
                animateTimerBar(holdTime);
            }
        }
        else if (Input.GetKeyUp(Key))
        {
            //Debug.Log("Ending Timer");
            startTime = 0;

            LeanTween.cancel(bar);
            resetTimerBar();
            hideTimerBar();
        }

        // if time > starttime + timeRange

        if (startInterval + interval <= Time.time)
        {
            //Debug.Log("Time for an action!");
            // pick a random interactable
            int index = Random.Range(0, interactables.Length);
            if (isPlaying)
            {

                InteractableManager currentInteractable = interactables[index];
                currentInteractable.activateButton();// activate it
            }
            startInterval = Time.time;
        }

    }

    private void ResetGame(bool firstTime)
    {
        combo = 1;
        score = 0;
        numLives = 3;
        sbm.ShowGamePlay(firstTime);
        sbm.SetCombo(combo);
        sbm.SetScore(score);
        sbm.SetLives(numLives);
        sbm.ResetTimer();
        sbm.SetTimerEnabled(true);
        isPlaying = true;
        environmentManager.ZoomAwayFromScoreboard();
    }

    private void makeInk()
    {
        // Vector3 startPosition = new Vector3(player.transform.position.x + 2f, 2f, player.transform.position.z);
        int inkIndex = Random.Range (0, inkSpawners.Count);
        Vector3 startPosition = inkSpawners[inkIndex].transform.position;
        GameObject droplet = Instantiate(inkPrefab, startPosition, Quaternion.identity);
    }

    public void incrementCombo()
    {
        combo++;
        sbm.SetCombo(combo);
    }

    public void updateScore()
    {
        score += points * combo;
        sbm.SetScore(score);
    }

    public void missedAlert()
    {
        if (isPlaying)
        {

            numLives--;
            sbm.SetLives(numLives);
            //Debug.Log("Lost a life :(");
            if (numLives == 0)
            {
                sbm.ShowGameOver();
                environmentManager.ZoomToScoreboard();
                isPlaying = false;
                foreach (InteractableManager interactable in interactables)
                {
                    interactable.deactivateButton(false);
                }
            }
            else
            {

                environmentManager.isFlickering = true;
                environmentManager.isShaking = true;
            }
            combo = (int)Mathf.Max(Mathf.Ceil(combo / 2), 1f);
            sbm.SetCombo(combo);
        }
    }

    // Ink Bar Stuff
    public void resetTimerBar()
    {
        LeanTween.moveLocalX(bar, 0f, 0f);
        LeanTween.scaleX(bar, 0f, 0f);
    }
    public void animateTimerBar(float howLong)
    {
        LeanTween.moveLocalX(bar,0,howLong);
        LeanTween.scaleX(bar,barStartScale.x,howLong);
    }
    public void hideTimerBar()
    {
        LeanTween.scale(TimerBar, Vector3.zero, .3f).setEase(LeanTweenType.easeInBack);
    }

    public void showTimerBar()
    {
        LeanTween.scale(TimerBar, TimerBarScale, .5f).setEase(LeanTweenType.easeOutBack); ;
    }

    public void colorTimerBar(Color c)
    {
        Renderer barRenderer = bar.GetComponent<Renderer>();
        barRenderer.material.color = c;
    }
}
