using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 This script is pretty limited. Just used to show that we can make a flicker happen on these lights
*/
public class Flicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.Find("NE");
        go.GetComponent<Light>().intensity = Random.Range(0, 200);
    }
}
