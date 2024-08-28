Shader "Custom/ShadowShader"
{
    Properties
    {
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _Offset ("Offset", Range(-1, 1)) = -0.1
    }
    SubShader
    {
        Tags {"Queue"="Overlay"}
        LOD 100

        Pass
        {
            Lighting Off
            Cull Off
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _ShadowColor;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                // Aplicar um pequeno deslocamento no eixo Y para simular sombra
                v.vertex.y += _Offset;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _ShadowColor; 
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}
