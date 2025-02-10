Shader "Custom/Player"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", Range(0.1, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldViewDir : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _FresnelPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul((float3x3)unity_WorldToObject, v.normal));

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldViewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fresnel = pow(1.0 - dot(normalize(i.worldNormal), normalize(i.worldViewDir)), _FresnelPower);
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Aplicar o efeito de Fresnel apenas nas bordas
                float outline = smoothstep(0.0, 1.0, fresnel * _FresnelPower);
                fixed4 fresnelColor = _Color * outline;

                return texColor + fresnelColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
