using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RedFaceSettings))]
public class RedFaceSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty effectName = serializedObject.FindProperty("effectName");

        GUIStyle headerStyle = new(EditorStyles.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold
        };
        headerStyle.normal.textColor = Color.white;
        GUIStyle labelStyle = new(EditorStyles.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold
        };
        labelStyle.normal.textColor = Color.white;
        GUIStyle attentionStyle = new(EditorStyles.label)
        {
            fontSize = 10,
            fontStyle = FontStyle.Italic
        };
        attentionStyle.normal.textColor = Color.red;

        EditorGUILayout.LabelField("Red Face Effect \"" + effectName.stringValue + "\"", headerStyle);
        EditorGUILayout.PropertyField(effectName, new GUIContent("Name of the effect:"));

        SetRedFaces(labelStyle, attentionStyle);

        serializedObject.ApplyModifiedProperties();
    }

    private void SetRedFaces(GUIStyle labelStyle, GUIStyle attentionStyle)
    {
        SerializedProperty bpm = serializedObject.FindProperty("bpm");
        EditorGUILayout.Space();


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(bpm, new GUIContent("Track BPM"));
        bool changedBPM = EditorGUI.EndChangeCheck();


        EditorGUILayout.LabelField("Time Settings:", labelStyle);

        BeginColoredBox(Color.red);

        SetStartTime(bpm.floatValue, changedBPM);
        SetEndTime(bpm.floatValue, changedBPM);
        DrawSeparator();
        EndColoredBox();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Face Settings:", labelStyle);
        BeginColoredBox(Color.blue);

        SetRandom();
        SetCertain();
        DrawSeparator();
        EndColoredBox();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Limit Settings:", labelStyle);
        BeginColoredBox(Color.green);

        SetProximityAndDistanceLimit();
        DrawSeparator();
        EndColoredBox();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Basic Settings:", labelStyle);
        BeginColoredBox(Color.cyan);
        SetBasicSettings(attentionStyle, bpm.floatValue, changedBPM);
        DrawSeparator();
        EndColoredBox();
        
    }

    private void SetStartTime(float bpm, bool changedBPM)
    {

        SerializedProperty timeStartSeconds = serializedObject.FindProperty("timeStartSeconds");
        SerializedProperty timeStartBeats = serializedObject.FindProperty("timeStartBeats");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(timeStartSeconds, new GUIContent("Start Time (seconds)"));
        bool changedStartSeconds = EditorGUI.EndChangeCheck();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(timeStartBeats, new GUIContent("Start Time (beats)"));
        bool changedStartBeats = EditorGUI.EndChangeCheck();

        if (changedStartSeconds || changedBPM)
        {
            timeStartBeats.floatValue = timeStartSeconds.floatValue * bpm / 60f;
        }
        else if ((changedStartBeats || changedBPM) && bpm != 0f)
        {
            timeStartSeconds.floatValue = timeStartBeats.floatValue * 60f / bpm;
        }
    }

    private void SetEndTime(float bpm, bool changedBPM)
    {
        SerializedProperty isTimeEnd = serializedObject.FindProperty("isTimeEnd");
        SerializedProperty timeEndSeconds = serializedObject.FindProperty("timeEndSeconds");
        SerializedProperty timeEndBeats = serializedObject.FindProperty("timeEndBeats");

        EditorGUILayout.PropertyField(isTimeEnd, new GUIContent("Does Effect have an End?"));
        if (isTimeEnd.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeEndSeconds, new GUIContent("End Time (seconds)"));
            bool changedEndSeconds = EditorGUI.EndChangeCheck();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeEndBeats, new GUIContent("End Time (beats)"));
            bool changedEndBeats = EditorGUI.EndChangeCheck();

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

    private void SetRandom()
    {
        SerializedProperty isRandom = serializedObject.FindProperty("isRandom");
        SerializedProperty isStableQuantity = serializedObject.FindProperty("isStableQuantity");
        SerializedProperty quantityExact = serializedObject.FindProperty("quantityExact");
        SerializedProperty quantityMin = serializedObject.FindProperty("quantityMin");
        SerializedProperty quantityMax = serializedObject.FindProperty("quantityMax");

        EditorGUILayout.PropertyField(isRandom, new GUIContent("Is It Random?"));
        if (isRandom.boolValue)
        {
            EditorGUILayout.PropertyField(isStableQuantity, new GUIContent("Is Quantity Always Stable?"));

            if (isStableQuantity.boolValue)
            {
                EditorGUILayout.PropertyField(quantityExact, new GUIContent("Exact Quantity"));
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(quantityMin, new GUIContent("Minimum Quantity"));
                bool changedQuantityMin = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(quantityMax, new GUIContent("Maximum Quantity"));
                bool changedQuantityMax = EditorGUI.EndChangeCheck();

                if (changedQuantityMin && quantityMin.intValue > quantityMax.intValue)
                {
                    quantityMax.intValue = quantityMin.intValue;
                }
                else if (changedQuantityMax && quantityMin.intValue > quantityMax.intValue)
                {
                    quantityMin.intValue = quantityMax.intValue;
                }
            }

        }
    }

    private void SetCertain()
    {
        SerializedProperty isCertain = serializedObject.FindProperty("isCertain");
        SerializedProperty isRelativeToPlayer = serializedObject.FindProperty("isRelativeToPlayer");
        SerializedProperty arrayOfFacesRelativeToPlayer = serializedObject.FindProperty("arrayOfFacesRelativeToPlayer");
        SerializedProperty isRelativeToFigure = serializedObject.FindProperty("isRelativeToFigure");
        SerializedProperty arrayOfFacesRelativeToFigure = serializedObject.FindProperty("arrayOfFacesRelativeToFigure");

        EditorGUILayout.PropertyField(isCertain, new GUIContent("Is It Certain?"));
        if (isCertain.boolValue)
        {
            EditorGUILayout.PropertyField(isRelativeToFigure, new GUIContent("Is It Relative To Figure?"));
            if (isRelativeToFigure.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfFacesRelativeToFigure, new GUIContent("Array Of Faces Relative To Figure"));
            }

            EditorGUILayout.PropertyField(isRelativeToPlayer, new GUIContent("Is It Relative To Player?"));
            if (isRelativeToPlayer.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfFacesRelativeToPlayer, new GUIContent("Array Of Faces Relative To Player"));
            }
        }
    }
    private void SetProximityAndDistanceLimit()
    {
        SerializedProperty isProximityLimit = serializedObject.FindProperty("isProximityLimit");
        SerializedProperty proximityLimit = serializedObject.FindProperty("proximityLimit");
        SerializedProperty isDistanceLimit = serializedObject.FindProperty("isDistanceLimit");
        SerializedProperty distanceLimit = serializedObject.FindProperty("distanceLimit");

        EditorGUILayout.PropertyField(isProximityLimit, new GUIContent("Is There a Proximity Limt?"));
        if (isProximityLimit.boolValue)
        {
            EditorGUILayout.PropertyField(proximityLimit, new GUIContent("Proximity Limit"));
        }

        EditorGUILayout.PropertyField(isDistanceLimit, new GUIContent("Is There a Distance Limt?"));
        if (isDistanceLimit.boolValue)
        {
            EditorGUILayout.PropertyField(distanceLimit, new GUIContent("Distance Limit"));
        }
    }

    private void SetBasicSettings(GUIStyle attentionStyle, float bpm, bool changedBPM)
    {
        SerializedProperty isBasicSettingsChange = serializedObject.FindProperty("isBasicSettingsChange");

        SerializedProperty isMaterialChange = serializedObject.FindProperty("isMaterialChange");
        SerializedProperty material = serializedObject.FindProperty("material");

        SerializedProperty isColorDurationBeatsChange = serializedObject.FindProperty("isColorDurationBeatsChange");
        SerializedProperty colorDurationBeats = serializedObject.FindProperty("colorDurationBeats");
        SerializedProperty colorDurationSeconds = serializedObject.FindProperty("colorDurationSeconds");

        SerializedProperty isScaleUpDurationChange = serializedObject.FindProperty("isScaleUpDurationChange");
        SerializedProperty scaleUpDurationBeats = serializedObject.FindProperty("scaleUpDurationBeats");
        SerializedProperty scaleUpDurationSeconds = serializedObject.FindProperty("scaleUpDurationSeconds");

        SerializedProperty isWaitDurationChange = serializedObject.FindProperty("isWaitDurationChange");
        SerializedProperty waitDurationBeats = serializedObject.FindProperty("waitDurationBeats");
        SerializedProperty waitDurationSeconds = serializedObject.FindProperty("waitDurationSeconds");

        SerializedProperty isScaleDownDurationChange = serializedObject.FindProperty("isScaleDownDurationChange");
        SerializedProperty scaleDownDurationBeats = serializedObject.FindProperty("scaleDownDurationBeats");
        SerializedProperty scaleDownDurationSeconds = serializedObject.FindProperty("scaleDownDurationSeconds");

        SerializedProperty isHeightChange = serializedObject.FindProperty("isHeightChange");
        SerializedProperty height = serializedObject.FindProperty("height");

        SerializedProperty isOffsetChange = serializedObject.FindProperty("isOffsetChange");
        SerializedProperty offset = serializedObject.FindProperty("offset");

        EditorGUILayout.PropertyField(isBasicSettingsChange, new GUIContent("Is Basic Settings Change?"));
        EditorGUILayout.LabelField(" Do you really wanna change Basic Settings??? -_- Bro...", attentionStyle);
        if (isBasicSettingsChange.boolValue)
        {
            EditorGUILayout.PropertyField(isMaterialChange, new GUIContent("Is Material Change?"));
            if (isMaterialChange.boolValue)
            {
                EditorGUILayout.PropertyField(material, new GUIContent("Material"));
            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isColorDurationBeatsChange, new GUIContent("isScaleUpDurationChange?"));
            if (isColorDurationBeatsChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(colorDurationSeconds, new GUIContent("colorDurationSeconds (seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(colorDurationBeats, new GUIContent("colorDurationBeats (Beats)"));
                bool changedEndBeats = EditorGUI.EndChangeCheck();

                if (changedEndSeconds || changedBPM)
                {
                    colorDurationBeats.floatValue = colorDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedEndBeats || changedBPM) && bpm != 0f)
                {
                    colorDurationSeconds.floatValue = colorDurationBeats.floatValue * 60f / bpm;
                }
            }



            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isScaleUpDurationChange, new GUIContent("isScaleUpDurationChange?"));
            if (isScaleUpDurationChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(scaleUpDurationSeconds, new GUIContent("scaleUpDuration (seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(scaleUpDurationBeats, new GUIContent("scaleUpDuration (Beats)"));
                bool changedEndBeats = EditorGUI.EndChangeCheck();

                if (changedEndSeconds || changedBPM)
                {
                    scaleUpDurationBeats.floatValue = scaleUpDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedEndBeats || changedBPM) && bpm != 0f)
                {
                    scaleUpDurationSeconds.floatValue = scaleUpDurationBeats.floatValue * 60f / bpm;
                }
            }





            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isWaitDurationChange, new GUIContent("Is Wait Duration Change?"));
            if (isWaitDurationChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(waitDurationSeconds, new GUIContent("scaleDownDuration (seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(waitDurationBeats, new GUIContent("scaleDownDuration (Beats)"));
                bool changedEndBeats = EditorGUI.EndChangeCheck();

                if (changedEndSeconds || changedBPM)
                {
                    waitDurationBeats.floatValue = waitDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedEndBeats || changedBPM) && bpm != 0f)
                {
                    waitDurationSeconds.floatValue = waitDurationBeats.floatValue * 60f / bpm;
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isScaleDownDurationChange, new GUIContent("Is Scale Down Duration Change?"));
            if (isScaleDownDurationChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(scaleDownDurationSeconds, new GUIContent("scaleDownDuration (seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(scaleDownDurationBeats, new GUIContent("scaleDownDuration (Beats)"));
                bool changedEndBeats = EditorGUI.EndChangeCheck();

                if (changedEndSeconds || changedBPM)
                {
                    scaleDownDurationBeats.floatValue = scaleDownDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedEndBeats || changedBPM) && bpm != 0f)
                {
                    scaleDownDurationSeconds.floatValue = scaleDownDurationBeats.floatValue * 60f / bpm;
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isHeightChange, new GUIContent("Is Height Change?"));
            if (isHeightChange.boolValue)
            {
                EditorGUILayout.PropertyField(offset, new GUIContent("Height"));
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isOffsetChange, new GUIContent("Is Offset Change"));
            if (isOffsetChange.boolValue)
            {
                EditorGUILayout.PropertyField(offset, new GUIContent("Offset"));
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