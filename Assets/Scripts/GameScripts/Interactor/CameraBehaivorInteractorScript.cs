using UnityEngine;

public class CameraBehaivorInteractorScript : MonoBehaviour, IBeatUpdate
{
    [SerializeField] private CameraBeatZoomPresenterScript cameraBeatZoomPresenter;
    private float beatInterval;

    private bool isTurnOn = false;

    public void InitializeCamera(float beatInterval)
    {
        //SomeSettings
        this.beatInterval = beatInterval;
    }

    public void OnBeat()
    {
        if (isTurnOn)
            cameraBeatZoomPresenter.StartZooming(beatInterval);
    }

    public void TurnOn()
    {
        isTurnOn = true;
    }

    public void TurnOff()
    {
        isTurnOn = false;
    }
}
