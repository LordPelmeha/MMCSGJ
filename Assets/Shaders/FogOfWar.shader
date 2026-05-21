Shader "HalfEmpty/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Fog Center (world units)", Vector) = (0,0,0,0)
        _InnerRadius ("Inner Radius", Float) = 3.0
        _OuterRadius ("Outer Radius", Float) = 5.0
        _OuterAlpha ("Outer Zone Alpha", Float) = 0.4
        _DarknessAlpha ("Darkness Alpha", Float) = 1.0
        _MarkLayerColor ("Mark Highlight Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float2 _Center;
            float _InnerRadius;
            float _OuterRadius;
            float _OuterAlpha;
            float _DarknessAlpha;
            float4 _MarkLayerColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 screenWorld = i.uv;
                float dist = distance(screenWorld, _Center);

                // Smooth transition between zones
                float innerT = saturate((dist - _InnerRadius) / (_OuterRadius - _InnerRadius));
                float alpha = lerp(0.0, _OuterAlpha, innerT);

                // Outside outer radius → near darkness
                if (dist > _OuterRadius)
                    alpha = _DarknessAlpha;

                return fixed4(0, 0, 0, alpha * col.a);
            }
            ENDCG
        }
    }
}
