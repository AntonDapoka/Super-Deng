using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawnSettings))]
public class EnemySpawnSettingsEditor : Editor
{
    SerializedProperty spawnTimes;

    void OnEnable()
    {
        spawnTimes = serializedObject.FindProperty("spawnTimes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < spawnTimes.arraySize; i++)
        {
            SerializedProperty spawnTime = spawnTimes.GetArrayElementAtIndex(i);

            SerializedProperty time = spawnTime.FindPropertyRelative("time");

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

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Frame ¹" + i.ToString(), headerStyle);
            EditorGUILayout.PropertyField(time, new GUIContent("Time (beats)"));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Red Face Settings:", labelStyle);
            SetRedFaces(spawnTime);
            
            EditorGUILayout.LabelField("Red Wave Settings:", labelStyle);
            SetRedWaves(spawnTime);

            EditorGUILayout.LabelField("Fall Face Settings:", labelStyle);
            SetFallFaces(spawnTime);

            EditorGUILayout.LabelField("Reset Fall Face Settings:", labelStyle);
            SetReset(spawnTime);

            EditorGUILayout.LabelField("Bonus Settings:", labelStyle);
            SetBonusFaces(spawnTime, attentionStyle);

            EditorGUILayout.LabelField("Portal Settings:", labelStyle);
            SetPortalFaces(spawnTime);

            EditorGUILayout.LabelField("FaceDance Settings:", labelStyle);
            SetFaceDance(spawnTime);

            EditorGUILayout.LabelField("SphereDance Settings:", labelStyle);
            SetSphereDance(spawnTime);

            EditorGUILayout.LabelField("Camera Rotation Settings:", labelStyle);
            SetCameraRotation(spawnTime);

            EditorGUILayout.LabelField("RGB Settings:", labelStyle);
            SetRGB(spawnTime, attentionStyle);

            EditorGUILayout.LabelField("Orbits Settings:", labelStyle);
            SetOrbits(spawnTime, attentionStyle);

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Spawn Time"))
        {
            spawnTimes.InsertArrayElementAtIndex(spawnTimes.arraySize);
        }

        if (GUILayout.Button("Remove Last Spawn Time"))
        {
            if (spawnTimes.arraySize > 0)
            {
                spawnTimes.DeleteArrayElementAtIndex(spawnTimes.arraySize - 1);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SetRedFaces(SerializedProperty spawnTime)
    {
        SerializedProperty isRedFaceTurnOn = spawnTime.FindPropertyRelative("isRedFaceTurnOn");
        SerializedProperty isRedFaceRandom = spawnTime.FindPropertyRelative("isRedFaceRandom");
        SerializedProperty arrayOfRedFaces = spawnTime.FindPropertyRelative("arrayOfRedFaces");
        SerializedProperty quantityOfRedFaces = spawnTime.FindPropertyRelative("quantityOfRedFaces");

        EditorGUILayout.PropertyField(isRedFaceTurnOn, new GUIContent("Is Red Face turn on?"));
        if (isRedFaceTurnOn.boolValue)
        {

            EditorGUILayout.PropertyField(isRedFaceRandom, new GUIContent("Is Red Face random?"));
            if (isRedFaceRandom.boolValue)
            {
                EditorGUILayout.PropertyField(quantityOfRedFaces, new GUIContent("Quantity of Red Faces"));
            }
            else
            {
                EditorGUILayout.PropertyField(arrayOfRedFaces, new GUIContent("Array of Red Faces"), true);
            }
        }
    }

    private void SetRedWaves(SerializedProperty spawnTime)
    {

        SerializedProperty isRedWaveTurnOn = spawnTime.FindPropertyRelative("isRedWaveTurnOn");
        SerializedProperty isRedWaveRandom = spawnTime.FindPropertyRelative("isRedWaveRandom");
        SerializedProperty arrayOfRedWaves = spawnTime.FindPropertyRelative("arrayOfRedWaves");
        SerializedProperty lifeDurationOfRedWaves = spawnTime.FindPropertyRelative("lifeDurationOfRedWaves");
        SerializedProperty quantityOfRedWaves = spawnTime.FindPropertyRelative("quantityOfRedWaves");
        SerializedProperty proximityLimitOfRedWaves = spawnTime.FindPropertyRelative("proximityLimitOfRedWaves");

        EditorGUILayout.PropertyField(isRedWaveTurnOn, new GUIContent("Is Red Wave turn on?"));
        if (isRedWaveTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(isRedWaveRandom, new GUIContent("Is Red Wave random?"));
            if (isRedWaveRandom.boolValue)
            {
                EditorGUILayout.PropertyField(quantityOfRedWaves, new GUIContent("Quantity of Red Waves"));
            }
            else
            {
                EditorGUILayout.PropertyField(arrayOfRedWaves, new GUIContent("Array of Red Waves"), true);
            }
            EditorGUILayout.PropertyField(lifeDurationOfRedWaves, new GUIContent("Life Duration Of Red Waves"));
            EditorGUILayout.PropertyField(proximityLimitOfRedWaves, new GUIContent("Proximity Limit of Red Waves"));
        }
    }

    private void SetFallFaces(SerializedProperty spawnTime)
    {

        SerializedProperty isFallFaceTurnOn = spawnTime.FindPropertyRelative("isFallFaceTurnOn");
        SerializedProperty isFallFaceRandom = spawnTime.FindPropertyRelative("isFallFaceRandom");
        SerializedProperty arrayOfFallFaces = spawnTime.FindPropertyRelative("arrayOfFallFaces");
        SerializedProperty quantityOfFallFaces = spawnTime.FindPropertyRelative("quantityOfFallFaces");
        SerializedProperty proximityLimitOfFallFaces = spawnTime.FindPropertyRelative("proximityLimitOfFallFaces");
        SerializedProperty isResetDelay = spawnTime.FindPropertyRelative("isResetDelay");
        SerializedProperty resetDelayTime = spawnTime.FindPropertyRelative("resetDelayTime");

        EditorGUILayout.PropertyField(isFallFaceTurnOn, new GUIContent("Is Fall Face turn on?"));
        if (isFallFaceTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(isFallFaceRandom, new GUIContent("Is Fall Face random?"));
            if (isFallFaceRandom.boolValue)
            {
                EditorGUILayout.PropertyField(quantityOfFallFaces, new GUIContent("Quantity of Fall Faces"));
            }
            else
            {
                EditorGUILayout.PropertyField(arrayOfFallFaces, new GUIContent("Array of Fall Faces"), true);
            }

            EditorGUILayout.PropertyField(proximityLimitOfFallFaces, new GUIContent("Proximity Limit of Fall Faces"));
            EditorGUILayout.PropertyField(isResetDelay, new GUIContent("Is Reset Delay?"));
            if (isResetDelay.boolValue)
            {
                EditorGUILayout.PropertyField(resetDelayTime, new GUIContent("Reset Delay time"));
            }

        }
    }

    private void SetReset(SerializedProperty spawnTime)
    {
        SerializedProperty isResetFallFaceTurnOn = spawnTime.FindPropertyRelative("isResetFallFaceTurnOn");
        EditorGUILayout.PropertyField(isResetFallFaceTurnOn, new GUIContent("Is Reset Fall Face turn on?"));
    }

    private void SetBonusFaces(SerializedProperty spawnTime, GUIStyle attentionStyle)
    {
        SerializedProperty isBonusTurnOn = spawnTime.FindPropertyRelative("isBonusTurnOn");
        SerializedProperty isBonusRandom = spawnTime.FindPropertyRelative("isBonusRandom");
        SerializedProperty arrayOfBonuses = spawnTime.FindPropertyRelative("arrayOfBonuses");
        SerializedProperty quantityOfBonuses = spawnTime.FindPropertyRelative("quantityOfBonuses");
        SerializedProperty proximityLimitOfBonuses = spawnTime.FindPropertyRelative("proximityLimitOfBonuses");
        SerializedProperty isBonusHealth = spawnTime.FindPropertyRelative("isBonusHealth");
        SerializedProperty isBonusCombo = spawnTime.FindPropertyRelative("isBonusCombo");

        EditorGUILayout.PropertyField(isBonusTurnOn, new GUIContent("Is Bonus turn on?"));
        if (isBonusTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(isBonusRandom, new GUIContent("Is Bonus Random?"));
            if (isBonusRandom.boolValue)
            {
                EditorGUILayout.PropertyField(quantityOfBonuses, new GUIContent("Quantity of Bonuses"));
            }
            else
            {
                EditorGUILayout.PropertyField(arrayOfBonuses, new GUIContent("Array of Bonuses"), true);
            }

            EditorGUILayout.PropertyField(proximityLimitOfBonuses, new GUIContent("Proximity Limit of Bonuses"));
            EditorGUILayout.PropertyField(isBonusHealth, new GUIContent("Is Health Bonus?"));
            EditorGUILayout.PropertyField(isBonusCombo, new GUIContent("Is Combo Bonus?"));
            EditorGUILayout.LabelField("can not be false and false!!!", attentionStyle);
        }
    }

    private void SetPortalFaces(SerializedProperty spawnTime)
    {
        SerializedProperty isPortalTurnOn = spawnTime.FindPropertyRelative("isPortalTurnOn");
        SerializedProperty isPortalRandom = spawnTime.FindPropertyRelative("isPortalRandom");
        SerializedProperty arrayOfPortals = spawnTime.FindPropertyRelative("arrayOfPortals");
        SerializedProperty quantityOfPortals = spawnTime.FindPropertyRelative("quantityOfPortals");
        SerializedProperty proximityLimitOfPortals = spawnTime.FindPropertyRelative("proximityLimitOfPortals");

        EditorGUILayout.PropertyField(isPortalTurnOn, new GUIContent("Is Portal turn on?"));
        if (isPortalTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(isPortalRandom, new GUIContent("Is Portal Random?"));
            if (isPortalRandom.boolValue)
            {
                EditorGUILayout.PropertyField(quantityOfPortals, new GUIContent("Quantity of Portals"));
            }
            else
            {
                EditorGUILayout.PropertyField(arrayOfPortals, new GUIContent("Array of Portals"), true);
            }
            EditorGUILayout.PropertyField(proximityLimitOfPortals, new GUIContent("Proximity Limit of Portals"));
        }
    }

    private void SetFaceDance(SerializedProperty spawnTime)
    {
        SerializedProperty isFaceDanceTurnOn = spawnTime.FindPropertyRelative("isFaceDanceTurnOn");
        SerializedProperty durationOfCycleFaceDance = spawnTime.FindPropertyRelative("durationOfCycleFaceDance");
        SerializedProperty scaleFactorFaceDance = spawnTime.FindPropertyRelative("scaleFactorFaceDance");
        SerializedProperty isChangingFaceDance = spawnTime.FindPropertyRelative("isChangingFaceDance");
        SerializedProperty isIncreaseFaceDance = spawnTime.FindPropertyRelative("isIncreaseFaceDance");
        SerializedProperty durationChangingFaceDance = spawnTime.FindPropertyRelative("durationChangingFaceDance");

        EditorGUILayout.PropertyField(isFaceDanceTurnOn, new GUIContent("Is FaceDance turn on?"));
        if (isFaceDanceTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(durationOfCycleFaceDance, new GUIContent("Duration Of Cycle FaceDance"));
            EditorGUILayout.PropertyField(scaleFactorFaceDance, new GUIContent("Scale Factor FaceDance"));
            EditorGUILayout.PropertyField(isChangingFaceDance, new GUIContent("Is Changing FaceDance?"));
            if (isChangingFaceDance.boolValue)
            {
                EditorGUILayout.PropertyField(isIncreaseFaceDance, new GUIContent("Is Increase FaceDance?"));
                EditorGUILayout.PropertyField(durationChangingFaceDance, new GUIContent("Duration of Changing FaceDance"));
            }
        }
    }
    private void SetSphereDance(SerializedProperty spawnTime)
    {
        SerializedProperty isSphereDanceTurnOn = spawnTime.FindPropertyRelative("isSphereDanceTurnOn");
        SerializedProperty angleSphereDance = spawnTime.FindPropertyRelative("angleSphereDance");
        SerializedProperty durationOfCycleSphereDance = spawnTime.FindPropertyRelative("durationOfCycleSphereDance");

        EditorGUILayout.PropertyField(isSphereDanceTurnOn, new GUIContent("Is SphereDance turn on?"));

        if (isSphereDanceTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(angleSphereDance, new GUIContent("Angle of SphereDance"));
            EditorGUILayout.PropertyField(durationOfCycleSphereDance, new GUIContent("durationOfCycleSphereDance"));

        }
    }

    private void SetCameraRotation(SerializedProperty spawnTime)
    {
        SerializedProperty isCameraRotationTurnOn = spawnTime.FindPropertyRelative("isCameraRotationTurnOn");
        SerializedProperty isCameraRotationClockwise = spawnTime.FindPropertyRelative("isCameraRotationClockwise");
        SerializedProperty speedCameraRotation = spawnTime.FindPropertyRelative("speedCameraRotation");

        EditorGUILayout.PropertyField(isCameraRotationTurnOn, new GUIContent("Is Camera Rotation turn on?"));

        if (isCameraRotationTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(isCameraRotationClockwise, new GUIContent("Is Camera Rotation clockwise?"));
            EditorGUILayout.PropertyField(speedCameraRotation, new GUIContent("Camera Rotation Speed"));

        }
    }

    private void SetRGB(SerializedProperty spawnTime, GUIStyle attentionStyle)
    {
        SerializedProperty isRGBTurnOn = spawnTime.FindPropertyRelative("isRGBTurnOn");
        SerializedProperty speedRGB = spawnTime.FindPropertyRelative("speedRGB");
        SerializedProperty targetValueRGB = spawnTime.FindPropertyRelative("targetValueRGB");
        SerializedProperty isSetRGBIncrease = spawnTime.FindPropertyRelative("isSetRGBIncrease");
        SerializedProperty isSetRGBDecrease = spawnTime.FindPropertyRelative("isSetRGBDecrease");

        EditorGUILayout.PropertyField(isRGBTurnOn, new GUIContent("Is RGB turn on?"));
        if (isRGBTurnOn.boolValue)
        {
            EditorGUILayout.PropertyField(speedRGB, new GUIContent("Speed RGB"));
            EditorGUILayout.PropertyField(targetValueRGB, new GUIContent("Target Value SSRGB"));
            EditorGUILayout.PropertyField(isSetRGBIncrease, new GUIContent("Is set RGB increase?"));
            EditorGUILayout.PropertyField(isSetRGBDecrease, new GUIContent("Is set RGB decrease?"));
            EditorGUILayout.LabelField("can not be true and true!!!", attentionStyle);
        }
    }

    private void SetOrbits(SerializedProperty spawnTime, GUIStyle attentionStyle)
    {
        SerializedProperty isOrbitsOn = spawnTime.FindPropertyRelative("isOrbitsOn");
        SerializedProperty quantityOfOrbits = spawnTime.FindPropertyRelative("quantityOfOrbits");
        SerializedProperty sizesOfOrbits = spawnTime.FindPropertyRelative("sizesOfOrbits");
        SerializedProperty speedsOfOrbits = spawnTime.FindPropertyRelative("speedsOfOrbits");
        SerializedProperty materialsOfOrbits = spawnTime.FindPropertyRelative("materialsOfOrbits");
        SerializedProperty minChangeIntervalOfOrbits = spawnTime.FindPropertyRelative("minChangeIntervalOfOrbits");
        SerializedProperty maxChangeIntervalOfOrbits = spawnTime.FindPropertyRelative("maxChangeIntervalOfOrbits");

        EditorGUILayout.PropertyField(isOrbitsOn, new GUIContent("Is Orbits On?"));
        if (isOrbitsOn.boolValue)
        {
            EditorGUILayout.PropertyField(quantityOfOrbits, new GUIContent("Quantity Of Orbits"));
            if (quantityOfOrbits.intValue > 0)
            {
                EditorGUILayout.PropertyField(sizesOfOrbits, new GUIContent("Array Of Sizes Of Orbits"));
                EditorGUILayout.PropertyField(speedsOfOrbits, new GUIContent("Array Of Speeds Of Orbits"));
                EditorGUILayout.PropertyField(materialsOfOrbits, new GUIContent("Array Of Materials Of Orbits"));
                EditorGUILayout.PropertyField(minChangeIntervalOfOrbits, new GUIContent("Min Change Interval Of Orbits"));
                EditorGUILayout.PropertyField(maxChangeIntervalOfOrbits, new GUIContent("Max Change Interval Of Orbits"));
                if (sizesOfOrbits.arraySize != quantityOfOrbits.intValue ||
                    speedsOfOrbits.arraySize != quantityOfOrbits.intValue ||
                    materialsOfOrbits.arraySize != quantityOfOrbits.intValue)
                {
                    EditorGUILayout.LabelField("WROOONGGGG!!! Sizes of Arrays aren't correct!!!", attentionStyle);
                }
                         
            }
        }
    }
}