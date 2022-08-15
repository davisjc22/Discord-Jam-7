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
    public RawImage[] lifeImages;
    public RawImage[] ratingImages;
    private bool timerEnabled;
    private float timerCounter = 0f;
    private bool isGameOver = false;
    private int score = 0;
    private int combo = 1;
    private int rating = 0;
    private int numLives = 0;


    public enum Screen
    {
        GAMEPLAY,
        INFO,
        RESULTS
    }

    private Screen activeScreen;
    private GameObject EnumToScreen(Screen screen)
    {
        switch (screen)
        {
            case Screen.GAMEPLAY:
                return gameplayScreen;
            case Screen.INFO:
                return infoScreen;
            case Screen.RESULTS:
                return resultsScreen;
            default:
                return gameplayScreen;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        ShowScreen(Screen.INFO);
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
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < numLives)
            {
                lifeImages[i].enabled = true;
            }
            else
            {
                lifeImages[i].enabled = false;
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
    public LTDescr ShowScreen(Screen screen)
    {
        GameObject screenObject = EnumToScreen(screen);
        screenObject.transform.position = new Vector3(screenObject.transform.position.x, 800, screenObject.transform.position.z);
        activeScreen = screen;
        return LeanTween.moveLocalY(screenObject, 0, 0.8f).setEase(LeanTweenType.easeInOutQuad).setDelay(0.5f);
    }
    public void HideScreen(Screen screen)
    {
        GameObject screenObject = EnumToScreen(screen);
        LeanTween.moveLocalY(screenObject, screenObject.transform.position.y - 30, 0.2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
         {
             LeanTween.moveLocalY(screenObject, -800, 1.0f).setEase(LeanTweenType.easeInOutQuad);
         });
    }
    public LTDescr ReplaceScreen(Screen screen)
    {
        GameObject screenObject = EnumToScreen(screen);
        HideScreen(activeScreen);
        return ShowScreen(screen);
    }
    public void ShowGameOver()
    {
        SetTimerEnabled(false);
        foreach (RawImage image in lifeImages)
        {
            image.enabled = false;
        }
        foreach (RawImage image in ratingImages)
        {
            image.enabled = false;
        }
        float maximumPPS = 2000f / 60f;
        int _rating = (int)Mathf.Max(Mathf.Min(8f * ((score / timerCounter) / maximumPPS), 8f), 1f);
        SetRating(_rating);
        gameOverScoreTextElement.SetText("0");
        gameOverTimeTextElement.SetText("0:00");
        ReplaceScreen(Screen.RESULTS).setOnComplete(() =>
        {
            StartCoroutine(TallyTimer((int)timerCounter));

        });
    }
    public void ShowGamePlay(bool firstTime)
    {
        StopAllCoroutines();
        if (firstTime)
        {
            ShowScreen(Screen.INFO);
        }
        else
        {
            ReplaceScreen(Screen.GAMEPLAY);
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
        ShowScreen(Screen.GAMEPLAY);
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
