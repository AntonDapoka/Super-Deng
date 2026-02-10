// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/INab/EdgeDetection/EdgeBlendTemplate"
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
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		ZWrite Off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		
		Pass
		{
			
			Name "Screen Fog Blend"
			CGPROGRAM

			

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
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
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


			
			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v  )
			{
				ASEVaryingsDefault output = (ASEVaryingsDefault	)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);


				float4 positionCS = UnityObjectToClipPos(v.positionOS.xyz);

				output.positionCS = positionCS;

				output.positionVS = ComputeScreenPos(positionCS);

				output.uv = v.uv;
				

				

				return output;
			}

			float4 Frag (ASEVaryingsDefault i  ) : SV_Target
			{
				float4 ase_ppsScreenPosFragNorm = float4(i.uv,0,1);

				float2 uv_ScreenImage = i.uv.xy * _ScreenImage_ST.xy + _ScreenImage_ST.zw;
				float eyeDepth32 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_ppsScreenPosFragNorm.xy ));
				float smoothstepResult39 = smoothstep( 0.0 , 1.0 , pow( ( max( ( eyeDepth32 - _EdgeBlendFadeStart ) , 0.0 ) / _EdgeBlendFadeEnd ) , 2.0 ));
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch43 = ( 1.0 - saturate( smoothstepResult39 ) );
				#else
				float staticSwitch43 = 1.0;
				#endif
				#ifdef _USEUVOFFSET_ON
				float staticSwitch61 = ( staticSwitch43 * ( ( floor( ( frac( ( _OffsetChangesPerSecond * 0.5 * _Time.y ) ) * 2.0 ) ) - 0.5 ) * 2.0 * _OffsetStrength * (-1.0 + (tex2D( _OffsetNoise, ( i.uv.xy * _OffsetNoiseScale ) ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) );
				#else
				float staticSwitch61 = 0.0;
				#endif
				float temp_output_80_0 = ( i.uv.xy.x * _Frequency );
				float temp_output_73_0 = floor( ( frac( ( _ChangesPerSecond * 0.5 * _Time.y ) ) * 2.0 ) );
				float temp_output_86_0 = ( temp_output_73_0 + 0.0 );
				float temp_output_65_0 = ( _Amplitude * staticSwitch43 );
				float temp_output_79_0 = ( i.uv.xy.y * _Frequency );
				float2 appendResult96 = (float2(( sin( ( temp_output_80_0 + temp_output_86_0 ) ) * temp_output_65_0 ) , ( sin( ( temp_output_79_0 + temp_output_86_0 ) ) * temp_output_65_0 )));
				float temp_output_74_0 = ( temp_output_73_0 + 123.0 );
				float2 appendResult97 = (float2(( sin( ( temp_output_80_0 + temp_output_74_0 ) ) * temp_output_65_0 ) , ( sin( ( temp_output_79_0 + temp_output_74_0 ) ) * temp_output_65_0 )));
				#ifdef _USESKETCHEDGES_ON
				float staticSwitch23 = saturate( ( tex2D( _EdgeRT, ( appendResult96 + i.uv.xy + staticSwitch61 ) ).r + tex2D( _EdgeRT, ( appendResult97 + i.uv.xy + staticSwitch61 ) ).r ) );
				#else
				float staticSwitch23 = tex2D( _EdgeRT, ( staticSwitch61 + i.uv.xy ) ).r;
				#endif
				float lerpResult18 = lerp( 1.0 , tex2D( _GrainTexture, ( i.uv.xy * _GrainScale ) ).r , _GrainStrength);
				float lerpResult178 = lerp( 1.0 , lerpResult18 , staticSwitch43);
				#ifdef _USEGRAIN_ON
				float staticSwitch20 = lerpResult178;
				#else
				float staticSwitch20 = 1.0;
				#endif
				float4 lerpResult9 = lerp( tex2D( _ScreenImage, uv_ScreenImage ) , _EdgeColor , ( _EdgeColor.a * ( staticSwitch23 * staticSwitch20 ) ));
				

				float4 color = lerpResult9;
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.CommentaryNode;52;-5337.425,1323.339;Inherit;False;895.1426;352.2614;uv random offset;6;47;48;49;50;51;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;42;-5930.938,648.6601;Inherit;False;1642.957;554.5698;Depth Fade;10;32;33;34;36;35;37;38;39;40;41;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;22;-1813.122,1279.189;Inherit;False;1243.064;445.0576;Grain;9;16;15;17;18;19;20;21;105;178;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;9;-123.0038,-85.05075;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;10;-561.1033,-59.05064;Inherit;False;Property;_EdgeColor;_EdgeColor;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-288.1036,134.7493;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-474.0036,292.0493;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1686.122,1555.943;Inherit;False;Property;_GrainScale;GrainScale;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1536.922,1412.642;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-1345.036,1329.189;Inherit;True;Property;_GrainTexture;_GrainTexture;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;-1019.868,1421.798;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1188.867,1555.798;Inherit;False;Property;_GrainStrength;GrainStrength;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-908.0584,1611.58;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-1680.288,104.7886;Inherit;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1339.313,175.0992;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;31;-1188.313,225.0992;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-1654.314,619.0709;Inherit;True;Property;_TextureSample2;Texture Sample 0;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-1669.53,338.3294;Inherit;True;Property;_TextureSample1;Texture Sample 0;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;67;-5209.369,-380.7285;Inherit;False;895.1426;352.2614;uv random offset;6;73;72;70;69;68;71;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-4244.406,965.4943;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-4411.418,1621.932;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-4253.416,1686.932;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;2;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-4489.419,1788.932;Inherit;False;Property;_OffsetStrength;OffsetStrength;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-3723.417,1108.597;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;61;-3538.309,1032.543;Inherit;False;Property;_UseUvOffset;UseUvOffset;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-5069.425,1440.38;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;48;-5287.425,1535.38;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;49;-4866.423,1466.38;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-5341.735,1376.187;Inherit;False;Property;_OffsetChangesPerSecond;OffsetChangesPerSecond;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-4747.423,1519.38;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;51;-4588.281,1562.933;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-4941.368,-263.6875;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;69;-5159.369,-168.6875;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;70;-4738.367,-237.6875;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-4619.367,-184.6875;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;73;-4460.225,-141.1335;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-5070.679,-301.8805;Inherit;False;Property;_ChangesPerSecond;ChangesPerSecond;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-3892.616,-102.9544;Inherit;False;Property;_Frequency;Frequency;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;77;-3590.127,14.16564;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-3331.456,38.97891;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-3346.656,156.979;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-3121.452,71.0751;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-3130.452,197.0751;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-3118.452,327.0748;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-3127.452,453.0748;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;85;-3910.708,106.6516;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-3691.138,321.9212;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;123;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-3686.805,183.4444;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;87;-2967.66,118.0975;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;88;-2985.047,208.2954;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;89;-2958.965,328.9215;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;90;-2972.006,443.0272;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-2805.739,53.98097;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2787.264,337.6152;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2792.697,464.7616;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-3050.097,600.1339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-3293.303,631.2935;Inherit;False;Property;_Amplitude;Amplitude;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2794.13,190.561;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;96;-2603.125,91.09352;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;97;-2611.125,353.0935;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-2908.376,1231.65;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;-2263.247,618.5214;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;-2235.384,264.2639;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;105;-1811.406,1331.55;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;106;-2521.571,831.9975;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;107;-3099.503,1393.227;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;25;-1937.587,-162.5458;Inherit;True;Global;_EdgeRT;EdgeRT;8;0;Create;True;0;0;0;False;0;False;None;;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.StaticSwitch;43;-4123.405,1056.494;Inherit;False;Property;_UseDepthFade;UseDepthFade;5;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;33;-5516.419,747.3289;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;36;-5369.739,800.5211;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-5411.938,933.4392;Inherit;False;Property;_EdgeBlendFadeEnd;_EdgeBlendFadeEnd;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;37;-5175.74,854.5212;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;38;-4986.783,903.2258;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;39;-4820.537,974.6046;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;-4635.537,1042.605;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;41;-4465.98,1090.563;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;23;-987.1302,287.8542;Inherit;False;Property;_UseSketchEdges;UseSketchEdges;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-4919.014,1968.068;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;103;-5162.004,1899.127;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;59;-5142.907,2041.525;Inherit;False;Property;_OffsetNoiseScale;_OffsetNoiseScale;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-4702.858,1935.772;Inherit;True;Property;_OffsetNoise;OffsetNoise;15;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;32;-5759.747,698.6601;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-5880.938,887.4392;Inherit;False;Property;_EdgeBlendFadeStart;_EdgeBlendFadeStart;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;176;-4407.945,2010.499;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;20;-676.0584,1461.58;Inherit;False;Property;_UseGrain;UseGrain;4;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;178;-841.4173,1368.682;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;181;-401.9918,-311.8461;Inherit;False;Global;_GrabScreen0;Grab Screen 0;18;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;179;-477.321,-765.183;Inherit;True;Global;_TextureSample3;Texture Sample 3;18;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;186;-746.8086,-690.6248;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CustomExpressionNode;188;-1097.89,-558.9343;Inherit;False;#if UNITY_UV_STARTS_AT_TOP$return 1 - In@$#endif$return In@$;1;Create;1;True;In;FLOAT;0;In;;Inherit;False;My Custom Expression;True;False;0;;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;183;-1329.396,-756.7256;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;182;-806.381,-1156.88;Inherit;True;Global;_ScreenImage;_ScreenImage;18;0;Create;True;0;0;0;False;0;False;None;;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;191;279.6475,-233.853;Float;False;True;-1;2;ASEMaterialInspector;0;18;Hidden/INab/EdgeDetection/EdgeBlendTemplate;8268af943e8e93a4d81aac1a2a72e8fa;True;Screen Fog Blend;0;0;Screen Fog Blend;1;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.ColorNode;189;12.19944,-291.6838;Inherit;False;Constant;_Color0;Color 0;19;0;Create;True;0;0;0;False;0;False;0.6320754,0.0934199,0.5772265,0.2196078;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;9;0;179;0
WireConnection;9;1;10;0
WireConnection;9;2;12;0
WireConnection;12;0;10;4
WireConnection;12;1;13;0
WireConnection;13;0;23;0
WireConnection;13;1;20;0
WireConnection;15;0;105;0
WireConnection;15;1;16;0
WireConnection;17;1;15;0
WireConnection;18;1;17;1
WireConnection;18;2;19;0
WireConnection;26;0;25;0
WireConnection;26;1;100;0
WireConnection;30;0;26;1
WireConnection;30;1;27;1
WireConnection;31;0;30;0
WireConnection;29;0;25;0
WireConnection;29;1;62;0
WireConnection;27;0;25;0
WireConnection;27;1;98;0
WireConnection;53;0;51;0
WireConnection;54;0;53;0
WireConnection;54;2;55;0
WireConnection;54;3;176;0
WireConnection;60;0;43;0
WireConnection;60;1;54;0
WireConnection;61;0;60;0
WireConnection;47;0;45;0
WireConnection;47;2;48;0
WireConnection;49;0;47;0
WireConnection;50;0;49;0
WireConnection;51;0;50;0
WireConnection;68;0;71;0
WireConnection;68;2;69;0
WireConnection;70;0;68;0
WireConnection;72;0;70;0
WireConnection;73;0;72;0
WireConnection;80;0;77;1
WireConnection;80;1;75;0
WireConnection;79;0;77;2
WireConnection;79;1;75;0
WireConnection;81;0;80;0
WireConnection;81;1;86;0
WireConnection;82;0;79;0
WireConnection;82;1;86;0
WireConnection;83;0;80;0
WireConnection;83;1;74;0
WireConnection;84;0;79;0
WireConnection;84;1;74;0
WireConnection;85;0;73;0
WireConnection;74;0;85;0
WireConnection;86;0;73;0
WireConnection;87;0;81;0
WireConnection;88;0;82;0
WireConnection;89;0;83;0
WireConnection;90;0;84;0
WireConnection;91;0;87;0
WireConnection;91;1;65;0
WireConnection;93;0;89;0
WireConnection;93;1;65;0
WireConnection;94;0;90;0
WireConnection;94;1;65;0
WireConnection;65;0;66;0
WireConnection;65;1;43;0
WireConnection;92;0;88;0
WireConnection;92;1;65;0
WireConnection;96;0;91;0
WireConnection;96;1;92;0
WireConnection;97;0;93;0
WireConnection;97;1;94;0
WireConnection;62;0;61;0
WireConnection;62;1;107;0
WireConnection;98;0;97;0
WireConnection;98;1;106;0
WireConnection;98;2;61;0
WireConnection;100;0;96;0
WireConnection;100;1;106;0
WireConnection;100;2;61;0
WireConnection;43;1;44;0
WireConnection;43;0;41;0
WireConnection;33;0;32;0
WireConnection;33;1;34;0
WireConnection;36;0;33;0
WireConnection;37;0;36;0
WireConnection;37;1;35;0
WireConnection;38;0;37;0
WireConnection;39;0;38;0
WireConnection;40;0;39;0
WireConnection;41;0;40;0
WireConnection;23;1;29;1
WireConnection;23;0;31;0
WireConnection;58;0;103;0
WireConnection;58;1;59;0
WireConnection;56;1;58;0
WireConnection;176;0;56;1
WireConnection;20;1;21;0
WireConnection;20;0;178;0
WireConnection;178;1;18;0
WireConnection;178;2;43;0
WireConnection;179;0;182;0
WireConnection;186;0;183;1
WireConnection;186;1;188;0
WireConnection;188;0;183;2
WireConnection;191;0;9;0
ASEEND*/
//CHKSM=ED2B489E69EA89D76B60B47768DDE8ABB1FB48E5