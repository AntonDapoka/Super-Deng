using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObstaclesScript : MonoBehaviour
{
    private FaceScript[] faces;
    [SerializeField] private FaceArrayScript FAS;
    [SerializeField] private UnifiedFrameManagerScript UFMS;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private BeatController BC;
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
        BC.isTurnOn = true;
        RWS.isTurnOn = true;
        FM.isTurnOn = true;
        BSS.isTurnOn = true;
        PSS.isTurnOn = true;
        CZ.isTurnOn = true;
        PTTB.isTurnOn = true;
        TurnOnFaceScripts();
    }

    private void TurnOnFaceScripts()
    {
        faces = FAS.GetAllFaceScripts();
        for (int i = 0; i < faces.Length; i++)
        {
            faces[i].isTurnOn = true;  
        }
    }
}
