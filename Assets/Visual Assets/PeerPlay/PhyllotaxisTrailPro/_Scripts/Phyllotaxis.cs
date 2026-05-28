using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phyllotaxis : MonoBehaviour
{
    public AudioPeer _audioPeer;
    private Material _trailMat;
    public Color _trailColor;

    public float _degree, _scale;
    public int _numberStart;
    public int _stepSize;
    public int _maxIteration;

    //Lerping
    public bool _useLerping;
    private bool _isLerping;
    private Vector3 _startPosition, _endPosition;
    private float _lerpPosTimer, _lerpPosSpeed;
    public Vector2 _lerpPosSpeedMinMax;
    public AnimationCurve _lerpPosAnimCurve;
    [Range(0,7)]
    public int _lerpPosBand;

    private int _number;
    private int _currentIteration;
    private TrailRenderer _trailRenderer;
    private Vector2 CalculatePhyllotaxis(float degree, float scale, int number)
    {
        double angle = number * (degree * Mathf.Deg2Rad);
        float r = scale * Mathf.Sqrt(number);
        float x = r * (float)System.Math.Cos(angle);
        float y = r * (float)System.Math.Sin(angle);
        Vector2 vec2 = new Vector2(x, y);
        return vec2;
    }
    private Vector2 _phyllotaxisPosition;

    private bool _forward;
    public bool _repeat, _invert;

    //Scaling
    public bool _useScaleAnimation, _useScaleCurve;
    public Vector2 _scaleAnimMinMax;
    public AnimationCurve _scaleAnimCurve;
    public float _scaleAnimSpeed;
    [Range(0, 7)]
    public int _scaleBand;
    private float _scaleTimer, _currentScale;

     float _audioScaleFloat, _audioLerpPosFloat, _audioColorFloat;

    protected enum _audioScaleSetting { Band, BandBuffer, Amplitude, AmplitudeBuffer};
    [SerializeField]
    protected _audioScaleSetting audioScaleSetting = new _audioScaleSetting(); //inspector brush size selection

    protected enum _audioLerpPosSetting { Band, BandBuffer, Amplitude, AmplitudeBuffer };
    [SerializeField]
    protected _audioLerpPosSetting audioLerpPosSetting = new _audioLerpPosSetting(); //inspector brush size selection

    protected enum _audioColorSetting { Band, BandBuffer, Amplitude, AmplitudeBuffer };
    [SerializeField]
    protected _audioColorSetting audioColorSetting = new _audioColorSetting(); //inspector brush size selection

    //Color
    public bool _useColorOnAudio;
    public Vector2 _opacityMinMax;
    [Range(0, 7)]
    public int _colorBand;
    [Range(0, 1)]
    public float _colorThreshold;
    Color _audioColor;







    void SetLerpPositions()
    {
        _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _currentScale, _number);
        _startPosition = this.transform.localPosition;
        if (!float.IsNaN(_phyllotaxisPosition.x) && !float.IsNaN(_phyllotaxisPosition.y))
        {
            _endPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
        }
    }

    // Use this for initialization
    void Awake () {
        _currentScale = _scale;
        _forward = true;
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailMat = new Material(_trailRenderer.material);
        _trailMat.SetColor("_TintColor", _trailColor);
        _trailRenderer.material = _trailMat;
        _number = _numberStart;
        transform.localPosition = CalculatePhyllotaxis(_degree, _currentScale, _number);
        if (_useLerping)
        {
            _isLerping = true;
            SetLerpPositions();
        }
	}

    void SetAudioBands()
    {
        switch (audioScaleSetting)
        {
            case _audioScaleSetting.Band:
                _audioScaleFloat = _audioPeer._audioBand[_scaleBand];
                break;
            case _audioScaleSetting.BandBuffer:
                _audioScaleFloat = _audioPeer._audioBandBuffer[_scaleBand];
                break;
            case _audioScaleSetting.Amplitude:
                _audioScaleFloat = _audioPeer._Amplitude;
                break;
            case _audioScaleSetting.AmplitudeBuffer:
                _audioScaleFloat = _audioPeer._AmplitudeBuffer;
                break;
            default:
                _audioScaleFloat = _audioPeer._Amplitude;
                break;
        }

        switch (audioLerpPosSetting)
        {
            case _audioLerpPosSetting.Band:
                _audioLerpPosFloat = _audioPeer._audioBand[_lerpPosBand];
                break;
            case _audioLerpPosSetting.BandBuffer:
                _audioLerpPosFloat = _audioPeer._audioBandBuffer[_lerpPosBand];
                break;
            case _audioLerpPosSetting.Amplitude:
                _audioLerpPosFloat = _audioPeer._Amplitude;
                break;
            case _audioLerpPosSetting.AmplitudeBuffer:
                _audioLerpPosFloat = _audioPeer._AmplitudeBuffer;
                break;
            default:
                _audioLerpPosFloat = _audioPeer._Amplitude;
                break;
        }

        switch (audioColorSetting)
        {
            case _audioColorSetting.Band:
                _audioColorFloat = _audioPeer._audioBand[_colorBand];
                break;
            case _audioColorSetting.BandBuffer:
                _audioColorFloat = _audioPeer._audioBandBuffer[_colorBand];
                break;
            case _audioColorSetting.Amplitude:
                _audioColorFloat = _audioPeer._Amplitude;
                break;
            case _audioColorSetting.AmplitudeBuffer:
                _audioColorFloat = _audioPeer._AmplitudeBuffer;
                break;
            default:
                _audioColorFloat = _audioPeer._Amplitude;
                break;
        }

    }

    private void Update()
    {
        SetAudioBands();

        if (_useColorOnAudio)
        {
            if (_audioColorFloat > _colorThreshold)
            {
                float opacityEvaluate = Mathf.Lerp(_opacityMinMax.x, _opacityMinMax.y, _audioColorFloat);
                _audioColor = new Color(_trailColor.r, _trailColor.g, _trailColor.b, opacityEvaluate);
            }
            else
            {
                _audioColor = new Color(_trailColor.r, _trailColor.g, _trailColor.b, 0);
            }

            _trailMat.SetColor("_TintColor", _audioColor);

        }
        else
        {
            if (_trailMat.GetColor("_TintColor") != _trailColor)
            _trailMat.SetColor("_TintColor", _trailColor);
        }



        if (_useScaleAnimation)
        {
            if (_useScaleCurve)
            {
                _scaleTimer += (_scaleAnimSpeed * _audioScaleFloat) * Time.deltaTime;
                if (_scaleTimer >= 1)
                {
                    _scaleTimer -= 1;
                }
                _currentScale = Mathf.Lerp(_scaleAnimMinMax.x, _scaleAnimMinMax.y, _scaleAnimCurve.Evaluate(_scaleTimer));
            }
            else
            {
                _currentScale = Mathf.Lerp(_scaleAnimMinMax.x, _scaleAnimMinMax.y, _audioScaleFloat);
            }
        }


        if (_useLerping)
        {
            if (_isLerping)
            {
                _lerpPosSpeed = Mathf.Lerp(_lerpPosSpeedMinMax.x, _lerpPosSpeedMinMax.y, _lerpPosAnimCurve.Evaluate(_audioLerpPosFloat));
                _lerpPosTimer += Time.deltaTime * _lerpPosSpeed;
                transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, Mathf.Clamp01(_lerpPosTimer));
                if (_lerpPosTimer >= 1)
                {
                    _lerpPosTimer -= 1;
                    if (_forward)
                    {
                        _number += _stepSize;
                        _currentIteration++;
                    }
                    else
                    {
                        _number -= _stepSize;
                        _currentIteration--;
                    }
                    if ((_currentIteration >= 0) && (_currentIteration < _maxIteration))
                    {
                        SetLerpPositions();
                    }
                   else // current iteration has hit 0 or maxiteration
                    {
                        if (_repeat)
                        {
                            if (_invert)
                            {
                                _forward = !_forward;
                                SetLerpPositions();
                            }
                            else
                            {
                                _number = _numberStart;
                                _currentIteration = 0;
                                SetLerpPositions();
                            }
                        }
                        else
                        {
                            _isLerping = false;
                        }
                    }
                }

            }
        }
        if (!_useLerping)
        {
            _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _currentScale, _number);
            transform.localPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
            _number += _stepSize;
            _currentIteration++;
        }
    }

}
