Shader /*ase_name*/ "Hidden/Templates/INabStudio/BIRPBlit" /*end*/
{
	Properties
	{
		/*ase_props*/
	}

	SubShader
	{
		ZWrite Off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		/*ase_pass*/
		Pass
		{
			/*ase_main_pass*/
			Name "Screen Fog Blend"
			CGPROGRAM

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			/*ase_pragma*/
		


			struct ASEAttributesDefault
			{
			float4 positionOS : POSITION;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		
				/*ase_vdata:p=p;uv0=tc0*/
			};

			struct ASEVaryingsDefault
			{
				float4 positionCS : SV_POSITION;
				float3 positionVS : TEXCOORD1;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
				/*ase_interp(2,):sp=sp.xyzw;uv0=tc0.xy;uv1=tc1;uv2=tc2*/
			};

			/*ase_globals*/

			/*ase_funcs*/

			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v /*ase_vert_input*/ )
			{
				ASEVaryingsDefault output = (ASEVaryingsDefault	)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);


				float4 positionCS = UnityObjectToClipPos(v.positionOS.xyz);

				output.positionCS = positionCS;

				output.positionVS = ComputeScreenPos(positionCS);

				output.uv = v.uv;
				

				/*ase_vert_code:v=ASEAttributesDefault;o=ASEVaryingsDefault*/

				return output;
			}

			float4 Frag (ASEVaryingsDefault i /*ase_frag_input*/ ) : SV_Target
			{
				/*ase_local_var:spn*/float4 ase_ppsScreenPosFragNorm = float4(i.uv,0,1);

				/*ase_frag_code:i=ASEVaryingsDefault*/

				float4 color = /*ase_frag_out:Frag Color;Float4*/float4(0,0,0,0)/*end*/;
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
