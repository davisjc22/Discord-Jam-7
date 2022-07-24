using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNav : MonoBehaviour
{
[SerializeField]
    private Transform[] wayPoints;
    private Vector3 cityPostion;
    private Vector3 waypointPostion;
    private int waypointIdx = 1;
    public float speed = 1.0f;
    private float progress = 0f;

    void Start()
    {
        gameObject.transform.position = wayPoints[0].transform.position;
        waypointPostion = wayPoints[1].transform.position;
        cityPostion     = wayPoints[0].transform.position;
    }

    void Update()
    {
        progress += (Time.deltaTime * speed) / Vector3.Distance(cityPostion,waypointPostion);
        gameObject.transform.position = Vector3.Lerp(cityPostion, waypointPostion, progress);
        if(progress >= 1f)
        {
            nextWaypoint();
        }
    }

    void nextWaypoint()
    {
        // transform.RotateAround(wayPoints[waypointIdx].transform.position, Vector3.up, 90);
        waypointIdx = (waypointIdx + 1) % wayPoints.Length;
        cityPostion = gameObject.transform.position;
        waypointPostion = wayPoints[waypointIdx].transform.position;
        progress = 0f;
    }

}
