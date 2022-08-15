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

    // Player slider Bar
    public GameObject playerTimerBarPrefab;
    // the actual gameObject we want to control
    private GameObject playerTimerBar;
    private Vector3 playerTimerBarScale;
    private Vector3 playerTimerBarStartScale;
    public Color32 timerBarInkColor = new Color32(166, 31, 81, 255);

    private enum GameState
    {
        INFOSCREEN,
        PLAYING,
        RESULTS
    }
    private GameState gameState = GameState.INFOSCREEN;

    // Start is called before the first frame update
    void Start()
    {
        interactables = FindObjectsOfType<InteractableManager>();
        playerTimerBarScale = playerTimerBarPrefab.transform.localScale;
        playerTimerBar = playerTimerBarPrefab.transform.GetChild(1).gameObject;
        playerTimerBarStartScale = playerTimerBar.transform.localScale;
        playerTimerBar.transform.localScale = new Vector3(0, playerTimerBarStartScale.y, playerTimerBarStartScale.z);
        Renderer barRenderer = playerTimerBar.GetComponent<Renderer>();
        barRenderer.material.color = timerBarInkColor;
        resetPlayerTimerBar();
        hidePlayerTimerBar();
        startInterval = Time.time; // start the clock for the interactions
        environmentManager.ZoomToScoreboard();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 barPosition = new Vector3(player.transform.position.x, player.transform.position.y + 5f, player.transform.position.z);
        playerTimerBarPrefab.transform.position = barPosition;

        if (gameState == GameState.INFOSCREEN)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextState();
            }
        }
        else if (gameState == GameState.PLAYING)
        {

            if (Input.GetKeyDown(Key))
            {
                Renderer barRenderer = playerTimerBar.GetComponent<Renderer>();
                barRenderer.material.color = timerBarInkColor;
                startTime = Time.time;
                //Debug.Log("Starting Timer");
                showPlayerTimerBar();
                animatePlayerTimerBar(holdTime);
            }
            else if (Input.GetKey(Key))
            {
                if (startTime + holdTime <= Time.time)
                {
                    // Debug.Log("It Works Great!");
                    makeInk();
                    startTime = Time.time;
                    LeanTween.cancel(playerTimerBar);
                    resetPlayerTimerBar();
                    animatePlayerTimerBar(holdTime);
                }
            }
            else if (Input.GetKeyUp(Key))
            {
                //Debug.Log("Ending Timer");
                startTime = 0;

                LeanTween.cancel(playerTimerBar);
                resetPlayerTimerBar();
                hidePlayerTimerBar();
            }

            if (startInterval + interval <= Time.time)
            {
                //Debug.Log("Time for an action!");
                // pick a random interactable
                int index = Random.Range(0, interactables.Length);
                InteractableManager currentInteractable = interactables[index];
                currentInteractable.activateButton();// activate it
                startInterval = Time.time;
            }

            sbm.SetCombo(combo);
            sbm.SetScore(score);
            sbm.SetLives(numLives);
        }
        else if (gameState == GameState.RESULTS)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextState();
            }
        }

    }

    private void NextState()
    {
        switch (gameState)
        {
            case GameState.INFOSCREEN:
                ResetGame();
                environmentManager.ZoomAwayFromScoreboard();
                sbm.ReplaceScreen(ScoreboardManager.Screen.GAMEPLAY);
                gameState = GameState.PLAYING;
                break;
            case GameState.PLAYING:
                environmentManager.ZoomToScoreboard();
                sbm.ShowGameOver();
                gameState = GameState.RESULTS;
                break;
            case GameState.RESULTS:
                ResetGame();
                environmentManager.ZoomAwayFromScoreboard();
                sbm.ReplaceScreen(ScoreboardManager.Screen.GAMEPLAY);
                gameState = GameState.PLAYING;
                break;
        }
    }

    private void ResetGame()
    {
        combo = 1;
        score = 0;
        numLives = 3;
        sbm.ResetTimer();
        sbm.SetTimerEnabled(true);
    }

    private void makeInk()
    {
        // Vector3 startPosition = new Vector3(player.transform.position.x + 2f, 2f, player.transform.position.z);
        int inkIndex = Random.Range(0, inkSpawners.Count);
        Vector3 startPosition = inkSpawners[inkIndex].transform.position;
        GameObject droplet = Instantiate(inkPrefab, startPosition, Quaternion.identity);
    }

    private void LoseLife()
    {
        numLives--;
        if (numLives == 0)
        {
            NextState();
            foreach (InteractableManager interactable in interactables)
            {
                interactable.deactivateButton();
            }
        }
        else
        {

            environmentManager.isFlickering = true;
            environmentManager.isShaking = true;
        }
    }

    public void incrementCombo()
    {
        combo++;
    }

    public void updateScore()
    {
        score += points * combo;
    }

    public void missedAlert()
    {
        if (gameState == GameState.PLAYING)
        {
            LoseLife();
            combo = (int)Mathf.Max(Mathf.Ceil(combo / 2), 1f);
        }
    }

    // Player Timer Bar Stuff
    public void resetPlayerTimerBar()
    {
        LeanTween.moveLocalX(playerTimerBar, 0f, 0f);
        LeanTween.scaleX(playerTimerBar, 0f, 0f);
    }
    public void animatePlayerTimerBar(float howLong)
    {
        LeanTween.moveLocalX(playerTimerBar, 0, howLong);
        LeanTween.scaleX(playerTimerBar, playerTimerBarStartScale.x, howLong);
    }
    public void hidePlayerTimerBar()
    {
        LeanTween.scale(playerTimerBarPrefab, Vector3.zero, .3f).setEase(LeanTweenType.easeInBack);
    }

    public void showPlayerTimerBar()
    {
        LeanTween.scale(playerTimerBarPrefab, playerTimerBarScale, .5f).setEase(LeanTweenType.easeOutBack); ;
    }

    public void colorPlayerTimerBar(Color c)
    {
        Renderer barRenderer = playerTimerBar.GetComponent<Renderer>();
        barRenderer.material.color = c;
    }
}
