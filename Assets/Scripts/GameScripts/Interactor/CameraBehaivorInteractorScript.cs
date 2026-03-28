using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaivorInteractorScript : MonoBehaviour
{
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private CameraBeatZoomPresenterScript cameraBeatZoomPresenter;

    public void ActivateBeatZoom()
    {
        cameraBeatZoomPresenter.StartZooming(rhythmManager.beatInterval);
    }
}
