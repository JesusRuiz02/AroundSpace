Shader "Custom/ShaderHorse"
{
    Properties{
       _MainTex("Texture", 2D) = "white"{}
    }
    SubShader{
        CGPROGRAM

        #pragma surface surf Lambert
        float4 _RimColor;
        half _RimPower;
        sampler2D _MainTex;

        struct Input
        {
            float3 viewDir;
            float3 worldPos;
            float2 uv_MainTex;
        };
     void surf(Input IN,inout SurfaceOutput o)
     {
        o.Albedo = tex2D(_MainTex,IN.uv_MainTex);
        half rim = 1 - saturate(dot(IN.viewDir, o.Normal));
        o.Emission =  rim > 0.9  ? float3(0,1,0) :
         rim > 0.8 ? float3(1,1,0) :
         rim > 0.6  ? float3(1,0,1) :
         rim > 0.4  ? float3(0,1,1) :
         rim > 0.1  ? float3(1,0,0) : 0;
     }
    ENDCG
        }
}
