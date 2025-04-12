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
    [SerializeField] private CameraZoom CZ;
    [SerializeField] private PulseToTheBeat PTTB;
    [SerializeField] private BeatFlickeringScript BFS;

    public void StartLaunchObstacles()
    {
        if (UFMS != null) UFMS.isTurnOn = true;
        RM.StartWithSync();  //Запускаем RhythmManager с синхронизацией с бпм музыки
        BC.isTurnOn = true;
        CZ.isTurnOn = true;
        PTTB.isTurnOn = true;
        BFS.isTurnOn = true;
        TurnOnFaceScripts(); 
    }

    private void TurnOnFaceScripts() //Запускаем каждую грань Икосферы
    {
        faces = FAS.GetAllFaceScripts();
        for (int i = 0; i < faces.Length; i++)
        {
            faces[i].isTurnOn = true;  
        }
    }
}
