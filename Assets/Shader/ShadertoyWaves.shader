
Shader "Custom/ShadertoyWaves"
{
    
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = (i.uv * 2.0 - 1.0)
                    * float2(_ScreenParams.x / _ScreenParams.y, 1.0);

                float2 uv0 = uv;
                float3 finalColor = float3(0.0, 0.0, 0.0);

                float d = 0.0;

                for (int j = 0; j < 3; j++)
                {
                    uv = frac(uv * 2.0) - 0.5;

                    d = length(uv) * exp(-length(uv0));

                    d = sin(d * 8.0 + _Time.y) / 8.0;

                    d = pow(0.04 / max(abs(d), 0.0001), 2.2);
                }

                return float4(d - 0.2, d / 2.0, d, 1.0);
            }

            ENDCG
        }
    }
}
