using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaultCutSceneScript : MonoBehaviour
{
    [SerializeField] private LightShutDownScript LSDS;
    [SerializeField] private FaceArrayScript FAS;

    private void Start()
    {
        StartCoroutine(TurningOffBackground());
    }

    private IEnumerator TurningOffBackground() 
    {
        yield return new WaitForSeconds(5f);
        LSDS.StartFullShutDown();
        BreakingApartIcosahedron();
    }

    private void BreakingApartIcosahedron()
    {


    }
}
