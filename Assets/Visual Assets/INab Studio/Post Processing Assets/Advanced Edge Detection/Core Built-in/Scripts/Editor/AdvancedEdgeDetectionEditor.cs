using UnityEditor;
using UnityEngine;


namespace INab.AdvancedEdgeDetection.BIRP
{
    [CustomEditor(typeof(AdvancedEdgeDetection))]
    public class AdvancedEdgeDetectionEditor : Editor
    {
        #region Serialized Properties
        private SerializedProperty _StencilUse;
        private SerializedProperty _StencilMaskLayer;

        // Edge Detection properties
        SerializedProperty _Thickness;
        SerializedProperty _ResolutionAdjust;
        SerializedProperty _UseDepthFade;
        SerializedProperty _FadeStart;
        SerializedProperty _FadeEnd;
        SerializedProperty _NormalsEdgeDetection;
        SerializedProperty _NormalsOffset;
        SerializedProperty _NormalsHardness;
        SerializedProperty _NormalsPower;
        SerializedProperty _DepthEdgeDetection;
        SerializedProperty _AcuteAngleFix;
        SerializedProperty _ViewDirThreshold;
        SerializedProperty _DepthThreshold;
        SerializedProperty _DepthHardness;
        SerializedProperty _DepthPower;

        // Edge Blend properties
        SerializedProperty _EdgeColor;
        SerializedProperty _UseSketchEdges;
        SerializedProperty _Amplitude;
        SerializedProperty _Frequency;
        SerializedProperty _ChangesPerSecond;
        SerializedProperty _UseEdgeBlendDepthFade;
        SerializedProperty _EdgeBlendFadeStart;
        SerializedProperty _EdgeBlendFadeEnd;
        SerializedProperty _UseGrain;
        SerializedProperty _GrainTexture;
        SerializedProperty _GrainStrength;
        SerializedProperty _GrainScale;
        SerializedProperty _UseUvOffset;
        SerializedProperty _OffsetNoise;
        SerializedProperty _OffsetNoiseScale;
        SerializedProperty _OffsetChangesPerSecond;
        SerializedProperty _OffsetStrength;
        #endregion


        private void OnEnable()
        {
            _StencilMaskLayer = serializedObject.FindProperty("_StencilMaskLayer");

            SerializedProperty settings = serializedObject.FindProperty("m_settings");

            // Edge Detection properties
            _StencilUse = settings.FindPropertyRelative("_StencilUse");
            _Thickness = settings.FindPropertyRelative("_Thickness");
            _ResolutionAdjust = settings.FindPropertyRelative("_ResolutionAdjust");
            _UseDepthFade = settings.FindPropertyRelative("_UseDepthFade");
            _FadeStart = settings.FindPropertyRelative("_FadeStart");
            _FadeEnd = settings.FindPropertyRelative("_FadeEnd");
            _NormalsEdgeDetection = settings.FindPropertyRelative("_NormalsEdgeDetection");
            _NormalsOffset = settings.FindPropertyRelative("_NormalsOffset");
            _NormalsHardness = settings.FindPropertyRelative("_NormalsHardness");
            _NormalsPower = settings.FindPropertyRelative("_NormalsPower");
            _DepthEdgeDetection = settings.FindPropertyRelative("_DepthEdgeDetection");
            _AcuteAngleFix = settings.FindPropertyRelative("_AcuteAngleFix");
            _ViewDirThreshold = settings.FindPropertyRelative("_ViewDirThreshold");
            _DepthThreshold = settings.FindPropertyRelative("_DepthThreshold");
            _DepthHardness = settings.FindPropertyRelative("_DepthHardness");
            _DepthPower = settings.FindPropertyRelative("_DepthPower");

            // Edge Blend properties
            _EdgeColor = settings.FindPropertyRelative("_EdgeColor");
            _UseSketchEdges = settings.FindPropertyRelative("_UseSketchEdges");
            _Amplitude = settings.FindPropertyRelative("_Amplitude");
            _Frequency = settings.FindPropertyRelative("_Frequency");
            _ChangesPerSecond = settings.FindPropertyRelative("_ChangesPerSecond");
            _UseEdgeBlendDepthFade = settings.FindPropertyRelative("_UseEdgeBlendDepthFade");
            _EdgeBlendFadeStart = settings.FindPropertyRelative("_EdgeBlendFadeStart");
            _EdgeBlendFadeEnd = settings.FindPropertyRelative("_EdgeBlendFadeEnd");
            _UseGrain = settings.FindPropertyRelative("_UseGrain");
            _GrainTexture = settings.FindPropertyRelative("_GrainTexture");
            _GrainStrength = settings.FindPropertyRelative("_GrainStrength");
            _GrainScale = settings.FindPropertyRelative("_GrainScale");
            _UseUvOffset = settings.FindPropertyRelative("_UseUvOffset");
            _OffsetNoise = settings.FindPropertyRelative("_OffsetNoise");
            _OffsetNoiseScale = settings.FindPropertyRelative("_OffsetNoiseScale");
            _OffsetChangesPerSecond = settings.FindPropertyRelative("_OffsetChangesPerSecond");
            _OffsetStrength = settings.FindPropertyRelative("_OffsetStrength");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General Settings
            DrawGeneralSettings();

            // Edge Detection Settings
            DrawEdgeDetectionSettings();

            // Edge Blend Settings
            DrawEdgeBlendSettings();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneralSettings()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_StencilMaskLayer);
                EditorGUILayout.PropertyField(_StencilUse);

