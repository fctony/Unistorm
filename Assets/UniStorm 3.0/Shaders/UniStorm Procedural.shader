// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UniStorm/Procedural Clouds" {
    Properties {
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _BaseColor ("Ambient Color", Color) = (0,0,0,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MainTex2 ("Base (RGB)", 2D) = "white" {}
        _MainTex3 ("Base (RGB)", 2D) = "white" {}
        _CloudCover ("Cloud Cover", Range(0,1.25)) = 0.5
        _CloudSharpness ("Cloud Sharpness", Range(0.2,0.99)) = 0.8
        _CloudDensity ("Density", Range(0,1)) = 1
        _FogAmount ("Fog Amount", Range(0,1)) = 1
        _CloudSpeed ("Cloud Formation Speed", Vector) = (0.0003, 0, 0, 0)
        _ShadowStrength ("Shadow Strength", Range(0,64)) = 50

        _LoY ("Opaque Y", Float) = 0
        _HiY ("Transparent Y", Float) = 10
        _WorldPosLow ("Opaque Y2", Float) = 0
        _WorldPosHigh ("Transparent Y2", Float) = 10
		_WorldPosGlobal ("Transparent Global", Range(0,2500)) = 0

        _Attenuation ("Attenuation", Range(0,1)) = 1.0
		_Seed ("Seed", float) = 1

		_NoiseScale ("Noise Scale", Range(1, 10)) = 10
    }
    SubShader {
    Tags
        {
            "Queue"="Transparent-100" 
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "LightMode" = "ForwardBase" 
        }

    Pass {
   
    Blend SrcAlpha OneMinusSrcAlpha
    Cull Front 
    ZWrite Off

    CGPROGRAM
   
    #pragma target 3.0
    #pragma vertex vert
    #pragma fragment frag
	#pragma multi_compile_fog
    #include "noiseSimplex.cginc"
    #include "UnityCG.cginc"
    #include "Lighting.cginc"

    sampler2D _MainTex;
    sampler2D _MainTex2;
    sampler2D _MainTex3;
    sampler2D _Jitter;

    float4 _LightColor;
    float4 _BaseColor;
    float4 _CloudSpeed;
    float4 _CloudScale;

    float4 FogColor;
    float _FogAmount;
 
    float _CloudCover;
    float _CloudDensity;
    float _CloudSharpness;
    float _ShadowStrength;

    half _LoY;
    half _HiY;
    half _WorldPosLow;
    half _WorldPosHigh;
	half _WorldPosGlobal;

    uniform float
			_Attenuation,
			_Seed
		;
 
    struct v2f {
        float4 pos : SV_POSITION;
        float2 tex : TEXCOORD0;
        float2 alpha : TEXCOORD1;
        float2 HeightScale : TEXCOORD5;
        float3 normal : NORMAL;
        float4 colNEW : COLOR;
        float4 worldPos : TEXCOORD2;
        float2 viewDirection : TEXCOORD4;
    };
 
 float4 _MainTex_ST;
 float4 _MainTex2_ST;
 float4 _MainTex3_ST;

 float3 normalDirection;
 float3 diffuseReflection;
 float4 Final;
 float3 lightDirection;

  float _NoiseScale;

    v2f vert (appdata_base v) 
    {
        v2f o;
		UNITY_INITIALIZE_OUTPUT(v2f,o);

        normalDirection = normalize( mul( float4(v.normal, 0.0 ), unity_WorldToObject).xyz);

        _Attenuation = clamp(_Attenuation, 0, 1);
        float atten = _Attenuation-(_CloudCover-0.25); 

        lightDirection = normalize(_WorldSpaceLightPos0.xyz);
        diffuseReflection = atten * max(0.0, dot(normalDirection, lightDirection));

        o.colNEW = float4(diffuseReflection, 1.0);

		o.worldPos = mul (unity_ObjectToWorld, v.vertex);
        o.alpha = 1 - saturate((o.worldPos.y - _LoY) / (_HiY - _LoY));
        o.HeightScale = 1 - saturate((o.worldPos.y - _WorldPosLow) / ((_WorldPosHigh-_WorldPosGlobal) - _WorldPosLow)-_CloudCover);

        o.pos = UnityObjectToClipPos(v.vertex);
       	o.tex = v.texcoord.xy;

        return o;
    }


    float4 frag (v2f i) : COLOR
    {
        float2 offset = _Time.xy * _CloudSpeed.xy;
        float4 tex = tex2D( _MainTex, (i.tex.xy) + offset );
        float Density;

        float ns = snoise(i.tex.xy*_NoiseScale+ offset*210+_Seed);
		float4 NoiseCol = float4(ns,ns,ns,ns+_CloudCover);

		float ns2 = snoise(NoiseCol*0.001);
		float4 NoiseCol2 = float4(ns,ns,ns,ns2+_CloudCover);

        half4 col = tex2D(_MainTex,i.tex.xy * _MainTex_ST.xy + offset * 2);
		half4 col2 = tex2D(_MainTex2,i.tex.xy * _MainTex2_ST.xy + offset * 150)*3;
		half4 col3 = tex2D(_MainTex3,i.tex.xy * _MainTex3_ST.xy + offset * 15)*0.5;

		half4 cloudColor = tex2D( _MainTex, (i.tex.xy) + offset * 50);
		tex.a = (col2.a*NoiseCol2.a*1.25) * col3.a * col.a * (NoiseCol.a) * NoiseCol2.a;
		tex.a = i.alpha.y * tex.a;

        tex = max( tex - ( 1 - _CloudCover), 0 );

        float3 EndTracePos = Final;
        float3 TraceDir = i.colNEW;
        float3 CurTracePos =  TraceDir*50;
        TraceDir *= 4.0;

        tex.a = tex.a * i.HeightScale.xy;
        tex.a = 1.0 - pow( _CloudSharpness, tex.a * 128);

        for( int i = 0; i < _ShadowStrength; i++)
        {
            CurTracePos += TraceDir; 
            float4 tex2 = ((col2.a*NoiseCol2.a*1.25) * col3.a * col.a * (NoiseCol.a) * NoiseCol2.a) * 256 * _CloudDensity;
            Density += 0.05 * step( CurTracePos.z, tex2.a);
        }

        float Light = 1 / exp( Density * 2);
        float4 FinalColor = float4 (Light * _LightColor.r, Light * _LightColor.g, Light * _LightColor.b, tex.a);
        FinalColor.xyz += (_BaseColor.xyz*1.25)*cloudColor;
        return FinalColor;
    }
    ENDCG
    }
  }
}
 