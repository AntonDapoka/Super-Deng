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

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Red Face Settings:", labelStyle);
        SetRedFaces();

        

        serializedObject.ApplyModifiedProperties();
    }

    private void SetRedFaces()
    { 
        SerializedProperty bpm = serializedObject.FindProperty("bpm");
        SerializedProperty timeStartSeconds = serializedObject.FindProperty("timeStartSeconds");
        SerializedProperty timeStartBeats = serializedObject.FindProperty("timeStartBeats");
        SerializedProperty isTimeEnd = serializedObject.FindProperty("isTimeEnd");
        SerializedProperty timeEndSeconds = serializedObject.FindProperty("timeEndSeconds");
        SerializedProperty timeEndBeats = serializedObject.FindProperty("timeEndBeats");
        SerializedProperty isRandom = serializedObject.FindProperty("isRandom");
        SerializedProperty isArray = serializedObject.FindProperty("isArray");
        SerializedProperty isInterval = serializedObject.FindProperty("isInterval");
        SerializedProperty quantityExact = serializedObject.FindProperty("quantityExact");
        SerializedProperty quantityMin = serializedObject.FindProperty("quantityMin");
        SerializedProperty quantityMax = serializedObject.FindProperty("quantityMax");
        SerializedProperty isCommonArray = serializedObject.FindProperty("isCommonArray");
        SerializedProperty arrayOfFaces = serializedObject.FindProperty("arrayOfFaces");
        SerializedProperty isStripArray = serializedObject.FindProperty("isStripArray");
        SerializedProperty arrayOfStrips = serializedObject.FindProperty("arrayOfStrips");
        SerializedProperty isRelativeToThePlayerArray = serializedObject.FindProperty("isRelativeToThePlayerArray");
        SerializedProperty arrayOfEqualDistanceFaces = serializedObject.FindProperty("arrayOfEqualDistanceFaces");

        SerializedProperty isTurnOnWayFinder = serializedObject.FindProperty("isTurnOnWayFinder");
        SerializedProperty colorDurationBeatsChange = serializedObject.FindProperty("colorDurationBeatsChange");
        SerializedProperty colorDurationSecondsChange = serializedObject.FindProperty("colorDurationSecondsChange");
        SerializedProperty scaleDurationBeatsChangeUp = serializedObject.FindProperty("scaleDurationBeatsChangeUp");
        SerializedProperty scaleDurationSecondsChangeUp = serializedObject.FindProperty("scaleDurationSecondsChangeUp");
        SerializedProperty waitDurationBeats = serializedObject.FindProperty("waitDurationBeats");
        SerializedProperty waitDurationSeconds = serializedObject.FindProperty("waitDurationSeconds");
        SerializedProperty scaleDurationBeatsChangeDown = serializedObject.FindProperty("scaleDurationBeatsChangeDown");
        SerializedProperty scaleDurationSecondsChangeDown = serializedObject.FindProperty("scaleDurationSecondsChangeDown");
        SerializedProperty scaleChange = serializedObject.FindProperty("scaleChange");


        EditorGUILayout.PropertyField(bpm, new GUIContent("Track BPM"));

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(timeStartSeconds, new GUIContent("Start Time (seconds)"));
        bool changedStartSeconds = EditorGUI.EndChangeCheck();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(timeStartBeats, new GUIContent("Start Time (beats)"));
        bool changedStartBeats = EditorGUI.EndChangeCheck();

        int bpmInt = bpm.intValue;

        if (changedStartSeconds)
        {
            timeStartBeats.floatValue = timeStartSeconds.floatValue * bpmInt / 60f;
        }
        else if (changedStartBeats)
        {
            timeStartSeconds.floatValue = timeStartBeats.floatValue * 60f / bpmInt;
        }

        EditorGUILayout.PropertyField(isTimeEnd, new GUIContent("Is there End Time?"));

        if (isTimeEnd.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeEndSeconds, new GUIContent("End Time (seconds)"));
            bool changedEndSeconds = EditorGUI.EndChangeCheck();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(timeEndBeats, new GUIContent("End Time (beats)"));
            bool changedEndBeats = EditorGUI.EndChangeCheck();

            if (changedEndSeconds)
            {
                timeEndBeats.floatValue = timeEndSeconds.floatValue * bpmInt / 60f;
            }
            else if (changedEndBeats)
            {
                timeEndSeconds.floatValue = timeEndBeats.floatValue * 60f / bpmInt;
            }
        }
        EditorGUILayout.Space();


        EditorGUILayout.PropertyField(isRandom, new GUIContent("Is Random?"));

        if (isRandom.boolValue)
        {
            EditorGUILayout.PropertyField(isInterval, new GUIContent("is Exact Number?"));

            if (isInterval.boolValue)
            {
                
                EditorGUILayout.PropertyField(quantityMin, new GUIContent("Minimum Quantity"));
                EditorGUILayout.PropertyField(quantityMax, new GUIContent("Maximum Quantity"));
            }
            else
            {
                EditorGUILayout.PropertyField(quantityExact, new GUIContent("Quantity of Red Faces"));
            }
        }

        EditorGUILayout.PropertyField(isArray, new GUIContent("Is Array?"));
        if (isArray.boolValue)
        {
            EditorGUILayout.PropertyField(isCommonArray, new GUIContent("Is Array Common?"));
            if (isCommonArray.boolValue) 
            {
                EditorGUILayout.PropertyField(arrayOfFaces, new GUIContent("Common Array Of Faces"));
            }

            EditorGUILayout.PropertyField(isStripArray, new GUIContent("Is Strip Array?"));
            if (isStripArray.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfStrips, new GUIContent("Array Of Strips"));
            }

            EditorGUILayout.PropertyField(isRelativeToThePlayerArray, new GUIContent("Is Relative To The Player Array?"));
            if (isRelativeToThePlayerArray.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfEqualDistanceFaces, new GUIContent("Array Of Equal Distance Faces"));
            }
        }

        EditorGUILayout.PropertyField(isTurnOnWayFinder, new GUIContent("Is Turn On Way Finder?"));
        EditorGUILayout.PropertyField(colorDurationBeatsChange, new GUIContent("color Duration Beats Change"));
        EditorGUILayout.PropertyField(colorDurationSecondsChange, new GUIContent("color Duration Seconds Change"));
        EditorGUILayout.PropertyField(scaleDurationBeatsChangeUp, new GUIContent("scale Duration Beats Change Up"));
        EditorGUILayout.PropertyField(scaleDurationSecondsChangeUp, new GUIContent("scale Duration Seconds Change Up"));
        EditorGUILayout.PropertyField(waitDurationBeats, new GUIContent("wait Duration Beats"));
        EditorGUILayout.PropertyField(waitDurationSeconds, new GUIContent("wait Duration Seconds"));
        EditorGUILayout.PropertyField(scaleDurationBeatsChangeDown, new GUIContent("scale Duration Beats Change Down"));
        EditorGUILayout.PropertyField(scaleDurationSecondsChangeDown, new GUIContent("scale Duration Seconds Change Down"));
        EditorGUILayout.PropertyField(scaleChange, new GUIContent("scaleChange"));
    }
}