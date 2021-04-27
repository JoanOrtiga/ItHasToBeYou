Shader "Custom/SymbolsTransparent"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MaskTex("Mask texture", 2D) = "white" {}
        _Cutout("Cutout", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"
        }
        /*
        Lighting Off
        Cull Back
       
        ZTest Less
        Fog{ Mode Off }*/
       
        //UsePass "Transparent/Diffuse/FORWARD"
        
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float _Cutout;

            fixed4 frag(v2f i) : SV_Target
            {
                i.screenPos /= i.screenPos.w;
              /*  fixed4 l_MaskColor = tex2D(_MaskTex, i.uv);
                if (l_MaskColor.a < _Cutout)
                    clip(-1);*/
                fixed4 col = tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));

                col = col > fixed4(0.1, 0.1, 0.1, 1) ? col : fixed4(0, 0, 0, 0.1);
                col.a = (step(0.9, col.x));
                return col;
            }
            
            ENDCG
        }
    }
}