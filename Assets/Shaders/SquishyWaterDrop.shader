Shader "Custom/SquishyWaterDrop"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.8
        _Metallic ("Metallic", Range(0,1)) = 0.2
        _FresnelPower ("Fresnel Power", Range(0,5)) = 2
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,0.5)
        _SquishAmount ("Squish Amount", Range(0,1)) = 0
        _SquishDirection ("Squish Direction", Vector) = (1,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Standard alpha:fade vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        float _FresnelPower;
        fixed4 _FresnelColor;
        float _SquishAmount;
        float4 _SquishDirection;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldPos;
        };

        void vert(inout appdata_full v)
        {
            // Apply squish deformation
            float3 squishDir = normalize(_SquishDirection.xyz);
            float squish = dot(v.vertex.xyz, squishDir) * _SquishAmount;
            v.vertex.xyz += squishDir * squish * 0.2;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            // Metallic and smoothness
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            
            // Fresnel effect for water-like appearance
            float fresnel = pow(1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)), _FresnelPower);
            o.Emission = _FresnelColor.rgb * fresnel;
            
            // Alpha with fresnel
            o.Alpha = c.a * (0.7 + fresnel * 0.3);
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
