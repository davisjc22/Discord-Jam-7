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

    public bool isZooming = true;
    public float zoomInFOV = 33f;
    public float zoomOutFOV = 68f;
    private float zoomTarget;
    public float cameraTransitionDuration = 1f;
    Vector3 originalCameraPosition;
    void Start()
    {
        lights = FindObjectsOfType<Light>();
        Camera camera = FindObjectOfType<Camera>();
        cameraTransform = camera.GetComponent<Transform>();
        originalCameraPosition = cameraTransform.localPosition;
        foreach (Light light in lights)
        {
            if (light.gameObject.tag == "RoomLights")
            {
                light.intensity = standardIntensity;
            }
        }
        ZoomAwayFromScoreboard();
    }

    // Update is called once per frame
    void Update()
    {
        Flicker();
        CameraShake();
        ZoomCamera();
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
                if (light.CompareTag("RoomLights"))
                {
                    light.intensity = Random.Range(1, 100);
                }
            }
            flickerElapsed += Time.deltaTime;
        }
        if (flickerElapsed >= flickerDuration)
        {
            flickerElapsed = 0f;
            isFlickering = false;
            foreach (Light light in lights)
            {
                if (light.CompareTag("RoomLights"))
                {
                    light.intensity = standardIntensity;
                }
            }
        }
    }

    void ZoomCamera()
    {
        Camera cam = cameraTransform.gameObject.GetComponent<Camera>();
        if (isZooming)
        {
            float angle = Mathf.Abs(zoomInFOV - zoomOutFOV);
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, zoomTarget, angle / cameraTransitionDuration * Time.deltaTime);

        }
        if (Mathf.Abs(cam.fieldOfView - zoomTarget) < 0.1f)
        {
            isZooming = false;
        }
    }

    public void ZoomToScoreboard()
    {
        LeanTween.moveLocal(cameraTransform.gameObject, new Vector3(-0.5f, -9.5f, -7.5f), cameraTransitionDuration).setEase(LeanTweenType.easeInOutSine);
        LeanTween.rotateLocal(cameraTransform.gameObject, new Vector3(0f, 0f, 0f), cameraTransitionDuration).setEase(LeanTweenType.easeInOutSine);
        isZooming = true;
        zoomTarget = zoomInFOV;
    }
    public void ZoomAwayFromScoreboard()
    {
        if (cameraTransform == null)
        {
            Camera camera = FindObjectOfType<Camera>();
            cameraTransform = camera.GetComponent<Transform>();
        }
        LeanTween.moveLocal(cameraTransform.gameObject, new Vector3(-0.5f, -9.5f, -15.5f), cameraTransitionDuration).setEase(LeanTweenType.easeInOutSine);
        LeanTween.rotateLocal(cameraTransform.gameObject, new Vector3(22f, 0f, 0f), cameraTransitionDuration).setEase(LeanTweenType.easeInOutSine);
        isZooming = true;
        zoomTarget = zoomOutFOV;
    }
}
