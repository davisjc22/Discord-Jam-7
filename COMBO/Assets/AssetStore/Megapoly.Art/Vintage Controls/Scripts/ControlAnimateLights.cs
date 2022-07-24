using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAnimateLights : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ButtonLamp> controls;
    private float interval;
    public float startTime = 1f;
    private int result;
    void Start()
    {
        interval = Random.Range(.5f, 1.5f);
        InvokeRepeating("Run", startTime, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Run()
    {
        foreach (ButtonLamp item in controls)
        {
            result = Random.Range(0, 2);
            item.on = intToBool(result);

        }
    }

    public static bool intToBool(int Number)
    {
        return (Number == 0 ? false : true);
    }
}
