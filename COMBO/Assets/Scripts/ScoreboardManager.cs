using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTextElement;
    public TextMeshProUGUI alertsTextElement;
    public TextMeshProUGUI comboTextElement;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetScore(Random.Range(0, 40).ToString());
    }

    void SetScore(string value)
    {
        scoreTextElement.SetText(value);
    }
    void SetAlerts(string value)
    {
        alertsTextElement.SetText(value);
    }
    void SetCombo(string value)
    {
        comboTextElement.SetText(value);
    }
}
