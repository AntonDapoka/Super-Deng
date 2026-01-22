using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RedFaceSettings))]
public class RedFaceSettingsEditor : ActionFaceSettingsEditor
{
    public override string GetActionStringName()
    {
        return "Red Face";
    }

    public override void SetActionFaceSpecialSettings(float bpm, bool changedBPM, bool isHint)
    {
        AddSettingsSection("Basic Settings:", Color.cyan, () =>
        {
            SetBasicSettings(bpm, changedBPM, isHint);
        });
    }

    private void SetBasicSettings(float bpm, bool changedBPM, bool isHint)
    {
        SerializedProperty isBasicSettingsChange = serializedObject.FindProperty("isBasicSettingsChange");

        SerializedProperty isMaterialChange = serializedObject.FindProperty("isMaterialChange");
        SerializedProperty material = serializedObject.FindProperty("material");

        SerializedProperty isColorDurationChange = serializedObject.FindProperty("isColorDurationChange");
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

            EditorGUILayout.PropertyField(isColorDurationChange, new GUIContent("Is Color Duration Change?"));
            if (isColorDurationChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(colorDurationSeconds, new GUIContent("Color Duration (seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(colorDurationBeats, new GUIContent("Color Duration (Beats)"));
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
            EditorGUILayout.PropertyField(isScaleUpDurationChange, new GUIContent("Is Scale Up Duration Change?"));
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
                EditorGUILayout.PropertyField(waitDurationSeconds, new GUIContent("Scale Down Duration (seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(waitDurationBeats, new GUIContent("Scale Down Duration (Beats)"));
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
                EditorGUILayout.PropertyField(scaleDownDurationSeconds, new GUIContent("Scale Down Duration (Seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(scaleDownDurationBeats, new GUIContent("Scale Down Duration (Beats)"));
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
                EditorGUILayout.PropertyField(height, new GUIContent("Height"));
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isOffsetChange, new GUIContent("Is Offset Change"));
            if (isOffsetChange.boolValue)
            {
                EditorGUILayout.PropertyField(offset, new GUIContent("Offset"));
            }
        }
    }
}