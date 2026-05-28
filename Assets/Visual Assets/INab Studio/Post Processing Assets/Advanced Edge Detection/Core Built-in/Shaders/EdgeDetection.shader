// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/INab/EdgeDetection/EdgeDetection"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_ResolutionAdjust("ResolutionAdjust", Int) = 0
		_Thickness("Thickness", Float) = 0
		_NormalsOffset("NormalsOffset", Float) = 0
		_NormalsPower("NormalsPower", Float) = 0
		_NormalsHardness("NormalsHardness", Float) = 0
		[Toggle(_NORMALSEDGES_ON)] _NormalsEdges("NormalsEdges", Float) = 0
		[Toggle(_DEPTHEDGES_ON)] _DepthEdges("DepthEdges", Float) = 0
		_FadeStart("FadeStart", Float) = 0
		[Toggle(_USEDEPTHFADE_ON)] _UseDepthFade("UseDepthFade", Float) = 0
		_FadeEnd("FadeEnd", Float) = 0
		_DepthThreshold("DepthThreshold", Float) = 0
		_DepthPower("DepthPower", Float) = 0
		_DepthHardness("DepthHardness", Float) = 0
		_ViewDirThreshold("ViewDirThreshold", Float) = 0
		[Toggle(_ACUTEANGLESFIX_ON)] _AcuteAnglesFix("AcuteAnglesFix", Float) = 0

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#pragma shader_feature_local _NORMALSEDGES_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _DEPTHEDGES_ON
			#pragma shader_feature_local _ACUTEANGLESFIX_ON


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform sampler2D _CameraDepthNormalsTexture;
			uniform int _ResolutionAdjust;
			uniform float _Thickness;
			uniform float _NormalsOffset;
			uniform float _NormalsPower;
			uniform float _NormalsHardness;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _FadeStart;
			uniform float _FadeEnd;
			uniform float _DepthThreshold;
			uniform float _ViewDirThreshold;
			uniform float _DepthPower;
			uniform float _DepthHardness;
			float2 UnStereo( float2 UV )
			{
				#if UNITY_SINGLE_PASS_STEREO
				float4 scaleOffset = unity_StereoScaleOffset[ unity_StereoEyeIndex ];
				UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
				#endif
				return UV;
			}
			
			float3 InvertDepthDir72_g5( float3 In )
			{
				float3 result = In;
				#if !defined(ASE_SRP_VERSION) || ASE_SRP_VERSION <= 70301
				result *= float3(1,1,-1);
				#endif
				return result;
			}
			


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 appendResult8 = (float2(( 1.0 / _ScreenParams.x ) , ( 1.0 / _ScreenParams.y )));
				float2 temp_output_9_0 = ( appendResult8 * ( (float)_ResolutionAdjust == 1.0 ? ( ( _ScreenParams.y / 1080.0 ) * _Thickness ) : _Thickness ) );
				float2 temp_output_50_0 = ( ( float2( 1,1 ) * temp_output_9_0 ) + i.uv.xy );
				float depthDecodedVal31 = 0;
				float3 normalDecodedVal31 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_50_0 ), depthDecodedVal31, normalDecodedVal31 );
				float2 temp_output_48_0 = ( i.uv.xy + ( float2( -1,-1 ) * temp_output_9_0 ) );
				float depthDecodedVal1 = 0;
				float3 normalDecodedVal1 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_48_0 ), depthDecodedVal1, normalDecodedVal1 );
				float3 temp_output_34_0 = ( normalDecodedVal31 - normalDecodedVal1 );
				float dotResult37 = dot( temp_output_34_0 , temp_output_34_0 );
				float2 temp_output_52_0 = ( ( float2( -1,1 ) * temp_output_9_0 ) + i.uv.xy );
				float depthDecodedVal33 = 0;
				float3 normalDecodedVal33 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_52_0 ), depthDecodedVal33, normalDecodedVal33 );
				float2 temp_output_51_0 = ( ( float2( 1,-1 ) * temp_output_9_0 ) + i.uv.xy );
				float depthDecodedVal32 = 0;
				float3 normalDecodedVal32 = float3(0,0,0);
				DecodeDepthNormal( tex2D( _CameraDepthNormalsTexture, temp_output_51_0 ), depthDecodedVal32, normalDecodedVal32 );
				float3 temp_output_35_0 = ( normalDecodedVal33 - normalDecodedVal32 );
				float dotResult38 = dot( temp_output_35_0 , temp_output_35_0 );
				float smoothstepResult40 = smoothstep( ( dotResult37 + dotResult38 ) , 0.0 , _NormalsOffset);
				float temp_output_1_0_g3 = _NormalsHardness;
				#ifdef _NORMALSEDGES_ON
				float staticSwitch46 = ( ( pow( smoothstepResult40 , _NormalsPower ) - temp_output_1_0_g3 ) / ( 1.0 - temp_output_1_0_g3 ) );
				#else
				float staticSwitch46 = 0.0;
				#endif
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth63 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float smoothstepResult60 = smoothstep( 0.0 , 1.0 , pow( ( max( ( eyeDepth63 - _FadeStart ) , 0.0 ) / _FadeEnd ) , 2.0 ));
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch64 = saturate( smoothstepResult60 );
				#else
				float staticSwitch64 = 0.0;
				#endif
				float lerpResult65 = lerp( staticSwitch46 , 0.0 , staticSwitch64);
				float clampDepth85 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( temp_output_50_0, 0.0 , 0.0 ).xy );
				float clampDepth84 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( temp_output_48_0, 0.0 , 0.0 ).xy );
				float clampDepth87 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( temp_output_52_0, 0.0 , 0.0 ).xy );
				float clampDepth86 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, float4( temp_output_51_0, 0.0 , 0.0 ).xy );
				float2 UV22_g6 = ase_screenPosNorm.xy;
				float2 localUnStereo22_g6 = UnStereo( UV22_g6 );
				float2 break64_g5 = localUnStereo22_g6;
				float clampDepth69_g5 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy );
				#ifdef UNITY_REVERSED_Z
				float staticSwitch38_g5 = ( 1.0 - clampDepth69_g5 );
				#else
				float staticSwitch38_g5 = clampDepth69_g5;
				#endif
				float3 appendResult39_g5 = (float3(break64_g5.x , break64_g5.y , staticSwitch38_g5));
				float4 appendResult42_g5 = (float4((appendResult39_g5*2.0 + -1.0) , 1.0));
				float4 temp_output_43_0_g5 = mul( unity_CameraInvProjection, appendResult42_g5 );
				float3 temp_output_46_0_g5 = ( (temp_output_43_0_g5).xyz / (temp_output_43_0_g5).w );
				float3 In72_g5 = temp_output_46_0_g5;
				float3 localInvertDepthDir72_g5 = InvertDepthDir72_g5( In72_g5 );
				float4 appendResult49_g5 = (float4(localInvertDepthDir72_g5 , 1.0));
				float4 temp_output_112_0 = mul( unity_CameraToWorld, appendResult49_g5 );
				float4 normalizeResult111 = normalize( ( temp_output_112_0 - float4( _WorldSpaceCameraPos , 0.0 ) ) );
				float3 normalizeResult117 = normalize( cross( ddx( temp_output_112_0 ).xyz , ddy( temp_output_112_0 ).xyz ) );
				float dotResult116 = dot( normalizeResult111 , float4( normalizeResult117 , 0.0 ) );
				#ifdef _ACUTEANGLESFIX_ON
				float staticSwitch106 = ( ( ( 1.0 - saturate( dotResult116 ) ) * _ViewDirThreshold ) + 1.0 );
				#else
				float staticSwitch106 = 1.0;
				#endif
				float smoothstepResult77 = smoothstep( ( sqrt( ( pow( ( clampDepth85 - clampDepth84 ) , 2.0 ) + pow( ( clampDepth87 - clampDepth86 ) , 2.0 ) ) ) * 100.0 ) , 0.0 , ( clampDepth84 * _DepthThreshold * staticSwitch106 ));
				float temp_output_1_0_g4 = _DepthHardness;
				#ifdef _DEPTHEDGES_ON
				float staticSwitch82 = ( ( pow( smoothstepResult77 , _DepthPower ) - temp_output_1_0_g4 ) / ( 1.0 - temp_output_1_0_g4 ) );
				#else
				float staticSwitch82 = 0.0;
				#endif
				float4 temp_cast_21 = (saturate( max( lerpResult65 , staticSwitch82 ) )).xxxx;
				

				finalColor = temp_cast_21;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.CommentaryNode;15;-4319.353,-310.3046;Inherit;False;1192.62;699.802;texel size;10;6;7;8;9;11;10;5;13;14;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenParams;5;-4269.353,-247.3051;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-4055.98,72.59977;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1080;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-4020.511,276.8305;Inherit;False;Property;_Thickness;Thickness;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-2406.811,-833.7338;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-2377.201,-612.1464;Inherit;True;Property;_TextureSample1;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2370.379,-412.0219;Inherit;True;Property;_TextureSample2;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;29;-2370.379,-209.6237;Inherit;True;Property;_TextureSample3;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DecodeDepthNormalNode;1;-2020.51,-824.1042;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.DecodeDepthNormalNode;31;-1998.945,-640.1766;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.DecodeDepthNormalNode;32;-2003.661,-387.0692;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.DecodeDepthNormalNode;33;-2001.12,-192.4093;Inherit;False;1;0;FLOAT4;0,0,0,0;False;2;FLOAT;0;FLOAT3;1
