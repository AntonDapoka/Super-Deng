using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceStateScript : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private bool _havePlayer = false;
    [SerializeField] private bool _transferInProgress = false;
    [SerializeField] private bool _isKilling = false;
    [SerializeField] private bool _isBlinking = false;
    [SerializeField] private bool _isColored = false;
    [SerializeField] private bool _isBlocked = false;
    [SerializeField] private bool _isPortal = false;
    [SerializeField] private bool _isBonus = false;
    [SerializeField] private bool _isTutorial = false;
    [SerializeField] private bool _isUpsideDown = false;

    [HideInInspector][SerializeField] private bool _isLeft = false;
    [HideInInspector][SerializeField] private bool _isRight = false;
    [HideInInspector][SerializeField] private bool _isTop = false;

    public bool HavePlayer { get => _havePlayer; set => _havePlayer = value; }
    public bool TransferInProgress { get => _transferInProgress; set => _transferInProgress = value; }
    public bool IsKilling { get => _isKilling; set => _isKilling = value; }
    public bool IsBlinking { get => _havePlayer; set => _havePlayer = value; }
    public bool IsColored { get => _havePlayer; set => _havePlayer = value; }
    public bool IsBlocked { get => _havePlayer; set => _havePlayer = value; }
    public bool IsPortal { get => _havePlayer; set => _havePlayer = value; }
    public bool IsBonus { get => _havePlayer; set => _havePlayer = value; }
    public bool IsTutorial { get => _havePlayer; set => _havePlayer = value; }
    public bool IsUpsideDown { get => _havePlayer; set => _havePlayer = value; }

    public bool IsLeft { get => _havePlayer; set => _havePlayer = value; }
    public bool IsRight { get => _havePlayer; set => _havePlayer = value; }
    public bool IsTop { get => _havePlayer; set => _havePlayer = value; }
}
