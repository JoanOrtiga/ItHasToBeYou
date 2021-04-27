Shader "Custom/SymbolsCutout"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _CutTex ("Cutout (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

     SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert alpha:fade

        sampler2D _CardAtlasTex;
        float4 _ColorUVZone;

        struct Input {
            float2 uv_CardAtlasTex;
            float3 vertexColor;
            float4 screenPos : TEXCOORD1;
        };

        void vert (inout appdata_full v, out Input o) { // http://answers.unity3d.com/questions/923726/unity-5-standard-shader-support-for-vertex-colors.html
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.vertexColor = v.color;
        }

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 card = tex2D (_MainTex, float2(IN.screenPos.x, IN.screenPos.y));
            o.Albedo = card;
            o.Alpha = 1; // make it opaque to start with

            // if this is a pip, colourize it
            if (IN.uv_CardAtlasTex.x >= _ColorUVZone.x && IN.uv_CardAtlasTex.y >= _ColorUVZone.y && IN.uv_CardAtlasTex.x <= _ColorUVZone.z && IN.uv_CardAtlasTex.y <= _ColorUVZone.w) {
                o.Albedo *= IN.vertexColor; // colourized
                o.Alpha = card.a; // transparented
            }
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}