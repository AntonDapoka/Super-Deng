using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public bool isTurnOn = false;
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private float zoomFactor = 1.5f;  
    [SerializeField] private float zoomTime = 0.5f;  
    [SerializeField] private float returnTime = 0.2f;
    [SerializeField] private RhythmManager RM;
    private Coroutine zoomCoroutine;
    private float originalFOV; 
    private float targetFOV; 
    

    private void Start()
    {
        originalFOV = mainCamera.orthographicSize;
    }

    public void StartZooming()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCamera());
    }

    private IEnumerator ZoomCamera()
    {
        if (isTurnOn)
        {
            targetFOV = originalFOV / zoomFactor;
            
            float zoomSpeed = (targetFOV - mainCamera.orthographicSize) / (zoomTime * (RM.bpm / 60f));
            float elapsedTime = 0f;
            /*
            while (elapsedTime < zoomTime)
            {
                mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }*/
            mainCamera.orthographicSize += zoomSpeed * 0.25f;




            float returnSpeed = (originalFOV - mainCamera.orthographicSize) / returnTime;
            elapsedTime = 0f;

            while (elapsedTime < returnTime)
            {
                mainCamera.orthographicSize += returnSpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mainCamera.orthographicSize = originalFOV;
        }
    }
}