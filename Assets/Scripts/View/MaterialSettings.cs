using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSettings", menuName = "ScriptableObjects/MaterialSettings")]
public class MaterialSettings : ScriptableObject
{ 
    [Header("PlayerPyramid")]
    public Material materialPlayerFrame;
    public Material materialPlayerGlowingPartTurnOn;
    public Material materialPlayerGlowingPartTurnOff;
    public Material materialPlayerGlowingPartDamage;
    public Material materialPlayerBeatFilter;
    [Header("PlayerSides")]
    public Material materialLeft;
    public Material materialRight;
    public Material materialTop;
    [Header("CommonFace")]
    public Material materialCommonFaceFrame;
    public Material materialCommonFaceGlowingPartTurnOn;
    public Material materialCommonFaceGlowingPartTurnOff;
    [Header("Obstacles")]
    public Material materialRedFace;
    public Material materialRedWave;
    public Material materialFallFace;
    [Header("Extra")]
    public Material materialCheckPoint;
    public Material materialBonus;
    public Material materialPortal;
    public Material materialShield;
    [Header("111")]
    public Material x;
}

