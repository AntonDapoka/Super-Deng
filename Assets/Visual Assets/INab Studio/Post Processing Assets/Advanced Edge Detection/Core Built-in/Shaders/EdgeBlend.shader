// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/INab/EdgeDetection/EdgeBlend"
{
	Properties
	{
		_EdgeColor("_EdgeColor", Color) = (0,0,0,0)
		_GrainScale("GrainScale", Float) = 0
		_GrainTexture("_GrainTexture", 2D) = "white" {}
		_GrainStrength("GrainStrength", Float) = 0
		[Toggle(_USEGRAIN_ON)] _UseGrain("UseGrain", Float) = 0
		[Toggle(_USEDEPTHFADE_ON)] _UseDepthFade("UseDepthFade", Float) = 0
		[Toggle(_USEUVOFFSET_ON)] _UseUvOffset("UseUvOffset", Float) = 0
		[Toggle(_USESKETCHEDGES_ON)] _UseSketchEdges("UseSketchEdges", Float) = 0
		_EdgeBlendFadeStart("_EdgeBlendFadeStart", Float) = 0
		_EdgeBlendFadeEnd("_EdgeBlendFadeEnd", Float) = 0
		_OffsetChangesPerSecond("OffsetChangesPerSecond", Float) = 0
		_ChangesPerSecond("ChangesPerSecond", Float) = 0
		_OffsetStrength("OffsetStrength", Float) = 0
		_OffsetNoiseScale("_OffsetNoiseScale", Float) = 0
		_OffsetNoise("OffsetNoise", 2D) = "white" {}
		_Amplitude("Amplitude", Float) = 0
		_Frequency("Frequency", Float) = 0

	}

		CGINCLUDE

#include "UnityCG.cginc"
#include "UnityShaderVariables.cginc"

#pragma vertex Vert
#pragma fragment Frag
#pragma target 3.0

#include "UnityCG.cginc"
#include "UnityShaderVariables.cginc"
#define ASE_NEEDS_FRAG_SCREEN_POSITION_NORMALIZED
#pragma shader_feature_local _USESKETCHEDGES_ON
#pragma shader_feature_local _USEUVOFFSET_ON
#pragma shader_feature_local _USEDEPTHFADE_ON
#pragma shader_feature_local _USEGRAIN_ON




			struct ASEAttributesDefault
		{
			float4 positionOS : POSITION;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID


		};

		struct ASEVaryingsDefault
		{
			float4 positionCS : SV_POSITION;
			float3 positionVS : TEXCOORD1;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO

		};

		uniform sampler2D _ScreenImage;
		uniform float4 _ScreenImage_ST;
		uniform float4 _EdgeColor;
		uniform sampler2D _EdgeRT;
		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _EdgeBlendFadeStart;
		uniform float _EdgeBlendFadeEnd;
		uniform float _OffsetChangesPerSecond;
		uniform float _OffsetStrength;
		uniform sampler2D _OffsetNoise;
		uniform float _OffsetNoiseScale;
		uniform float _Frequency;
		uniform float _ChangesPerSecond;
		uniform float _Amplitude;
		uniform sampler2D _GrainTexture;
		uniform float _GrainScale;
		uniform float _GrainStrength;



		float2 TransformTriangleVertexToUV(float2 vertex)
		{
			float2 uv = (vertex + 1.0) * 0.5;
			return uv;
		}

		ASEVaryingsDefault Vert(ASEAttributesDefault v)
		{
			ASEVaryingsDefault output = (ASEVaryingsDefault)0;

			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);


			float4 positionCS = UnityObjectToClipPos(v.positionOS.xyz);

			output.positionCS = positionCS;

			output.positionVS = ComputeScreenPos(positionCS);

			output.uv = v.uv;




			return output;
		}

		float4 Frag(ASEVaryingsDefault i) : SV_Target
		{
			float4 ase_ppsScreenPosFragNorm = float4(i.uv,0,1);

			float2 uv_ScreenImage = i.uv.xy * _ScreenImage_ST.xy + _ScreenImage_ST.zw;
			float eyeDepth32 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, ase_ppsScreenPosFragNorm.xy));
			float smoothstepResult39 = smoothstep(0.0 , 1.0 , pow((max((eyeDepth32 - _EdgeBlendFadeStart) , 0.0) / _EdgeBlendFadeEnd) , 2.0));
			#ifdef _USEDEPTHFADE_ON
			float staticSwitch43 = (1.0 - saturate(smoothstepResult39));
			#else
			float staticSwitch43 = 1.0;
			#endif
			#ifdef _USEUVOFFSET_ON
			float staticSwitch61 = (staticSwitch43 * ((floor((frac((_OffsetChangesPerSecond * 0.5 * _Time.y)) * 2.0)) - 0.5) * 2.0 * _OffsetStrength * (-1.0 + (tex2D(_OffsetNoise, (i.uv.xy * _OffsetNoiseScale)).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0))));
			#else
			float staticSwitch61 = 0.0;
			#endif
			float temp_output_80_0 = (i.uv.xy.x * _Frequency);
			float temp_output_73_0 = floor((frac((_ChangesPerSecond * 0.5 * _Time.y)) * 2.0));
			float temp_output_86_0 = (temp_output_73_0 + 0.0);
			float temp_output_65_0 = (_Amplitude * staticSwitch43);
			float temp_output_79_0 = (i.uv.xy.y * _Frequency);
			float2 appendResult96 = (float2((sin((temp_output_80_0 + temp_output_86_0)) * temp_output_65_0) , (sin((temp_output_79_0 + temp_output_86_0)) * temp_output_65_0)));
			float temp_output_74_0 = (temp_output_73_0 + 123.0);
			float2 appendResult97 = (float2((sin((temp_output_80_0 + temp_output_74_0)) * temp_output_65_0) , (sin((temp_output_79_0 + temp_output_74_0)) * temp_output_65_0)));
			#ifdef _USESKETCHEDGES_ON
			float staticSwitch23 = saturate((tex2D(_EdgeRT, (appendResult96 + i.uv.xy + staticSwitch61)).r + tex2D(_EdgeRT, (appendResult97 + i.uv.xy + staticSwitch61)).r));
			#else
			float staticSwitch23 = tex2D(_EdgeRT, (staticSwitch61 + i.uv.xy)).r;
			#endif
			float lerpResult18 = lerp(1.0 , tex2D(_GrainTexture, (i.uv.xy * _GrainScale)).r , _GrainStrength);
			float lerpResult178 = lerp(1.0 , lerpResult18 , staticSwitch43);
			#ifdef _USEGRAIN_ON
			float staticSwitch20 = lerpResult178;
			#else
			float staticSwitch20 = 1.0;
			#endif
			float4 lerpResult9 = lerp(tex2D(_ScreenImage, uv_ScreenImage) , _EdgeColor , (_EdgeColor.a * (staticSwitch23 * staticSwitch20)));


			float4 color = lerpResult9;

			return color;
		}
			ENDCG

	SubShader
	{
		

			ZWrite Off
				Lighting Off
				Blend SrcAlpha OneMinusSrcAlpha

			Pass
		{
			 Name "EdgeDetection_NoStencil"

			CGPROGRAM


			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			
			#pragma shader_feature_local _USESKETCHEDGES_ON
			#pragma shader_feature_local _USEUVOFFSET_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _USEGRAIN_ON


			ENDCG
		}

			Pass
		{
			Name "EdgeDetection_StencilNotEqual"

			Stencil
			{
				Ref 1
				Comp NotEqual
			}

			CGPROGRAM


			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0

			#pragma shader_feature_local _USESKETCHEDGES_ON
			#pragma shader_feature_local _USEUVOFFSET_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _USEGRAIN_ON


			ENDCG
		}

			Pass
		{
			Name "EdgeDetection_StencilEqual"

			 Stencil
			 {
				 Ref 1
				 Comp Equal
			 }

			CGPROGRAM


			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0

			#pragma shader_feature_local _USESKETCHEDGES_ON
			#pragma shader_feature_local _USEUVOFFSET_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _USEGRAIN_ON


			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
