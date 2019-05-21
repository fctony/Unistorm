// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UniStorm/Stars" {
Properties {
	_Color ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_StarTex1 ("Star Texture 1", 2D) = "white" {}
	_StarTex2 ("Star Texture 2", 2D) = "white" {}
	_StarSpeed ("Rotation Speed", Float) = 2.0
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	_LoY ("Opaque Y", Float) = 0
    _HiY ("Transparent Y", Float) = 10
}

Category {
	Tags { "Queue"="Transparent-1000" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	ColorMask RGB
	Cull Front 
	Lighting Off 
	ZWrite Off

	SubShader 
	{
	
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _StarTex1;
			sampler2D _StarTex2;
			fixed4 _Color;
			half _LoY;
      		half _HiY;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
				#endif
			};

			float2 rotateUV(float2 uv, float degrees)
            {
               const float Rads = (UNITY_PI * 2.0) / 360.0;
 
               float ConvertedRadians = degrees * Rads;
               float _sin = sin(ConvertedRadians);
               float _cos = cos(ConvertedRadians);
 
                float2x2 R_Matrix = float2x2( _cos, -_sin, _sin, _cos);
 
                uv -= 0.5;
                uv = mul(R_Matrix, uv);
                uv += 0.5;
 
                return uv;
            }
			
			float4 _StarTex1_ST;
			float4 _StarTex2_ST;
			float _StarSpeed;
			float _Rotation;

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;

				_Rotation = _Time.x*_StarSpeed*10;

				o.texcoord1.xy = TRANSFORM_TEX(rotateUV(v.texcoord, _Rotation), _StarTex1);
				o.texcoord1.zw = TRANSFORM_TEX(rotateUV(v.texcoord, _Rotation), _StarTex2);

				float4 worldV = mul (unity_ObjectToWorld, v.vertex);
		        o.color.a = 1 - saturate((worldV.y - _LoY) / (_HiY - _LoY)); 

				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : SV_Target
			{				
				fixed4 col = 1.0f * i.color * _Color * (tex2D(_StarTex1, i.texcoord1.xy) + tex2D(_StarTex2, i.texcoord1.zw));
				return col;
			}
			ENDCG 
			}
		}	
	}
}
