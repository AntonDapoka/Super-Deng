using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObstaclesScript : MonoBehaviour
{
    [SerializeField] private UnifiedFrameManagerScript UFMS;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private RedWaveScript RWS;
    [SerializeField] private FallManager FM;
    [SerializeField] private BonusSpawnerScript BSS;
    [SerializeField] private PortalSpawnerScript PSS;
    [SerializeField] private CameraZoom CZ;
    [SerializeField] private PulseToTheBeat PTTB;

    public void StartLaunchObstacles()
    {
        UFMS.isTurnOn = true;
        RM.StartWithSync();
        RFS.isTurnOn = true;
        RWS.isTurnOn = true;
        FM.isTurnOn = true;
        BSS.isTurnOn = true;
        PSS.isTurnOn = true;
        CZ.isTurnOn = true;
        PTTB.isTurnOn = true;
    }
}
