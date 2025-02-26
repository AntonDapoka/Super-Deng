using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Phyllotaxis)), CanEditMultipleObjects]
public class PhyllotaxisEditor : Editor
{
    GUIStyle boxStyle;
    Phyllotaxis _targetScript;

    SerializedObject _phyllotaxis;
    SerializedProperty _audioPeer;
    SerializedProperty _trailColor;
    SerializedProperty _degree;
    SerializedProperty _scale;
    SerializedProperty _numberStart;
    SerializedProperty _stepSize;
    SerializedProperty _maxIteration;
 
     bool _setupFoldout, _lerpPosFoldout, _scaleFoldout, _colorFoldout;

    SerializedProperty _useLerping;
    SerializedProperty _lerpPosSpeedMinMax;
    SerializedProperty _lerpPosAnimCurve;
    SerializedProperty _lerpPosBand;
    SerializedProperty _repeat;
    SerializedProperty _invert;
    SerializedProperty audioLerpPosSetting;

    SerializedProperty _useScaleAnimation;
    SerializedProperty _useScaleCurve;
    SerializedProperty _scaleAnimMinMax;
    SerializedProperty _scaleAnimCurve;
    SerializedProperty _scaleAnimSpeed;
    SerializedProperty _scaleBand;
    SerializedProperty audioScaleSetting;

    SerializedProperty _useColorOnAudio;
    SerializedProperty _opacityMinMax;
    SerializedProperty _colorBand;
    SerializedProperty _colorThreshold;
    SerializedProperty audioColorSetting;

    

