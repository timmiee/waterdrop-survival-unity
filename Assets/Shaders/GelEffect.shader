Shader "Custom/GelEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0,1)) = 0.7
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
        _Wobble ("Wobble Amount", Range(0,1)) = 0.1
        _WobbleSpeed ("Wobble Speed", Float) = 1.0
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
        float _Transparency;
        fixed4 _RimColor;
        float _RimPower;
        float _Wobble;
        float _WobbleSpeed;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldPos;
        };

        void vert(inout appdata_full v)
        {
            // Add wobble/jiggle effect
            float time = _Time.y * _WobbleSpeed;
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            float wobbleX = sin(time + worldPos.y * 5.0) * _Wobble;
            float wobbleY = cos(time + worldPos.x * 5.0) * _Wobble;
            v.vertex.xy += float2(wobbleX, wobbleY) * 0.1;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Base color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            // Rim lighting for gel effect
            float rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow(rim, _RimPower);
            
            // Smooth and translucent
            o.Smoothness = 0.9;
            o.Metallic = 0.1;
            
            // Semi-transparent
            o.Alpha = c.a * _Transparency;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
