using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceStateScript : MonoBehaviour
{
    [Header("Questions")]
    public bool havePlayer = false;
    private bool transferInProgress = false;
    public bool isKilling = false;
    public bool isBlinking = false;
    public bool isColored = false;
    public bool isBlocked = false;
    public bool isPortal = false;
    public bool isBonus = false;
    public bool isTutorial = false;
    public bool isUpsideDown = false;
    [HideInInspector] public bool isLeft = false;
    [HideInInspector] public bool isRight = false;
    [HideInInspector] public bool isTop = false;


    public bool GetHavePlayerBool()
    {
        return havePlayer;
    }

    public void SetHavePlayerBool(bool newValue)
    {
        havePlayer = newValue;
    }
}
