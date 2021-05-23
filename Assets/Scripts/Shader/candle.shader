Shader "Unlit/candle"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _iResolutionX("_iResolutionX", float) = 1920.0
        _iResolutionY("_iResolutionY", float) = 1080.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _iResolutionX;
            float _iResolutionY;

            float2x2 rotz(float angle)
            {
                float2x2 m;
                m[0][0] = cos(angle); m[0][1] = -sin(angle);
                m[1][0] = sin(angle); m[1][1] = cos(angle);
                return m;
            }

            float rand(float2 co) {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }


            float fbm(float2 uv)
            {
                float n = (tex2D(_MainTex, uv).r - 0.5) * 0.5;
                n += (tex2D(_MainTex, uv * 2.0).r - 0.5) * 0.5 * 0.5;
                n += (tex2D(_MainTex, uv * 3.0).r - 0.5) * 0.5 * 0.5 * 0.5;

                return n + 0.5;
            }


            // -----------------------------------------------
            float4 mainImage( in float2 fragCoord)
            {
                //float2 uv = fragCoord.xy / iResolution.xy;
                float2 uv = fragCoord;
                float2 _uv = uv;
                uv -= float2(0.5,0.0);
                uv.y /= _iResolutionX / _iResolutionY;
                float2 centerUV = uv;

                // height variation from fbm
                float variationH = fbm(float2(_Time.x * 0.3,0.0)) * 1.1;

                // flame "speed"
                float2 offset = float2(0.0, -_Time.x * 0.15);

                // flame turbulence
                float f = fbm(uv * 0.1 + offset); // rotation from fbm
                float l = max(0.1, length(uv)); // rotation amount normalized over distance
                uv += mul(rotz(((f - 0.5) / l) * smoothstep(-0.2, 0.4, _uv.y) * 0.45) , uv);

                // flame thickness
                float flame = 1.3 - length(uv.x) * 5.0;

                // bottom of flame 
                float blueflame = pow(flame * .9, 15.0);
                blueflame *= smoothstep(.2, -1.0, _uv.y);
                blueflame /= abs(uv.x * 2.0);
                blueflame = clamp(blueflame, 0.0, 1.0);

                // flame
                flame *= smoothstep(1., variationH * 0.5, _uv.y);
                flame = clamp(flame, 0.0, 1.0);
                flame = pow(flame, 3.);
                flame /= smoothstep(1.1, -0.1, _uv.y);

                // colors
                float4 col = lerp(float4(1.0, 1.0, 0.0, 0.0), float4(1.0, 1.0, 0.6, 0.0), flame);
                col = lerp(float4(1.0, .0, 0.0, 0.0), col, smoothstep(0.0, 1.6, flame));
                float4 fragColor = col;

                // a bit blueness on the bottom
                float4 bluecolor = lerp(float4(0.0, 0.0, 1.0, 0.0), fragColor, 0.95);
                fragColor = lerp(fragColor, bluecolor, blueflame);

                // clear bg outside of the flame
                fragColor *= flame;
                fragColor.a = flame;

                // bg halo
                float haloSize = 0.5;
                float centerL = 1.0 - (length(centerUV + float2(0.0, 0.1)) / haloSize);
                float4 halo = float4(.8, .3, .3, 0.0) * 1.0 * fbm(float2(_Time.x * 0.035,0.0)) * centerL + 0.02;
                float4 finalCol = lerp(halo, fragColor, fragColor.a);
                fragColor = finalCol;

                // just a hint of noise
                fragColor *= lerp(rand(uv) + rand(uv * .45), 1.0, 0.9);
                fragColor = clamp(fragColor, 0.0, 1.0);

                return fragColor;
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                return mainImage(i.uv);
            }
            ENDCG
        }
    }
}
