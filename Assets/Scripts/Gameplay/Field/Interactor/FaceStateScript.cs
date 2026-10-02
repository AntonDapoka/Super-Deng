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
    IsBonus,
    IsLeft,
    IsRight,
    IsTop
}

public class FaceStateScript : MonoBehaviour
{
    private readonly Dictionary<FaceProperty, bool> _boolStates = new();
    private readonly Dictionary<BonusType, bool> _boolBonusTypes = new();

    public bool GetFaceState(FaceProperty prop) => _boolStates.TryGetValue(prop, out bool value) && value;
    public void SetFaceState(FaceProperty prop, bool value) => _boolStates[prop] = value;

    public bool GetBonusType(BonusType prop) => _boolBonusTypes.TryGetValue(prop, out bool value) && value;
    public void SetBonusType(BonusType prop, bool value) => _boolBonusTypes[prop] = value;
/*
    [SerializeField] private bool _debugHavePlayer;
    [SerializeField] private bool _debugIsKilling;
    [SerializeField] private bool _debugIsColored;
    [SerializeField] private bool _debugIsLeft;
    [SerializeField] private bool _debugIsRight;
    [SerializeField] private bool _debugIsTop;

    private void Update()
    {
        _debugHavePlayer = GetFaceState(FaceProperty.HavePlayer);
        _debugIsKilling = GetFaceState(FaceProperty.IsKilling);
        _debugIsColored = GetFaceState(FaceProperty.IsColored);
        _debugIsLeft = GetFaceState(FaceProperty.IsLeft);
        _debugIsRight = GetFaceState(FaceProperty.IsRight);
        _debugIsTop = GetFaceState(FaceProperty.IsTop);
    }*/
}