using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FallFaceSettings))]
public class FallFaceSettingsEditor : ActionFaceSettingsEditor
{
    public override string GetActionStringName()
    {
        return "Fall Face";
    }

    public override void SetActionFaceSpecialSettings(float bpm, bool changedBPM, bool isHint)
    {
        /*AddSettingsSection("Fall Face Special Settings:", Color.clear, () =>
        {
            SetFallFaceSpecialSettings(bpm, changedBPM, isHint);
        });*/

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

        SerializedProperty isBlinkingDurationChange = serializedObject.FindProperty("isBlinkingDurationChange");
        SerializedProperty blinkingDurationBeats = serializedObject.FindProperty("blinkingDurationBeats");
        SerializedProperty blinkingDurationSeconds = serializedObject.FindProperty("blinkingDurationSeconds");

        SerializedProperty isReturningDurationChange = serializedObject.FindProperty("isReturningDurationChange");
        SerializedProperty returningDurationBeats = serializedObject.FindProperty("returningDurationBeats");
        SerializedProperty returningDurationSeconds = serializedObject.FindProperty("returningDurationSeconds");

        SerializedProperty isImpulseChange = serializedObject.FindProperty("isImpulseChange");
        SerializedProperty impulse = serializedObject.FindProperty("impulse");

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

            EditorGUILayout.PropertyField(isBlinkingDurationChange, new GUIContent("Is Blinking Duration Change?"));
            if (isBlinkingDurationChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(blinkingDurationSeconds, new GUIContent("Blinking Duration (Seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(blinkingDurationBeats, new GUIContent("Blinking Duration (Beats)"));
                bool changedEndBeats = EditorGUI.EndChangeCheck();

                if (changedEndSeconds || changedBPM)
                {
                    blinkingDurationBeats.floatValue = blinkingDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedEndBeats || changedBPM) && bpm != 0f)
                {
                    blinkingDurationSeconds.floatValue = blinkingDurationBeats.floatValue * 60f / bpm;
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isReturningDurationChange, new GUIContent("Is Returning Duration Change?"));
            if (isReturningDurationChange.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(returningDurationSeconds, new GUIContent("Returning Duration (Seconds)"));
                bool changedEndSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(returningDurationBeats, new GUIContent("Returning Duration (Beats)"));
                bool changedEndBeats = EditorGUI.EndChangeCheck();

                if (changedEndSeconds || changedBPM)
                {
                    returningDurationBeats.floatValue = returningDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedEndBeats || changedBPM) && bpm != 0f)
                {
                    returningDurationSeconds.floatValue = returningDurationBeats.floatValue * 60f / bpm;
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isImpulseChange, new GUIContent("Is Impulse Change?"));
            if (isImpulseChange.boolValue)
            {
                EditorGUILayout.PropertyField(impulse, new GUIContent("Impulse"));
            }
        }
    }
}
