Shader "Custom/LevelFade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		_Pattern("DissolvePattern", 2D) = "white" {}
		_DissolveLevel("DissolveLevel", Float) = 0.0
		_Cutout("Circle Cutout", Float) = 0.0
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Off
		CGPROGRAM
#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
			float2 uv_Pattern;
			float3 worldPos;
			float3 viewDir;
			float4 screenPos;
		};
		sampler2D _MainTex;
		sampler2D _Pattern;
		float _DissolveLevel;
		float _Cutout;
		float _Radius;
		float _Speed;

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 pattern = tex2D(_Pattern, IN.uv_Pattern);
			clip(frac((IN.screenPos.y) * _DissolveLevel) - _Cutout);
			
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