    public override void OnInspectorGUI()
    {

        serializedObject.Update();



        if (boxStyle == null) { boxStyle = GUI.skin.FindStyle("box"); }

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Phyllotaxis Trail Pro", EditorStyles.largeLabel, GUILayout.Height(20));
        EditorGUILayout.LabelField("Peer Play", EditorStyles.largeLabel, GUILayout.Height(20));
        EditorGUILayout.Separator();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Setup", EditorStyles.toolbarDropDown)) { _setupFoldout = !_setupFoldout; }
        if (_setupFoldout)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_audioPeer, new GUIContent("AudioPeer", "Select the AudioPeer object"));
            EditorGUILayout.PropertyField(_trailColor, new GUIContent("Trail Color", "Select the trail color and opacity"));
            EditorGUILayout.PropertyField(_degree, new GUIContent("Degree", "Select the degree of change for each iteration"));
            EditorGUILayout.PropertyField(_scale, new GUIContent("Scale", "Select scale in between of iterations"));
            EditorGUILayout.PropertyField(_numberStart, new GUIContent("Number Start", "Select the starting number"));
            EditorGUILayout.PropertyField(_stepSize, new GUIContent("Step Size", "Select the step size"));
            EditorGUILayout.PropertyField(_maxIteration, new GUIContent("Max Iteration", "Select the max iterations"));
        }

        if (GUILayout.Button("Lerp Position", EditorStyles.toolbarDropDown)) { _lerpPosFoldout = !_lerpPosFoldout; }
        if (_lerpPosFoldout)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_useLerping, new GUIContent("Use Lerping", "Wheither to use or not use lerping. If not, trail skips to next position"));
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(audioLerpPosSetting, new GUIContent("Audio Frequency", "Select the audio frequency to use"));
            EditorGUILayout.PropertyField(_lerpPosBand, new GUIContent("Audio Band", "Select the audio band to read frequency from 0 = deep bass, 7 = very high frequencies"));
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(_lerpPosSpeedMinMax, new GUIContent("Speed Min/Max", "Select the Min & Max speed of lerping"));
            EditorGUILayout.PropertyField(_lerpPosAnimCurve, new GUIContent("Interpolation Curve", "How to interpolate between minimum and maximum speed, based on frequencies"));
            EditorGUILayout.PropertyField(_repeat, new GUIContent("Repeat", "Repeat or stop updating positions after max iterations have been hit"));
            EditorGUILayout.PropertyField(_invert, new GUIContent("Invert", "Count backwards, or restart at zero"));

                
        }

        if (GUILayout.Button("Scale", EditorStyles.toolbarDropDown)) { _scaleFoldout = !_scaleFoldout; }
        if (_scaleFoldout)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_useScaleAnimation, new GUIContent("Use Scaling", "Wheither to use or not use scaling on audio"));
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(audioScaleSetting, new GUIContent("Audio Frequency", "Select the audio frequency to use"));
            EditorGUILayout.PropertyField(_scaleBand, new GUIContent("Audio Band", "Select the audio band to read frequency from 0 = deep bass, 7 = very high frequencies"));
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(_scaleAnimMinMax, new GUIContent("Scale Min/Max", "Select the Min & Max scale"));
            EditorGUILayout.PropertyField(_useScaleCurve, new GUIContent("Use Scale Curve", "ENABLED: interpolates between min/max with certain speed. DISABLED: scale directly to current frequency"));

            EditorGUILayout.PropertyField(_scaleAnimCurve, new GUIContent("Interpolation Curve", "How to interpolate between minimum and maximum scale, based on frequencies, with a certain speed"));
            EditorGUILayout.PropertyField(_scaleAnimSpeed, new GUIContent("Interpolation Speed", "speed of interpolation if UseScaleCurve is enabled"));

        }

        if (GUILayout.Button("Color", EditorStyles.toolbarDropDown)) { _colorFoldout = !_colorFoldout; }
        if (_colorFoldout)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_useColorOnAudio, new GUIContent("Use Color", "Wheither to use or not use color on audio"));
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(audioColorSetting, new GUIContent("Audio Frequency", "Select the audio frequency to use"));
            EditorGUILayout.PropertyField(_colorBand, new GUIContent("Audio Band", "Select the audio band to read frequency from 0 = deep bass, 7 = very high frequencies"));
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_opacityMinMax, new GUIContent("Opacity Min/Max", "0 = transparent, 1 = opaque"));
            EditorGUILayout.PropertyField(_colorThreshold, new GUIContent("Color Threshold", "The threshold on which the color changes, and below turns invisible"));
        }
       
        for (int i = 0; i < targets.Length; i++)

        {
            serializedObject.ApplyModifiedProperties();
        }
        serializedObject.ApplyModifiedProperties();
        //_phyllotaxis.ApplyModifiedProperties();

    }

  
    public void OnEnable()
    {
        Initialize();

    }

    public void Initialize()
    {
        _setupFoldout = true;
        _colorFoldout = true;
        _lerpPosFoldout = true;
        _scaleFoldout = true;
   

        _audioPeer = serializedObject.FindProperty("_audioPeer");
        _trailColor = serializedObject.FindProperty("_trailColor");
        _degree = serializedObject.FindProperty("_degree");
        _scale = serializedObject.FindProperty("_scale");
        _numberStart = serializedObject.FindProperty("_numberStart");
        _stepSize = serializedObject.FindProperty("_stepSize");
        _maxIteration = serializedObject.FindProperty("_maxIteration");


        _useLerping = serializedObject.FindProperty("_useLerping");
        audioLerpPosSetting = serializedObject.FindProperty("audioLerpPosSetting");
        _lerpPosBand = serializedObject.FindProperty("_lerpPosBand");
        _lerpPosSpeedMinMax = serializedObject.FindProperty("_lerpPosSpeedMinMax");
        _lerpPosAnimCurve = serializedObject.FindProperty("_lerpPosAnimCurve");
        _repeat = serializedObject.FindProperty("_repeat");
        _invert = serializedObject.FindProperty("_invert");

        _useScaleAnimation = serializedObject.FindProperty("_useScaleAnimation");
        _useScaleCurve = serializedObject.FindProperty("_useScaleCurve");
        _scaleAnimMinMax = serializedObject.FindProperty("_scaleAnimMinMax");
        _scaleAnimCurve = serializedObject.FindProperty("_scaleAnimCurve");
        _scaleAnimSpeed = serializedObject.FindProperty("_scaleAnimSpeed");
        _scaleBand = serializedObject.FindProperty("_scaleBand");
        audioScaleSetting = serializedObject.FindProperty("audioScaleSetting");

        _useColorOnAudio = serializedObject.FindProperty("_useColorOnAudio");
        _opacityMinMax = serializedObject.FindProperty("_opacityMinMax");
        _colorBand = serializedObject.FindProperty("_colorBand");
        _colorThreshold = serializedObject.FindProperty("_colorThreshold");
        audioColorSetting = serializedObject.FindProperty("audioColorSetting");
    }
}
