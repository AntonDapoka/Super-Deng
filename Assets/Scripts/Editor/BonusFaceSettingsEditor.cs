using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BonusFaceSettings))]
public class BonusFaceSettingsEditor : ActionFaceSettingsEditor
{
    public override string GetActionStringName()
    {
        return "Bonus";
    }

    public override void SetActionFaceSpecialSettings(float bpm, bool changedBPM, bool isHint)
    {
        AddSettingsSection("Bonus Special Settings:", Color.clear, () =>
        {
            SetBonusSpecialSettings(bpm, changedBPM, isHint);
        });

        AddSettingsSection("Basic Settings:", Color.cyan, () =>
        {
            SetBasicSettings(isHint);
        });
    }

    private void SetBonusSpecialSettings(float bpm, bool changedBPM, bool isHint)
    {
        SerializedProperty typeBonus = serializedObject.FindProperty("typeBonus");

        SerializedProperty isLifeDuration = serializedObject.FindProperty("isLifeDuration");
        SerializedProperty lifeDurationSeconds = serializedObject.FindProperty("lifeDurationSeconds");
        SerializedProperty lifeDurationBeats = serializedObject.FindProperty("lifeDurationBeats");

        SerializedProperty isDeathDuration = serializedObject.FindProperty("isDeathDuration");
        SerializedProperty deathDurationSeconds = serializedObject.FindProperty("deathDurationSeconds");
        SerializedProperty deathDurationBeats = serializedObject.FindProperty("deathDurationBeats");

        EditorGUILayout.PropertyField(typeBonus, new GUIContent("Bonus Type"));

        EditorGUILayout.PropertyField(isLifeDuration, new GUIContent("Is Life Duration?"));

        if (isHint)
            EditorGUILayout.HelpBox("����� ����� �����, �� ��������� �������� ��� ���������� �����������", MessageType.Info);


        if (isLifeDuration.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(lifeDurationSeconds, new GUIContent("Life Duration (Seconds)"));
            bool changedLifeDurationSeconds = EditorGUI.EndChangeCheck();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(lifeDurationBeats, new GUIContent("Life Duration (Beats)"));
            bool changedLifeDurationBeats = EditorGUI.EndChangeCheck();

            if (changedLifeDurationSeconds || changedBPM)
            {
                lifeDurationBeats.floatValue = lifeDurationSeconds.floatValue * bpm / 60f;
            }
            else if ((changedLifeDurationBeats || changedBPM) && bpm != 0f)
            {
                lifeDurationSeconds.floatValue = lifeDurationBeats.floatValue * 60f / bpm;
            }

            EditorGUILayout.PropertyField(isDeathDuration, new GUIContent("Is Death Duration?"));

            if (isDeathDuration.boolValue)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(deathDurationSeconds, new GUIContent("Death Duration (Seconds)"));
                bool changedDeathDurationSeconds = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(deathDurationBeats, new GUIContent("Death Duration (Beats)"));
                bool changedDeathDurationBeats = EditorGUI.EndChangeCheck();

                if (changedDeathDurationSeconds || changedBPM)
                {
                    deathDurationBeats.floatValue = deathDurationSeconds.floatValue * bpm / 60f;
                }
                else if ((changedDeathDurationBeats || changedBPM) && bpm != 0f)
                {
                    deathDurationSeconds.floatValue = deathDurationBeats.floatValue * 60f / bpm;
                }
            }
        }
    }

    private void SetBasicSettings(bool isHint)
    {
        SerializedProperty isBasicSettingsChange = serializedObject.FindProperty("isBasicSettingsChange");

        SerializedProperty isMaterialChange = serializedObject.FindProperty("isMaterialChange");
        SerializedProperty material = serializedObject.FindProperty("material");

        SerializedProperty isBonusSymbolPrefabChange = serializedObject.FindProperty("isBonusSymbolPrefabChange");
        SerializedProperty bonusSymbolPrefab = serializedObject.FindProperty("bonusSymbolPrefab");

        EditorGUILayout.PropertyField(isBasicSettingsChange, new GUIContent("Is Basic Settings Change?"));
        EditorGUILayout.LabelField(" Do you really wanna change Basic Settings??? -_- Bro...", attentionStyle);
        if (isBasicSettingsChange.boolValue)
        {
            EditorGUILayout.PropertyField(isMaterialChange, new GUIContent("Is Material Change?"));
            if (isMaterialChange.boolValue)
            {
                EditorGUILayout.PropertyField(material, new GUIContent("Material"));
            }

            EditorGUILayout.PropertyField(isBonusSymbolPrefabChange, new GUIContent("Is Bonus Symbol Prefab Change?"));
            if (isBonusSymbolPrefabChange.boolValue)
            {
                EditorGUILayout.PropertyField(bonusSymbolPrefab, new GUIContent("Bonus Symbol Prefab"));
            }
        }
    }
}