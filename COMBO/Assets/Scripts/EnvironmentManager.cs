using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // Start is called before the first frame update
    Light[] lights;
    public bool isFlickering = false;
    public bool isShaking = false;
    private float flickerElapsed = 0;
    public float flickerDuration = 2f;
    private int standardIntensity = 200;
    Transform cameraTransform;
    public float shakeDuration = 2f;
    private float shakeElapsed = 0f;

    public float shakeAmount = 0.2f;
    Vector3 originalCameraPosition;
    void Start()
    {
        lights = FindObjectsOfType<Light>();
        Camera camera = FindObjectOfType<Camera>();
        cameraTransform = camera.GetComponent<Transform>();
        originalCameraPosition = cameraTransform.localPosition;
        foreach (Light light in lights)
        {
            light.intensity = standardIntensity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Flicker();
        CameraShake();
    }

    void CameraShake()
    {
        if (isShaking)
        {
            cameraTransform.localPosition = originalCameraPosition + Random.insideUnitSphere * shakeAmount;
            shakeElapsed += Time.deltaTime;

            if (shakeElapsed >= shakeDuration)
            {
                shakeElapsed = 0f;
                isShaking = false;
                cameraTransform.localPosition = originalCameraPosition;
            }
        }

    }
    void Flicker()
    {
        if (isFlickering)
        {
            foreach (Light light in lights)
            {
                light.intensity = Random.Range(1, 100);
            }
            flickerElapsed += Time.deltaTime;
        }
        if (flickerElapsed >= flickerDuration)
        {
            flickerElapsed = 0f;
            isFlickering = false;
            foreach (Light light in lights)
            {
                light.intensity = standardIntensity;
            }
        }
    }
}
