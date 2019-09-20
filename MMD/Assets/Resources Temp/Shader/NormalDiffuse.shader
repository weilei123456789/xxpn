Shader "Custom/NormalDiffuse" 
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white"{}
		_Normal("Normal Texture", 2D) = "bump"{}
		[HDR]_RimColor("Rim Color", Color) = (0.5,0.5,0.5,1)
		_RimPower("Rim Power", Range(0.5,8.0)) = 8.0
	}

	SubShader
	{
		Tags{"RenderType" = "Opaque"}
		LOD 300

		CGPROGRAM
		#pragma surface surf Lambert

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _Normal;
		float4 _RimColor;
		float _RimPower;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_Normal;
			float3 viewDir;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) ;
			o.Albedo = tex * _Color;
			o.Alpha = tex.a;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));

			if (_RimPower < 7.9)
			{
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			}
		}
		ENDCG
	
	}
	Fallback "Diffuse"
}
