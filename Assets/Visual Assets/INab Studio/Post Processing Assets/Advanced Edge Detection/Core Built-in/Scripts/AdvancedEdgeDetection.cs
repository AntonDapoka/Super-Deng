using System;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace INab.AdvancedEdgeDetection.BIRP
{
    [Serializable]

    public class EdgeDetectionSettings
    {
        public enum StencilUse { None = 0, NotEqual = 1, Equal = 2 }

        [SerializeField] public StencilUse _StencilUse = StencilUse.None;

        // Edge Detection properties

        // Main
        [Range(0, 5)]
        [SerializeField] public float _Thickness = 1.0f;
        [SerializeField] public bool _ResolutionAdjust = false;

        // Depth Fade
        [SerializeField] public bool _UseDepthFade = false;
        [SerializeField] public float _FadeStart = 20;
        [SerializeField] public float _FadeEnd = 40;

        // Normals
        [SerializeField] public bool _NormalsEdgeDetection = true;
        [Range(.01f, 1.5f)]
        [SerializeField] public float _NormalsOffset = 0.1f;
        [Range(0, .99f)]
        [SerializeField] public float _NormalsHardness = 0;
        [Range(1, 5)]
        [SerializeField] public float _NormalsPower = 1;

        // Depth
        [SerializeField] public bool _DepthEdgeDetection = true;
        [SerializeField] public bool _AcuteAngleFix = false;
        [Range(0, 30)]
        [SerializeField] public float _ViewDirThreshold = 1;
        [Range(0, 3)]
        [SerializeField] public float _DepthThreshold = 1;
        [Range(0, 1)]
        [SerializeField] public float _DepthHardness = .9f;
        [Range(1, 5)]
        [SerializeField] public float _DepthPower = 5;


        // Edge Blend properties

        // Colors
        [SerializeField] public Color _EdgeColor = Color.black;

        [SerializeField] public bool _UseEdgeBlendDepthFade = false;
        [SerializeField] public float _EdgeBlendFadeStart = 10;
        [SerializeField] public float _EdgeBlendFadeEnd = 20;

        // Sketch
        [SerializeField] public bool _UseSketchEdges = false;
        [Range(0, .01f)]
        [SerializeField] public float _Amplitude = .005f;
        [Range(0, 150)]
        [SerializeField] public float _Frequency = 40;
        [Range(0, 10)]
        [SerializeField] public float _ChangesPerSecond = 0;


        // Grain
        [SerializeField] public bool _UseGrain = false;
        [SerializeField] public Texture2D _GrainTexture;
        [Range(0, 1)]
        [SerializeField] public float _GrainStrength = 1;
        [Range(0, 3)]
        [SerializeField] public float _GrainScale = 1;

        // UV Offset
        [SerializeField] public bool _UseUvOffset = false;
        [SerializeField] public Texture2D _OffsetNoise;
        [Range(0, 4)]
        [SerializeField] public float _OffsetNoiseScale = .4f;
        [Range(0, 10)]
        [SerializeField] public float _OffsetChangesPerSecond = 0;
        [Range(0, .01f)]
        [SerializeField] public float _OffsetStrength = .005f;

        // Custom Data properties
        [SerializeField] public bool _UseCustomTexture = false;
        [SerializeField] public Texture2D _CustomTexture;
    }

    //  ImageEffectAllowedInSceneView, 
    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class AdvancedEdgeDetection : MonoBehaviour
    {
        [SerializeField] private EdgeDetectionSettings m_settings = new EdgeDetectionSettings();

        public LayerMask _StencilMaskLayer;
        private List<Renderer> stencilRenderers = new List<Renderer>();

        private Material m_EdgeDetectionMaterial;
        private Material m_EdgeBlendMaterial;
        private Material m_StencilMaterial;

        private Camera cam;
        private Camera sceneCam;
        private CommandBuffer cmd;

        private void FindStencilRenderers()
        {
            stencilRenderers.Clear();

            var renderersArray = FindObjectsOfType<Renderer>(false);

            foreach (var renderer in renderersArray)
            {
                if (_StencilMaskLayer == (_StencilMaskLayer | (1 << renderer.gameObject.layer)))
                {
                    stencilRenderers.Add(renderer);
                }
            }
        }

        public void OnEnable()
        {
            GetMaterials();

            cam = GetComponent<Camera>();
            cam.depthTextureMode = DepthTextureMode.DepthNormals;

            cmd = new CommandBuffer();

            cmd.Clear();
            cmd.name = "Advanced Edge Detection";

            cmd.BeginSample(cmd.name);

            var screenImage = Shader.PropertyToID("_ScreenImage");
            cmd.GetTemporaryRT(screenImage, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBFloat);

            cmd.SetRenderTarget(screenImage);
            cmd.Blit(BuiltinRenderTextureType.CameraTarget, screenImage);



            // Edge RT
            int edgeRT = Shader.PropertyToID("_EdgeRT");
            cmd.GetTemporaryRT(edgeRT, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBFloat);

            // Edge Blits
            cmd.SetRenderTarget(edgeRT);
            cmd.Blit(screenImage, edgeRT, m_EdgeDetectionMaterial);

            
            // Stencil
            if (m_settings._StencilUse != EdgeDetectionSettings.StencilUse.None)
            {
                cmd.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);

                FindStencilRenderers();
                foreach (var renderer in stencilRenderers)
                {
                    for (int submeshIndex = 0; submeshIndex < renderer.sharedMaterials.Length; submeshIndex++)
                    {
                        cmd.DrawRenderer(renderer, m_StencilMaterial, submeshIndex, 0);
                    }
                }
            }
            // Stencil
            

            // Main blit

            cmd.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            cmd.Blit(screenImage, BuiltinRenderTextureType.CameraTarget, m_EdgeBlendMaterial, (int)m_settings._StencilUse);
           
            cmd.ReleaseTemporaryRT(screenImage);
            cmd.ReleaseTemporaryRT(edgeRT);

            cmd.EndSample(cmd.name);

            cam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, cmd);
#if UNITY_EDITOR
            if(SceneView.GetAllSceneCameras().Length > 0)
            {
                sceneCam = SceneView.GetAllSceneCameras()[0];
                sceneCam.depthTextureMode = DepthTextureMode.DepthNormals;

                sceneCam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, cmd);
            }
