using UnityEngine;

public class CameraBeatZoomPresenterScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private CameraBeatZoomViewScript cameraBeatZoomView;
    [SerializeField] private float zoomFactor = 1.5f;  
    [SerializeField] private float zoomTime = 0.5f;  
    [SerializeField] private float returnTime = 0.2f;
    [SerializeField] private float beatImpulse = 0.25f;

    private void Start()
    {
        cameraBeatZoomView.SetZoomSettings(mainCamera, zoomFactor, zoomTime, returnTime, beatImpulse);
    }

    public void StartZooming(float beatInterval)
    {
        cameraBeatZoomView.StartZooming(beatInterval);
    }
}