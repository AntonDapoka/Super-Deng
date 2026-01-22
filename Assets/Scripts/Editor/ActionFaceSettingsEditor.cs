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
            EditorGUILayout.HelpBox("Не забывайте, что параметр рандомности и параметр конкретности могут вызываться ОДНОВРЕМЕННО. Не забывайте вовремя удалять лишние данные", MessageType.Warning);

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
        SerializedProperty isStableQuantity = serializedObject.FindProperty("isStableQuantity");
        SerializedProperty quantityExact = serializedObject.FindProperty("quantityExact");
        SerializedProperty quantityMin = serializedObject.FindProperty("quantityMin");
        SerializedProperty quantityMax = serializedObject.FindProperty("quantityMax");

        EditorGUILayout.PropertyField(isRandom, new GUIContent("Is It Random?"));

        if (isHint)
            EditorGUILayout.HelpBox("В случае наличия параметр рандомности предполагает вызов случайных плит из всех возможных", MessageType.Info);


        if (isRandom.boolValue)
        {
            EditorGUILayout.PropertyField(isStableQuantity, new GUIContent("Is Quantity Always Stable?"));

            if (isHint)
                EditorGUILayout.HelpBox("Если поле имеет значение true, то количество вызываемых плит будет одинаково каждый бит, если оно равно false, то оно будет рандомно выбираться между двух величин", MessageType.Info);

            if (isStableQuantity.boolValue)
            {
                EditorGUILayout.PropertyField(quantityExact, new GUIContent("Exact Quantity"));

                if (isHint)
                    EditorGUILayout.HelpBox("Постоянное количество рандомно вызываемых плит. Каждый бит будет вызываться " + quantityExact.intValue + " плит", MessageType.Info);
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(quantityMin, new GUIContent("Minimum Quantity"));
                bool changedQuantityMin = EditorGUI.EndChangeCheck();

                if (isHint)
                    EditorGUILayout.HelpBox("Минимальное количество вызываемых плит. Число вызываемых плит будет выбираться рандомно между " + quantityMin.intValue + " и " + quantityMax.intValue, MessageType.Info);

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(quantityMax, new GUIContent("Maximum Quantity"));
                bool changedQuantityMax = EditorGUI.EndChangeCheck();

                if (isHint)
                    EditorGUILayout.HelpBox("Максимальное количество вызываемых плит.", MessageType.Info);


                if (changedQuantityMin && quantityMin.intValue > quantityMax.intValue)
                {
                    quantityMax.intValue = quantityMin.intValue;
                }
                else if (changedQuantityMax && quantityMin.intValue > quantityMax.intValue)
                {
                    quantityMin.intValue = quantityMax.intValue;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMin.intValue < 0)
                {
                    quantityMin.intValue = 0;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMax.intValue < 0)
                {
                    quantityMax.intValue = 0;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMin.intValue > 321)
                {
                    quantityMin.intValue = 321;
                }

                if ((changedQuantityMin || changedQuantityMax) && quantityMax.intValue > 321)
                {
                    quantityMax.intValue = 321;
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
            EditorGUILayout.HelpBox("Конкретные вызываемые плиты указываются вручную.", MessageType.Info);

        if (isCertain.boolValue)
        {
            EditorGUILayout.PropertyField(isRelativeToFigure, new GUIContent("Is It Relative To Figure?"));

            if (isHint)
                EditorGUILayout.HelpBox("Плиты относительно фигуры, иными словами поверхности, по которой перемещается игрок, не меняются с течением времени и перемещением игрока.", MessageType.Info);

            if (isRelativeToFigure.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfFacesRelativeToFigure, new GUIContent("Array Of Faces Relative To Figure"));

                if (isHint)
                    EditorGUILayout.HelpBox("Для указания точных индексов смотрите на специализированные схемы.", MessageType.Info);
            }

            EditorGUILayout.PropertyField(isRelativeToPlayer, new GUIContent("Is It Relative To Player?"));

            if (isHint)
                EditorGUILayout.HelpBox("Плиты относительно игрока меняются с перемещением пирамидки", MessageType.Info);

            if (isRelativeToPlayer.boolValue)
            {
                EditorGUILayout.PropertyField(arrayOfFacesRelativeToPlayer, new GUIContent("Array Of Faces Relative To Player"));

                if (isHint)
                    EditorGUILayout.HelpBox("Для вычисления точных индексов смотрите на специализированные схемы.", MessageType.Info);
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
            EditorGUILayout.HelpBox("Если вы хотите ограничить спавн плит по отдаленности от игрока, то ставьте галочку. Работает и для рандомных, и конкретных плит.", MessageType.Info);

        EditorGUILayout.PropertyField(isProximityLimit, new GUIContent("Is There a Proximity Limit?"));

        if (isProximityLimit.boolValue)
        {
            EditorGUILayout.PropertyField(proximityLimit, new GUIContent("Proximity Limit"));

            if (isHint)
                EditorGUILayout.HelpBox("Лимит по приближенности. Если равен 1, то на соседних гранях от игрока не будут появляться враги, если 2, то на соседях соседях не будут появляться красные плиты, и так далее", MessageType.Info);
        }

        EditorGUILayout.PropertyField(isDistanceLimit, new GUIContent("Is There a Distance Limit?"));
        if (isDistanceLimit.boolValue)
        {
            EditorGUILayout.PropertyField(distanceLimit, new GUIContent("Distance Limit"));

            if (isHint)
                EditorGUILayout.HelpBox("Аналогично предыдущему лимиту, только по отдаленности. Проверьте ваши математические расчеты перед указанием.", MessageType.Info);
        }
    }
}
