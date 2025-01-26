Shader "Custom/ToonShading"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _RampTex ("Toon Ramp Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1, 1, 1, 1)
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
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _RampTex;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float3 CalculateToonLighting(float3 normal, float3 lightDir)
            {
                float NdotL = max(dot(normal, lightDir), 0.0);
                return tex2D(_RampTex, float2(NdotL, 0.0)).rgb;
            }

            fixed3 CalculateRimLighting(float3 normal, float3 viewDir)
            {
                float rim = 1.0 - saturate(dot(normal, viewDir));
                return pow(rim, 4.0); // Exponent controls rim sharpness
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Fetch textures
                float4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Calculate lighting
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 normal = normalize(i.worldNormal);

                float3 toonLight = CalculateToonLighting(normal, lightDir);
                // float3 rimLight = CalculateRimLighting(normal, viewDir);
                return float4(toonLight * texColor.rgb /*+ rimLight * 0.5*/, texColor.a);
            }
            ENDCG
        }

        // Pass
        // {
        //     Name "Outline"
        //     Tags { "LightMode"="Always" }
        //     Cull Front
        //     ZWrite On
        //     ZTest LEqual
        //     ColorMask 0

        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag

        //     uniform float _OutlineWidth;

        //     struct appdata
        //     {
        //         float4 vertex : POSITION;
        //         float3 normal : NORMAL;
        //     };

        //     struct v2f
        //     {
        //         float4 pos : SV_POSITION;
        //     };

        //     v2f vert(appdata v)
        //     {
        //         v2f o;
        //         float3 normal = UnityObjectToWorldNormal(v.normal);
        //         float4 pos = UnityObjectToClipPos(v.vertex);
        //         pos.xy += normal.xy * _OutlineWidth; Push vertices outward
        //         o.pos = pos;
        //         return o;
        //     }

        //     fixed4 frag(v2f i) : SV_Target
        //     {
        //         return fixed4(0, 0, 0, 1); Black outline
        //     }
        //     ENDCG
        // }
    }
    FallBack "Diffuse"
}
