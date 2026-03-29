using System.Collections.Generic;
using UnityEngine;

public enum FaceProperty
{
    HavePlayer,
    TransferInProgress,
    IsKilling,
    IsBlinking,
    IsColored,
    IsBlocked,
    IsPortal,
    IsBonusCombo,
    IsBonusShield,
    IsBonusHealth,
    IsLeft,
    IsRight,
    IsTop
}

public class FaceStateScript : MonoBehaviour
{
    private readonly Dictionary<FaceProperty, bool> _boolStates = new();

    public bool Get(FaceProperty prop) => _boolStates.TryGetValue(prop, out bool value) && value;
    public void Set(FaceProperty prop, bool value) => _boolStates[prop] = value;
}