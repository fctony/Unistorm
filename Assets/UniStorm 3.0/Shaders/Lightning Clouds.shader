 Shader "UniStorm/Lightning Clouds" {
     Properties {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         _Color ("Base Color", Color) = (1.0,1.0,1.0,1.0)
		 _LoY ("Opaque Y", Float) = 0
		 _HiY ("Transparent Y", Float) = 10
     }
     SubShader {
         Tags{
             "Queue"="Transparent"
             "IgnoreProjector"="True"
             "RenderType"="Transparent"
         }
         
         Lighting On Cull Front ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
         
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Standard nofog vertex:myvert alpha:fade 
		 #pragma target 3.0
 
         sampler2D _MainTex;
         fixed4 _Color;
		 half _LoY;
		 half _HiY;
 
         struct Input {
             float2 uv_MainTex;
			 float3 worldPos;
			 half alpha;
         };

		void myvert (inout appdata_full v, out Input data) 
		{ 
		  UNITY_INITIALIZE_OUTPUT(Input,data);
          float4 worldV = mul (unity_ObjectToWorld, v.vertex);
          data.alpha = 1 - saturate((worldV.y - _LoY) / (_HiY - _LoY));
		}
 
        void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float2 offset = _Time.xy * 0.005;
			half4 c = tex2D (_MainTex, IN.uv_MainTex+offset);
			o.Albedo = UNITY_LIGHTMODEL_AMBIENT*2;
			o.Alpha = IN.alpha*c.a*_Color.a;
        }
         ENDCG
     } 
 }