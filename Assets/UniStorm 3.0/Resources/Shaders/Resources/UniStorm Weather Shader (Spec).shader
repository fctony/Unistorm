// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "UniStorm/Global Weather Shaders/Global Weather Shader (Spec)" {
    Properties {
		_MainTexColor ("Base Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MainBump ("MainBump", 2D) = "bump" {}
		_Smoothness ("Smoothness", Range (0, 1) ) = 0.25
		_Occlusion("Occlusion", 2D)= "white" {}
		_MetallicTex("Metallic", 2D)= "white" {}
		_MetallicSmoothness ("Metallic Smoothness", Range (0, 1) ) = 1.0
		_SnowLayerColor ("Snow Layer Color", Color) = (1.0,1.0,1.0,1.0)
        _SnowLayerTex ("Snow Layer Tex (RGB)", 2D) = "white" {}
        _SnowLayerBump ("Snow Layer Bump", 2D) ="bump" {}
        //_SnowStrength ("Snow Strength", Range(0, 0.75)) = 0
        _SnowDirection ("Snow Direction", Vector) = (0, 1, 0)
		_SnowSmoothness ("Snow Smoothness", Range (0, 1) ) = 0.25
		_WetnessMultiplier ("Wetness Multiplier", Range (0, 1) ) = 0.5
		//_WetnessStrength ("Wetness Strength", Range (0, 0.75) ) = 0.0
		_DetailBump ("Detail Bump (RGB)", 2D) = "white" {}
		_DetailIntensity ("Detail Intensity", Range (0, 1) ) = 0.0
    }

	CGINCLUDE
	#define _GLOSSYENV 1
	#define UNITY_SETUP_BRDF_INPUT MetallicSetup
	ENDCG

    SubShader {
        Tags { "RenderType"="Opaque"  "Queue" = "Geometry" }
       
        CGPROGRAM
        #pragma target 4.0
		#include "UnityPBSLighting.cginc"
        #pragma surface surf Standard 
 
		fixed4 _MainTexColor;
        sampler2D _MainTex;
        sampler2D _MainBump;
		sampler2D _Occlusion;
		sampler2D _DetailBump;
		sampler2D _MetallicTex;
		fixed4 _SnowLayerColor;
        sampler2D _SnowLayerTex;
        sampler2D _SnowLayerBump;
        uniform float _SnowStrength;
		uniform float _WetnessStrength;
        float3 _SnowDirection;
		float _SnowSmoothness;
		float _Smoothness;
		half _MetallicSmoothness;
		float _Scale;
		float _DetailIntensity;
		float _WetnessMultiplier;
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_MainBump;
            float2 uv_SnowLayerTex;
            float2 uv_SnowLayerBump;
			float2 uv_Occlusion;
			float2 uv_DetailBump;
            float3 worldNormal;
			float4 screenPos;
			float3 worldPos;
			half4 color : COLOR;
            INTERNAL_DATA
        };
       

 
        void surf (Input IN, inout SurfaceOutputStandard o) 
		{
            half4 MainDiffuse = tex2D(_MainTex, IN.uv_MainTex) * _MainTexColor;
            half4 SnowDiffuse = tex2D(_SnowLayerTex, IN.uv_SnowLayerTex) * _SnowLayerColor;
			half3 SnowNormal = UnpackNormal(tex2D(_SnowLayerBump, IN.uv_SnowLayerBump));
			half3 Occlusion = tex2D(_Occlusion, IN.uv_Occlusion);
			half3 DetailNormal = UnpackNormal(tex2D(_DetailBump, IN.uv_DetailBump));
			float AdjustedWetness = 0;
			o.Normal = UnpackNormal(tex2D(_MainBump, IN.uv_MainBump));

            half SnowMask = dot(WorldNormalVector(IN, o.Normal + half3(SnowNormal.r*0.5, SnowNormal.g*0.5, 0)), _SnowDirection);
			SnowMask = saturate(pow(SnowMask, 1));
			half WetnessMask = dot(WorldNormalVector(IN, o.Normal), half3(0, 0.5, 0));	
			WetnessMask = pow(0.5 * WetnessMask + 0.5, 2.0);

			half SnowBlend = (SnowMask * 15 * _SnowStrength) * IN.color;
			SnowBlend = saturate(pow(SnowBlend,8));
			half3 ColorBlend = lerp(MainDiffuse, SnowDiffuse, SnowBlend);

			half3 NormalBlend = lerp(o.Normal, SnowNormal+o.Normal*0.4, SnowBlend);
			o.Normal = NormalBlend + (DetailNormal * half3(_DetailIntensity,_DetailIntensity,0));
			o.Albedo = ColorBlend;

            if (SnowMask > lerp(1.0, 0, _SnowStrength))
            {
				o.Smoothness = lerp(o.Smoothness, _SnowSmoothness, SnowBlend);
            }
            else
            {
				if (WetnessMask > lerp(1.0, 0, 0.8))
				{
					AdjustedWetness = _WetnessStrength *(_WetnessMultiplier) * IN.color;
				}

				float4 metal = tex2D(_MetallicTex, IN.uv_MainTex);
				o.Metallic = metal.rgb;
				o.Smoothness = saturate(metal.a * _MetallicSmoothness + AdjustedWetness);
				o.Smoothness = clamp(o.Smoothness, 0, 0.8);
            }
  
			o.Occlusion = Occlusion.rgb;
            o.Alpha = MainDiffuse.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}