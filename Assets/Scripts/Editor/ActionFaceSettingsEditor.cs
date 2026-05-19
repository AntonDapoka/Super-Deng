using UnityEditor;
using UnityEngine;

public abstract class ActionFaceSettingsEditor : ActionSettingsEditor
{
    public override void SetActionSpecialSettings(float bpm, bool changedBPM, bool isHint)
    {
        AddSettingsSection("Face Settings:", Color.red, () =>
        {
            SetRandom(isHint);
            SetCertain(isHint);
        });

        if (isHint)
            EditorGUILayout.HelpBox("�� ���������, ��� �������� ����������� � �������� ������������ ����� ���������� ������������. �� ��������� ������� ������� ������ ������", MessageType.Warning);

        AddSettingsSection("Limit Settings:", Color.blue, () =>
        {
            SetProximityAndDistanceLimit(isHint);
        });

        SetActionFaceSpecialSettings(bpm, changedBPM, isHint);
    }

    public abstract void SetActionFaceSpecialSettings(float bpm, bool changedBPM, bool isHint);

    private void SetRandom(bool isHint)
    {
        SerializedProperty isRandom = serializedObject.FindProperty("isRandom");
        SerializedProperty isPseudoRandom = serializedObject.FindProperty("isPseudoRandom");
        SerializedProperty isStableQuantity = serializedObject.FindProperty("isStableQuantity");
        SerializedProperty quantityExact = serializedObject.FindProperty("quantityExact");
        SerializedProperty quantityMin = serializedObject.FindProperty("quantityMin");
        SerializedProperty quantityMax = serializedObject.FindProperty("quantityMax");

        EditorGUILayout.PropertyField(isRandom, new GUIContent("Is It Random?"));

        if (isHint)
            EditorGUILayout.HelpBox("� ������ ������� �������� ����������� ������������ ����� ��������� ���� �� ���� ���������", MessageType.Info);

        if (isRandom.boolValue)
        {
            EditorGUILayout.PropertyField(isPseudoRandom, new GUIContent("Is Pseudo Random?"));
            
            EditorGUILayout.PropertyField(isStableQuantity, new GUIContent("Is Quantity Always Stable?"));

            if (isHint)
                EditorGUILayout.HelpBox("���� ���� ����� �������� true, �� ���������� ���������� ���� ����� ��������� ������ ���, ���� ��� ����� false, �� ��� ����� �������� ���������� ����� ���� �������", MessageType.Info);

            if (isStableQuantity.boolValue)
            {
                EditorGUILayout.PropertyField(quantityExact, new GUIContent("Exact Quantity"));

                if (isHint)
                    EditorGUILayout.HelpBox("���������� ���������� �������� ���������� ����. ������ ��� ����� ���������� " + quantityExact.floatValue + " ����", MessageType.Info);
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(quantityMin, new GUIContent("Minimum Quantity"));
                bool changedQuantityMin = EditorGUI.EndChangeCheck();

                if (isHint)
                    EditorGUILayout.HelpBox("����������� ���������� ���������� ����. ����� ���������� ���� ����� ���������� �������� ����� " + quantityMin.floatValue + " � " + quantityMax.floatValue, MessageType.Info);

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(quantityMax, new GUIContent("Maximum Quantity"));
                bool changedQuantityMax = EditorGUI.EndChangeCheck();

                if (isHint)
                    EditorGUILayout.HelpBox("������������ ���������� ���������� ����.", MessageType.Info);


                if (changedQuantityMin && quantityMin.floatValue > quantityMax.floatValue)
                {
                    quantityMax.floatValue = quantityMin.floatValue;
                }
                else if (changedQuantityMax && quantityMin.floatValue > quantityMax.floatValue)
                {
                    quantityMin.floatValue = quantityMax.floatValue;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMin.floatValue < 0)
                {
                    quantityMin.floatValue = 0;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMax.floatValue < 0)
                {
                    quantityMax.floatValue = 0;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMin.floatValue > 321)
                {
                    quantityMin.floatValue = 321;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMax.floatValue > 321)
                {
                    quantityMax.floatValue = 321;
                }
            }
        }
    }

