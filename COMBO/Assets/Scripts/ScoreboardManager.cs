using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTextElement;
    public TextMeshProUGUI comboTextElement;
    public TextMeshProUGUI timeTextElement;

    public TextMeshProUGUI gameOverTimeTextElement;
    public TextMeshProUGUI gameOverScoreTextElement;
    public GameObject resultsScreen;
    public GameObject gameplayScreen;
    public GameObject infoScreen;
    public RawImage life1;
    public RawImage life2;
    public RawImage life3;
    public RawImage[] ratingImages;
    private bool timerEnabled;
    private float timerCounter = 0f;
    private bool isGameOver = false;
    private int score = 0;
    private int combo = 1;
    private int rating = 0;
    private int numLives = 0;

    private GameObject activeScreen;


    // Start is called before the first frame update
    void Start()
    {
        ShowScreen(infoScreen);
    }

    string SecondsToTime(int time)
    {
        int timerMin = (int)Mathf.Floor(Mathf.Floor(time) / 60);
        int timerSec = (int)Mathf.Floor(time) % 60;
        return timerMin.ToString() + ":" + timerSec.ToString().PadLeft(2, '0');
    }

    string CommasInNumber(int value)
    {
        return value.ToString("#,##0");
    }

    // Update is called once per frame
    void Update()
    {
        if (timerEnabled)
        {
            timerCounter += Time.deltaTime;
        }
        timeTextElement.SetText(SecondsToTime(((int)timerCounter)));
        scoreTextElement.SetText(CommasInNumber(score));
        comboTextElement.SetText(combo.ToString() + "X");
        switch (numLives)
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

    public void SetScore(int _score)
    {
        score = _score;
    }
    public void SetCombo(int _combo)
    {
        combo = _combo;
    }
    public void SetLives(int _numLives)
    {
        numLives = _numLives;
    }
    public void SetRating(int _rating)
    {
        rating = _rating;
    }
    public void SetTimerEnabled(bool _timeEnabled)
    {
        timerEnabled = _timeEnabled;
    }
    public void ResetTimer()
    {
        timerCounter = 0f;
    }
    public LTDescr ShowScreen(GameObject screen)
    {
        screen.transform.position = new Vector3(screen.transform.position.x, 800, screen.transform.position.z);
        activeScreen = screen;
        return LeanTween.moveLocalY(screen, 0, 0.8f).setEase(LeanTweenType.easeInOutQuad).setDelay(0.5f);
    }
    public void HideScreen(GameObject screen)
    {
        LeanTween.moveLocalY(screen, screen.transform.position.y - 30, 0.2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
         {
             LeanTween.moveLocalY(screen, -800, 1.0f).setEase(LeanTweenType.easeInOutQuad);
         });
    }
    public LTDescr ReplaceScreen(GameObject oldScreen, GameObject newScreen)
    {
        HideScreen(activeScreen);
        return ShowScreen(newScreen);
    }
    public void ShowGameOver()
    {
        SetTimerEnabled(false);
        life1.enabled = false;
        life2.enabled = false;
        life3.enabled = false;
        foreach (RawImage image in ratingImages)
        {
            image.enabled = false;
        }
        float maximumPPS = 2000f / 60f;
        int _rating = (int)Mathf.Max(Mathf.Min((score / timerCounter) / maximumPPS, 8f), 1f);
        SetRating(_rating);
        gameOverScoreTextElement.SetText("0");
        gameOverTimeTextElement.SetText("0:00");
        ReplaceScreen(gameplayScreen, resultsScreen).setOnComplete(() =>
        {
            StartCoroutine(TallyTimer((int)timerCounter));

        });
    }
    public void ShowGamePlay(bool firstTime)
    {
        StopAllCoroutines();
        if (firstTime)
        {
            ShowScreen(infoScreen);
            // StartGamePlay();
        }
        else
        {
            // HideScreen(infoScreen);
            ReplaceScreen(resultsScreen, gameplayScreen);
        }
    }
    public void StartGamePlay()
    {
        foreach (RawImage image in ratingImages)
        {
            image.enabled = false;
        }
        SetScore(0);
        SetLives(3);
        timerCounter = 0;
        ShowScreen(gameplayScreen);
    }

    IEnumerator TallyTimer(int value)
    {
        float duration = 2f;
        float currentValue = 0f;
        while (Math.Abs(currentValue - value) > 0.4f)
        {
            currentValue = Mathf.MoveTowards(currentValue, value, (value - currentValue) / duration);
            int currentInt = (int)currentValue;
            string currentString = SecondsToTime(currentInt);
            gameOverTimeTextElement.SetText(currentString);
            yield return new WaitForSeconds(0.05f);
        }
        gameOverTimeTextElement.SetText(SecondsToTime(value));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TallyScore(score));
    }

    IEnumerator TallyScore(int value)
    {
        float duration = 2f;
        float currentValue = 0f;
        while (Math.Abs(currentValue - value) > 0.4f)
        {
            currentValue = Mathf.MoveTowards(currentValue, value, (value - currentValue) / duration);
            int currentInt = (int)currentValue;
            string currentString = CommasInNumber(currentInt);
            gameOverScoreTextElement.SetText(currentString);
            yield return new WaitForSeconds(0.05f);
        }
        gameOverScoreTextElement.SetText(CommasInNumber(value));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowRating(rating));
    }

    IEnumerator ShowRating(int rating)
    {
        foreach (RawImage image in ratingImages)
        {
            image.enabled = false;
        }
        for (int i = 0; i < Math.Min(ratingImages.Length, rating); i++)
        {
            ratingImages[i].enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
