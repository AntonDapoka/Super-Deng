using System;
using UnityEditor;
using UnityEngine;

public abstract class ActionSettingsEditor : Editor
{
    protected GUIStyle headerStyle;
    protected GUIStyle labelStyle;
    protected GUIStyle attentionStyle;
    protected GUIStyle hintStyle;

    protected bool stylesInitialized = false;

    private void InitStyles()
    {
        headerStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold
        };
        headerStyle.normal.textColor = Color.white;

        labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold
        };
        labelStyle.normal.textColor = Color.white;

        attentionStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 10,
            fontStyle = FontStyle.Italic
        };
        attentionStyle.normal.textColor = Color.red;

        hintStyle = new(EditorStyles.label)
        {
            fontSize = 10,
            fontStyle = FontStyle.Italic,
            wordWrap = true
        };
        hintStyle.normal.textColor = Color.green;

        headerStyle.normal.textColor = Color.white;
        labelStyle.normal.textColor = Color.white;
        attentionStyle.normal.textColor = Color.red;
        hintStyle.normal.textColor = Color.green;

        stylesInitialized = true;
    }

    public override void OnInspectorGUI()
    {
        if (!stylesInitialized) InitStyles();

        serializedObject.Update();
        SerializedProperty effectName = serializedObject.FindProperty("effectName");

        string name = GetActionStringName();

        EditorGUILayout.LabelField(name + " \"" + effectName.stringValue + "\"", headerStyle);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(effectName, new GUIContent("Name of the effect:"));

        SerializedProperty type = serializedObject.FindProperty("type");
        EditorGUILayout.PropertyField(type, new GUIContent("Action Type"));

        SetActionStandardSettings();

        serializedObject.ApplyModifiedProperties();
    }

    public abstract string GetActionStringName();

    private void SetActionStandardSettings()
    {
        SerializedProperty bpm = serializedObject.FindProperty("bpm");
        SerializedProperty isHint = serializedObject.FindProperty("isHint");

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(bpm, new GUIContent("Track BPM"));
        bool changedBPM = EditorGUI.EndChangeCheck();

        EditorGUILayout.PropertyField(isHint, new GUIContent("Turn On Hints?"));

        AddSettingsSection("Time Settings:", Color.black, () =>
        {
            SetStartTime(bpm.floatValue, changedBPM, isHint.boolValue);
            SetEndTime(bpm.floatValue, changedBPM, isHint.boolValue);
            SetForcedBreakTime(bpm.floatValue, changedBPM, isHint.boolValue);
        });

        SetActionSpecialSettings(bpm.floatValue, changedBPM, isHint.boolValue);
    }

    public abstract void SetActionSpecialSettings(float bpm, bool changedBPM, bool isHint);

    protected void AddSettingsSection(string title, Color color, Action content)
    {
        EditorGUILayout.LabelField(title, labelStyle);
        BeginColoredBox(color);

        content?.Invoke();

        DrawSeparator();
        EndColoredBox();
        EditorGUILayout.Space();
    }

    private void SetStartTime(float bpm, bool changedBPM, bool isHint)
    {
        SerializedProperty timeStartSeconds = serializedObject.FindProperty("timeStartSeconds");
        SerializedProperty timeStartBeats = serializedObject.FindProperty("timeStartBeats");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(timeStartSeconds, new GUIContent("Start Time (seconds)"));
        bool changedStartSeconds = EditorGUI.EndChangeCheck();

        if (isHint)
            EditorGUILayout.HelpBox("Время начала этого эффекта, указывается в секундах. Эффект станет активным на следующем бите трека после указанного времени.", MessageType.Info);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(timeStartBeats, new GUIContent("Start Time (beats)"));
        bool changedStartBeats = EditorGUI.EndChangeCheck();

        if (isHint)
            EditorGUILayout.HelpBox("Время начала этого эффекта, указывается в битах. Эффект станет активным на этом конкретном бите трека.", MessageType.Info);

        if (changedStartSeconds || changedBPM)
        {
            timeStartBeats.floatValue = timeStartSeconds.floatValue * bpm / 60f;
        }
        else if ((changedStartBeats || changedBPM) && bpm != 0f)
        {
            timeStartSeconds.floatValue = timeStartBeats.floatValue * 60f / bpm;
        }
    }

    private void SetEndTime(float bpm, bool changedBPM, bool isHint)
    {
        SerializedProperty isTimeEnd = serializedObject.FindProperty("isTimeEnd");
        SerializedProperty timeEndSeconds = serializedObject.FindProperty("timeEndSeconds");
        SerializedProperty timeEndBeats = serializedObject.FindProperty("timeEndBeats");

        EditorGUILayout.PropertyField(isTimeEnd, new GUIContent("Does Effect have an End?"));
        if (isHint)
            EditorGUILayout.HelpBox("Если данный эффект не имеет конца, то в случае указания конкретных граней он сработает единоразово, в случае рандомного спавна он будет активным до истечения таймера. Если конец указан, то и рандомные, и конкретные грани будут вызываться до указанного времени", MessageType.Info);
        if (isTimeEnd.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeEndSeconds, new GUIContent("End Time (seconds)"));
            bool changedEndSeconds = EditorGUI.EndChangeCheck();

            if (isHint)
                EditorGUILayout.HelpBox("Время конца этого эффекта, указывается в секундах. После назначенного времени эффект вызываться не будет", MessageType.Info);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeEndBeats, new GUIContent("End Time (beats)"));
            bool changedEndBeats = EditorGUI.EndChangeCheck();

            if (isHint)
                EditorGUILayout.HelpBox("Время конца этого эффекта, указывается в битах. После конкретного бита эффект вызываться не будет", MessageType.Info);

            if (changedEndSeconds || changedBPM)
            {
                timeEndBeats.floatValue = timeEndSeconds.floatValue * bpm / 60f;
            }
            else if ((changedEndBeats || changedBPM) && bpm != 0f)
            {
                timeEndSeconds.floatValue = timeEndBeats.floatValue * 60f / bpm;
            }
        }
    }

    private void SetForcedBreakTime(float bpm, bool changedBPM, bool isHint)
    {
        SerializedProperty isTimeForcedBreak = serializedObject.FindProperty("isTimeForcedBreak");
        SerializedProperty timeForcedBreakSeconds = serializedObject.FindProperty("timeForcedBreakSeconds");
        SerializedProperty timeForcedBreakBeats = serializedObject.FindProperty("timeForcedBreakBeats");

        EditorGUILayout.PropertyField(isTimeForcedBreak, new GUIContent("Does Effect have a Forced Break?"));

        if (isHint)
            EditorGUILayout.HelpBox("Форсированный брейк отличается от обычного заканчивания тем, что отключает все Action в любой фазе, то есть на следующий бит на поле от них не останется следа", MessageType.Info);

        if (isTimeForcedBreak.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeForcedBreakSeconds, new GUIContent("End Time (seconds)"));
            bool changedForcedBreakSeconds = EditorGUI.EndChangeCheck();

            if (isHint)
                EditorGUILayout.HelpBox("Время форсированной остановки этого эффекта, указывается в секундах. После назначенного времени эффект исчезнет в течение одного бита в независимости от любоых обстоятельств", MessageType.Info);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeForcedBreakBeats, new GUIContent("End Time (beats)"));
            bool changedForcedBreakBeats = EditorGUI.EndChangeCheck();

            if (isHint)
                EditorGUILayout.HelpBox("Время форсированной остановки этого эффекта, указывается в битах. После назначенного времени эффект исчезнет в течение одного бита в независимости от любоых обстоятельств", MessageType.Info);

            if (changedForcedBreakSeconds || changedBPM)
            {
                timeForcedBreakBeats.floatValue = timeForcedBreakSeconds.floatValue * bpm / 60f;
            }
            else if ((changedForcedBreakBeats || changedBPM) && bpm != 0f)
            {
                timeForcedBreakSeconds.floatValue = timeForcedBreakBeats.floatValue * 60f / bpm;
            }
        }
    }

    private void DrawSeparator(float thickness = 1, float padding = 10)
    {
        var rect = EditorGUILayout.GetControlRect(false, padding + thickness);
        rect.height = thickness;
        rect.y += padding / 2f;
        rect.x -= 2;
        rect.width += 6;

        EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 1));
    }

    private void BeginColoredBox(Color color)
    {
        var c = GUI.color;
        GUI.color = color;
        EditorGUILayout.BeginVertical("box");
        GUI.color = c;
    }

    private void EndColoredBox()
    {
        EditorGUILayout.EndVertical();
    }
}