                if(GUILayout.Button("Update Stencil"))
                {
                    var t = (target as AdvancedEdgeDetection);
                    t.OnDisable();
                    t.OnEnable();
                }
            }
        }

        private void DrawEdgeDetectionSettings()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Edge Detection Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_Thickness);
                EditorGUILayout.PropertyField(_ResolutionAdjust);
                EditorGUILayout.PropertyField(_UseDepthFade);
                if (_UseDepthFade.boolValue)
                {
                    EditorGUILayout.PropertyField(_FadeStart);
                    EditorGUILayout.PropertyField(_FadeEnd);
                }
                EditorGUILayout.PropertyField(_NormalsEdgeDetection);
                if (_NormalsEdgeDetection.boolValue)
                {
                    EditorGUILayout.PropertyField(_NormalsOffset);
                    EditorGUILayout.PropertyField(_NormalsHardness);
                    EditorGUILayout.PropertyField(_NormalsPower);
                }
                EditorGUILayout.PropertyField(_DepthEdgeDetection);
                if (_DepthEdgeDetection.boolValue)
                {
                    EditorGUILayout.PropertyField(_AcuteAngleFix);
                    if (_AcuteAngleFix.boolValue)
                    {
                        EditorGUILayout.PropertyField(_ViewDirThreshold);
                    }
                    EditorGUILayout.PropertyField(_DepthThreshold);
                    EditorGUILayout.PropertyField(_DepthHardness);
                    EditorGUILayout.PropertyField(_DepthPower);
                }
            }
        }

        private void DrawEdgeBlendSettings()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Edge Blend Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(_EdgeColor);
                EditorGUILayout.PropertyField(_UseEdgeBlendDepthFade);
                if (_UseEdgeBlendDepthFade.boolValue)
                {
                    EditorGUILayout.PropertyField(_EdgeBlendFadeStart);
                    EditorGUILayout.PropertyField(_EdgeBlendFadeEnd);
                }
                EditorGUILayout.PropertyField(_UseSketchEdges);
                if (_UseSketchEdges.boolValue)
                {
                    EditorGUILayout.PropertyField(_Amplitude);
                    EditorGUILayout.PropertyField(_Frequency);
                    EditorGUILayout.PropertyField(_ChangesPerSecond);
                    
                }
                EditorGUILayout.PropertyField(_UseGrain);
                if (_UseGrain.boolValue)
                {
                    EditorGUILayout.PropertyField(_GrainTexture);
                    EditorGUILayout.PropertyField(_GrainStrength);
                    EditorGUILayout.PropertyField(_GrainScale);
                }
                EditorGUILayout.PropertyField(_UseUvOffset);
                if (_UseUvOffset.boolValue)
                {
                    EditorGUILayout.PropertyField(_OffsetNoise);
                    EditorGUILayout.PropertyField(_OffsetNoiseScale);
                    EditorGUILayout.PropertyField(_OffsetChangesPerSecond);
                    EditorGUILayout.PropertyField(_OffsetStrength);
                }
            }
        }

    }
}