#endif
        }

        public void OnDisable()
        {
            if (m_EdgeBlendMaterial)
            {
                DestroyImmediate(m_EdgeBlendMaterial);
            }

            if (m_EdgeDetectionMaterial)
            {
                DestroyImmediate(m_EdgeDetectionMaterial);
            }

            if (m_StencilMaterial)
            {
                DestroyImmediate(m_StencilMaterial);
            }

            cam.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, cmd);

#if UNITY_EDITOR
            if(sceneCam) sceneCam.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, cmd);
#endif
        }

        private void Update()
        {
            SetEdgeDetectionProperties();
            SetEdgeBlendProperties();
        }

        private void SetEdgeDetectionProperties()
        {
            // Main
            m_EdgeDetectionMaterial.SetFloat("_Thickness", m_settings._Thickness);
            m_EdgeDetectionMaterial.SetInt("_ResolutionAdjust", m_settings._ResolutionAdjust ? 1 : 0);

            // Depth Fade
            if (m_settings._UseDepthFade)
            {
                m_EdgeDetectionMaterial.EnableKeyword("_USEDEPTHFADE_ON");
                m_EdgeDetectionMaterial.SetFloat("_FadeStart", m_settings._FadeStart);
                m_EdgeDetectionMaterial.SetFloat("_FadeEnd", m_settings._FadeEnd);
            }
            else
            {
                m_EdgeDetectionMaterial.DisableKeyword("_USEDEPTHFADE_ON");
            }

            // Normals
            if (m_settings._NormalsEdgeDetection)
            {
                m_EdgeDetectionMaterial.EnableKeyword("_NORMALSEDGES_ON");
                m_EdgeDetectionMaterial.SetFloat("_NormalsOffset", m_settings._NormalsOffset);
                m_EdgeDetectionMaterial.SetFloat("_NormalsHardness", m_settings._NormalsHardness);
                m_EdgeDetectionMaterial.SetFloat("_NormalsPower", m_settings._NormalsPower);
            }
            else
            {
                m_EdgeDetectionMaterial.DisableKeyword("_NORMALSEDGES_ON");
            }

            // Depth
            if (m_settings._DepthEdgeDetection)
            {
                m_EdgeDetectionMaterial.EnableKeyword("_DEPTHEDGES_ON");
                m_EdgeDetectionMaterial.SetFloat("_ViewDirThreshold", m_settings._ViewDirThreshold);
                m_EdgeDetectionMaterial.SetFloat("_DepthThreshold", m_settings._DepthThreshold);
                m_EdgeDetectionMaterial.SetFloat("_DepthHardness", m_settings._DepthHardness);
                m_EdgeDetectionMaterial.SetFloat("_DepthPower", m_settings._DepthPower);
            }
            else
            {
                m_EdgeDetectionMaterial.DisableKeyword("_DEPTHEDGES_ON");
            }

            if (m_settings._AcuteAngleFix)
            {
                m_EdgeDetectionMaterial.EnableKeyword("_ACUTEANGLESFIX_ON");
            }
            else
            {
                m_EdgeDetectionMaterial.DisableKeyword("_ACUTEANGLESFIX_ON");
            }
        }

        private void SetEdgeBlendProperties()
        {
            // Colors
            m_EdgeBlendMaterial.SetColor("_EdgeColor", m_settings._EdgeColor);

            // Sketch
            if (m_settings._UseSketchEdges)
            {
                m_EdgeBlendMaterial.EnableKeyword("_USESKETCHEDGES_ON");
                m_EdgeBlendMaterial.SetFloat("_Amplitude", m_settings._Amplitude);
                m_EdgeBlendMaterial.SetFloat("_Frequency", m_settings._Frequency);
                m_EdgeBlendMaterial.SetFloat("_ChangesPerSecond", m_settings._ChangesPerSecond);
            }
            else
            {
                m_EdgeBlendMaterial.DisableKeyword("_USESKETCHEDGES_ON");
            }

            if (m_settings._UseEdgeBlendDepthFade)
            {
                m_EdgeBlendMaterial.EnableKeyword("_USEDEPTHFADE_ON");
                m_EdgeBlendMaterial.SetFloat("_EdgeBlendFadeStart", m_settings._EdgeBlendFadeStart);
                m_EdgeBlendMaterial.SetFloat("_EdgeBlendFadeEnd", m_settings._EdgeBlendFadeEnd);
            }
            else
            {
                m_EdgeBlendMaterial.DisableKeyword("_USEDEPTHFADE_ON");
            }

            // Grain
            if (m_settings._UseGrain)
            {
                m_EdgeBlendMaterial.EnableKeyword("_USEGRAIN_ON");
                m_EdgeBlendMaterial.SetTexture("_GrainTexture", m_settings._GrainTexture);
                m_EdgeBlendMaterial.SetFloat("_GrainStrength", m_settings._GrainStrength);
                m_EdgeBlendMaterial.SetFloat("_GrainScale", m_settings._GrainScale);
            }
            else
            {
                m_EdgeBlendMaterial.DisableKeyword("_USEGRAIN_ON");
            }

            // UV Offset
            if (m_settings._UseUvOffset)
            {
                m_EdgeBlendMaterial.EnableKeyword("_USEUVOFFSET_ON");
                m_EdgeBlendMaterial.SetTexture("_OffsetNoise", m_settings._OffsetNoise);
                m_EdgeBlendMaterial.SetFloat("_OffsetNoiseScale", m_settings._OffsetNoiseScale);
                m_EdgeBlendMaterial.SetFloat("_OffsetChangesPerSecond", m_settings._OffsetChangesPerSecond);
                m_EdgeBlendMaterial.SetFloat("_OffsetStrength", m_settings._OffsetStrength);
            }
            else
            {
                m_EdgeBlendMaterial.DisableKeyword("_USEUVOFFSET_ON");
            }
        }

        private void GetMaterials()
        {
            m_StencilMaterial = new Material(Shader.Find("Hidden/INab/EdgeDetection/Stencil"));

            m_EdgeDetectionMaterial = new Material(Shader.Find("Hidden/INab/EdgeDetection/EdgeDetection"));
            m_EdgeBlendMaterial = new Material(Shader.Find("Hidden/INab/EdgeDetection/EdgeBlend"));


        }
    }
}