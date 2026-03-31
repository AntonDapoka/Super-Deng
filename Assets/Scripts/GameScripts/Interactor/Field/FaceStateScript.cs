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
/*
    [SerializeField] private bool _debugHavePlayer;
    [SerializeField] private bool _debugIsKilling;
    [SerializeField] private bool _debugIsColored;
    [SerializeField] private bool _debugIsLeft;
    [SerializeField] private bool _debugIsRight;
    [SerializeField] private bool _debugIsTop;

    private void Update()
    {
        _debugHavePlayer = Get(FaceProperty.HavePlayer);
        _debugIsKilling = Get(FaceProperty.IsKilling);
        _debugIsColored = Get(FaceProperty.IsColored);
        _debugIsLeft = Get(FaceProperty.IsLeft);
        _debugIsRight = Get(FaceProperty.IsRight);
        _debugIsTop = Get(FaceProperty.IsTop);
    }*/
}