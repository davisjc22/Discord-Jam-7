using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleOrbitter : MonoBehaviour
{
    public GameObject floor;
    public int speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, floor.transform.up, speed * Time.deltaTime);
    }
}
