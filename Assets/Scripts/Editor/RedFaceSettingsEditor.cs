using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RedFaceSettings))]
public class RedFaceSettingsEditor : Editor
{
    private GUIStyle headerStyle;
    private GUIStyle labelStyle;
    private GUIStyle attentionStyle;
    private GUIStyle hintStyle;

    private bool stylesInitialized = false;

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
        
        EditorGUILayout.LabelField("Red Face Effect \"" + effectName.stringValue + "\"", headerStyle);
        EditorGUILayout.PropertyField(effectName, new GUIContent("Name of the effect:"));

        SetRedFaces();

        serializedObject.ApplyModifiedProperties();
    }

    private void SetRedFaces()
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
        });

        AddSettingsSection("Face Settings:", Color.red, () =>
        {
            SetRandom(isHint.boolValue);
            SetCertain(isHint.boolValue);
        });
        if (isHint.boolValue)
            EditorGUILayout.HelpBox("Не забывайте, что параметр рандомности и параметр конкретности могут вызываться ОДНОВРЕМЕННО. Не забывайте вовремя удалять лишние данные", MessageType.Warning);

        AddSettingsSection("Limit Settings:", Color.blue, () =>
        {
            SetProximityAndDistanceLimit(isHint.boolValue);
        });

        AddSettingsSection("Basic Settings:", Color.cyan, () =>
        {
            SetBasicSettings(bpm.floatValue, changedBPM, isHint.boolValue);
        });

    }

    private void AddSettingsSection(string title, Color color, Action content)
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

        if(isHint)
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

        EditorGUILayout.PropertyField(isProximityLimit, new GUIContent("Is There a Proximity Limt?"));

        if (isProximityLimit.boolValue)
        {
            EditorGUILayout.PropertyField(proximityLimit, new GUIContent("Proximity Limit"));

            if (isHint)
                EditorGUILayout.HelpBox("Лимит по приближенности. Если равен 1, то на соседних гранях от игрока не будут появляться враги, если 2, то на соседях соседях не будут появляться красные плиты, и так далее", MessageType.Info);
        }

        EditorGUILayout.PropertyField(isDistanceLimit, new GUIContent("Is There a Distance Limt?"));
        if (isDistanceLimit.boolValue)
        {
            EditorGUILayout.PropertyField(distanceLimit, new GUIContent("Distance Limit"));

            if (isHint)
                EditorGUILayout.HelpBox("Аналогично предыдущему лимиту, только по отдаленности. Проверьте ваши математические расчеты перед указанием.", MessageType.Info);
        }
    }

    private void SetBasicSettings(float bpm, bool changedBPM, bool isHint)
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