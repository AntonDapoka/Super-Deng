using System.Collections;
using UnityEngine;

public class CameraBeatZoomViewScript : MonoBehaviour
{
    private Camera mainCamera; 
    private float zoomFactor = 1.5f;  
    private float zoomTime = 0.5f;  
    private float returnTime = 0.2f;
    private float beatImpulse = 0.25f;

    private Coroutine zoomCoroutine;
    private float originalFOV; 
    private float targetFOV; 

    public void SetZoomSettings(Camera mainCamera, float zoomFactor, float zoomTime, float returnTime, float beatImpulse)
    {
        this.mainCamera = mainCamera;
        this.zoomFactor = zoomFactor;
        this.zoomTime = zoomTime;
        this.returnTime = returnTime;
        this.beatImpulse = beatImpulse;

        originalFOV = mainCamera.orthographicSize;
    }

    public void StartZooming(float beatInterval)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomCamera(beatInterval));
    }

    private IEnumerator ZoomCamera(float beatInterval)
    {
        float elapsedTime = 0f;
        targetFOV = originalFOV / zoomFactor;
        float zoomSpeed = (targetFOV - mainCamera.orthographicSize) / (zoomTime * beatInterval);

         mainCamera.orthographicSize += zoomSpeed * beatImpulse;

        float returnSpeed = (originalFOV - mainCamera.orthographicSize) / returnTime;

        while (elapsedTime < returnTime)
        {
            mainCamera.orthographicSize += returnSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize =  originalFOV;
    }
}