    private void SetCertain(bool isHint)
    {
        SerializedProperty isCertain = serializedObject.FindProperty("isCertain");
        SerializedProperty isRelativeToPlayer = serializedObject.FindProperty("isRelativeToPlayer");
        SerializedProperty arrayOfFacesRelativeToPlayer = serializedObject.FindProperty("arrayOfFacesRelativeToPlayer");
        SerializedProperty isRelativeToFigure = serializedObject.FindProperty("isRelativeToFigure");
        SerializedProperty arrayOfFacesRelativeToFigure = serializedObject.FindProperty("arrayOfFacesRelativeToFigure");

        EditorGUILayout.PropertyField(isCertain, new GUIContent("Is It Certain?"));

        if (isHint)
            EditorGUILayout.HelpBox("���������� ���������� ����� ����������� �������.", MessageType.Info);

        if (isCertain.boolValue)
        {
            EditorGUILayout.PropertyField(isRelativeToFigure, new GUIContent("Is It Relative To Figure?"));

            if (isHint)
                EditorGUILayout.HelpBox("����� ������������ ������, ����� ������� �����������, �� ������� ������������ �����, �� �������� � �������� ������� � ������������ ������.", MessageType.Info);

            if (isRelativeToFigure.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfFacesRelativeToFigure, new GUIContent("Array Of Faces Relative To Figure"));

                if (isHint)
                    EditorGUILayout.HelpBox("��� �������� ������ �������� �������� �� ������������������ �����.", MessageType.Info);
            }

            EditorGUILayout.PropertyField(isRelativeToPlayer, new GUIContent("Is It Relative To Player?"));

            if (isHint)
                EditorGUILayout.HelpBox("����� ������������ ������ �������� � ������������ ���������", MessageType.Info);

            if (isRelativeToPlayer.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfFacesRelativeToPlayer, new GUIContent("Array Of Faces Relative To Player"));

                if (isHint)
                    EditorGUILayout.HelpBox("��� ���������� ������ �������� �������� �� ������������������ �����.", MessageType.Info);
            }
        }
    }
    private void SetProximityAndDistanceLimit(bool isHint)
    {
        SerializedProperty isProximityLimit = serializedObject.FindProperty("isProximityLimit");
        SerializedProperty proximityLimit = serializedObject.FindProperty("proximityLimit");
        SerializedProperty isDistanceLimit = serializedObject.FindProperty("isDistanceLimit");
        SerializedProperty distanceLimit = serializedObject.FindProperty("distanceLimit");

        if (isHint)
            EditorGUILayout.HelpBox("���� �� ������ ���������� ����� ���� �� ������������ �� ������, �� ������� �������. �������� � ��� ���������, � ���������� ����.", MessageType.Info);

        EditorGUILayout.PropertyField(isProximityLimit, new GUIContent("Is There a Proximity Limit?"));

        if (isProximityLimit.boolValue)
        {
            EditorGUILayout.PropertyField(proximityLimit, new GUIContent("Proximity Limit"));

            if (isHint)
                EditorGUILayout.HelpBox("����� �� ��������������. ���� ����� 1, �� �� �������� ������ �� ������ �� ����� ���������� �����, ���� 2, �� �� ������� ������� �� ����� ���������� ������� �����, � ��� �����", MessageType.Info);
        }

        EditorGUILayout.PropertyField(isDistanceLimit, new GUIContent("Is There a Distance Limit?"));
        if (isDistanceLimit.boolValue)
        {
            EditorGUILayout.PropertyField(distanceLimit, new GUIContent("Distance Limit"));

            if (isHint)
                EditorGUILayout.HelpBox("���������� ����������� ������, ������ �� ������������. ��������� ���� �������������� ������� ����� ���������.", MessageType.Info);
        }
    }
}