Node;AmplifyShaderEditor.SimpleSubtractOpNode;34;-1658.377,-694.8051;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;-1681.049,-305.9538;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;37;-1449.218,-639.3442;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;38;-1475.308,-322.7011;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1289.117,-456.7108;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;40;-1031.631,-424.2491;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;-736.7057,-359.5599;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1256.593,-294.6585;Inherit;False;Property;_NormalsOffset;NormalsOffset;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-961.8191,-244.3709;Inherit;False;Property;_NormalsPower;NormalsPower;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-722.8191,-508.3708;Inherit;False;Property;_NormalsHardness;NormalsHardness;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;44;-455.0532,-368.527;Inherit;False;Inverse Lerp;-1;;3;09cbe79402f023141a4dc1fddd4c9511;0;3;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-3731.352,-260.3049;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-3716.996,-153.2356;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-3521.732,-181.6554;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-3304.733,-93.65525;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;11;-3894.732,-10.65531;Inherit;False;Property;_ResolutionAdjust;ResolutionAdjust;0;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.Compare;10;-3628.732,28.34472;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-3880.98,109.5997;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;17;-3235.38,-726.5313;Inherit;False;Constant;_Vector1;Vector 0;3;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;18;-3240.38,-609.5313;Inherit;False;Constant;_Vector2;Vector 0;3;0;Create;True;0;0;0;False;0;False;1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;16;-3233.35,-848.3569;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;-1,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;19;-3247.38,-477.5313;Inherit;False;Constant;_Vector3;Vector 0;3;0;Create;True;0;0;0;False;0;False;-1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2987.14,-398.9047;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2990.97,-539.9945;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-2978.753,-662.4914;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2995.753,-818.4914;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-2764.116,-829.2658;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-2726.116,-670.2659;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-2729.116,-496.2659;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-2730.116,-318.2658;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;49;-3052.14,-1019.976;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;26;-2767.545,-1145.209;Inherit;True;Global;_CameraDepthNormalsTexture;_CameraDepthNormalsTexture ;5;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.CommentaryNode;53;-1321.715,-1375.866;Inherit;False;1642.957;554.5698;Depth Fade;9;63;61;60;59;58;57;56;55;54;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;55;-907.1957,-1277.197;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;56;-760.5156,-1224.004;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;58;-566.5169,-1170.004;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;59;-377.56,-1121.3;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;60;-211.3139,-1049.921;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;65;676.9526,-569.6401;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;3;1066.415,-337.9138;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;4;897.4158,-320.9138;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;64;446.2427,-713.7401;Inherit;False;Property;_UseDepthFade;UseDepthFade;9;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-802.7148,-1091.086;Inherit;False;Property;_FadeEnd;FadeEnd;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1164.715,-1171.086;Inherit;False;Property;_FadeStart;FadeStart;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;61;24.11058,-967.2212;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;68;-1650.469,275.685;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;69;-1652.113,423.7973;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-1314.013,342.8635;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;73;-1178.013,368.8635;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;77;-635.1488,292.1564;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-891.9607,151.3801;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;78;-411.0359,338.3388;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;46;-221.2804,-307.834;Inherit;False;Property;_NormalsEdges;NormalsEdges;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-435.1396,200.0317;Inherit;False;Property;_DepthHardness;DepthHardness;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-627.7048,470.1463;Inherit;False;Property;_DepthPower;DepthPower;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-1186.326,228.1059;Inherit;False;Property;_DepthThreshold;DepthThreshold;11;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-1638.295,643.4489;Inherit;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;82;44.66286,201.3035;Inherit;False;Property;_DepthEdges;DepthEdges;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;88;-1472.212,429.4912;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;70;-1473.119,285.774;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;80;-232.4824,240.1364;Inherit;False;Inverse Lerp;-1;;4;09cbe79402f023141a4dc1fddd4c9511;0;3;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;86;-1926.856,467.6092;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;87;-1939.829,629.3929;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;84;-1928.306,227.1902;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;85;-1934.093,357.41;Inherit;False;1;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-992.5086,399.6545;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-1407.986,1294.477;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;104;-1238.986,1353.477;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-1196.287,1231.964;Inherit;False;Constant;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;106;-1024.286,1305.964;Inherit;False;Property;_AcuteAnglesFix;AcuteAnglesFix;15;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;109;-2881.552,1391.788;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;110;-3202.768,1499.738;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;111;-2698.564,1397.054;Inherit;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;112;-3281.772,1347.689;Inherit;False;Reconstruct World Position From Depth;-1;;5;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.DdxOpNode;113;-2901.462,1733.824;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DdyOpNode;114;-2901.561,1839.876;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CrossProductOpNode;115;-2683.941,1798.535;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;116;-2359.077,1457.69;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;117;-2493.643,1717.026;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;118;-2195.191,1495.216;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;119;-2052.679,1597.81;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-1675.116,1501.427;Inherit;False;Property;_ViewDirThreshold;ViewDirThreshold;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;63;-1150.524,-1325.866;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1277.542,-347.8273;Float;False;True;-1;2;ASEMaterialInspector;0;9;Hidden/INab/EdgeDetection/EdgeDetection;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;7;False;;False;True;0;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;13;0;5;2
WireConnection;24;0;26;0
WireConnection;24;1;48;0
WireConnection;27;0;26;0
WireConnection;27;1;50;0
WireConnection;28;0;26;0
WireConnection;28;1;51;0
WireConnection;29;0;26;0
WireConnection;29;1;52;0
WireConnection;1;0;24;0
WireConnection;31;0;27;0
WireConnection;32;0;28;0
WireConnection;33;0;29;0
WireConnection;34;0;31;1
WireConnection;34;1;1;1
WireConnection;35;0;33;1
WireConnection;35;1;32;1
WireConnection;37;0;34;0
WireConnection;37;1;34;0
WireConnection;38;0;35;0
WireConnection;38;1;35;0
WireConnection;39;0;37;0
WireConnection;39;1;38;0
WireConnection;40;0;41;0
WireConnection;40;1;39;0
WireConnection;42;0;40;0
WireConnection;42;1;43;0
WireConnection;44;1;45;0
WireConnection;44;3;42;0
WireConnection;6;1;5;1
WireConnection;7;1;5;2
WireConnection;8;0;6;0
WireConnection;8;1;7;0
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;10;0;11;0
WireConnection;10;2;14;0
WireConnection;10;3;12;0
WireConnection;14;0;13;0
WireConnection;14;1;12;0
WireConnection;20;0;19;0
WireConnection;20;1;9;0
WireConnection;21;0;18;0
WireConnection;21;1;9;0
WireConnection;22;0;17;0
WireConnection;22;1;9;0
WireConnection;23;0;16;0
WireConnection;23;1;9;0
WireConnection;48;0;49;0
WireConnection;48;1;23;0
WireConnection;50;0;22;0
WireConnection;50;1;49;0
WireConnection;51;0;21;0
WireConnection;51;1;49;0
WireConnection;52;0;20;0
WireConnection;52;1;49;0
WireConnection;55;0;63;0
WireConnection;55;1;54;0
WireConnection;56;0;55;0
WireConnection;58;0;56;0
WireConnection;58;1;57;0
WireConnection;59;0;58;0
WireConnection;60;0;59;0
WireConnection;65;0;46;0
WireConnection;65;2;64;0
WireConnection;3;0;4;0
WireConnection;4;0;65;0
WireConnection;4;1;82;0
WireConnection;64;0;61;0
WireConnection;61;0;60;0
WireConnection;68;0;85;0
WireConnection;68;1;84;0
WireConnection;69;0;87;0
WireConnection;69;1;86;0
WireConnection;72;0;70;0
WireConnection;72;1;88;0
WireConnection;73;0;72;0
WireConnection;77;0;75;0
WireConnection;77;1;74;0
WireConnection;75;0;84;0
WireConnection;75;1;76;0
WireConnection;75;2;106;0
WireConnection;78;0;77;0
WireConnection;78;1;79;0
WireConnection;46;0;44;0
WireConnection;82;0;80;0
WireConnection;88;0;69;0
WireConnection;88;1;83;0
WireConnection;70;0;68;0
WireConnection;70;1;83;0
WireConnection;80;1;81;0
WireConnection;80;3;78;0
WireConnection;86;0;51;0
WireConnection;87;0;52;0
WireConnection;84;0;48;0
WireConnection;85;0;50;0
WireConnection;74;0;73;0
WireConnection;102;0;119;0
WireConnection;102;1;99;0
WireConnection;104;0;102;0
WireConnection;106;1;107;0
WireConnection;106;0;104;0
WireConnection;109;0;112;0
WireConnection;109;1;110;0
WireConnection;111;0;109;0
WireConnection;113;0;112;0
WireConnection;114;0;112;0
WireConnection;115;0;113;0
WireConnection;115;1;114;0
WireConnection;116;0;111;0
WireConnection;116;1;117;0
WireConnection;117;0;115;0
WireConnection;118;0;116;0
WireConnection;119;0;118;0
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=E663B79AB5D394E5FAD52B695B2E64F352472E93