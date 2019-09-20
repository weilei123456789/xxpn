Shader "Custom/Additive" {
	Properties{
		_MainTex("MainTexture", 2D) = "white" {}
	}
	SubShader{
		Tags { "QUEUE" = "Transparent" "IGNOREPROJECTOR" = "true" "RenderType" = "Transparent" }

		Pass {
			ZWrite Off
			Blend SrcAlpha One
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;

			struct myV2F {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			myV2F vert(appdata_base v) {
				myV2F v2f;
				v2f.pos = UnityObjectToClipPos(v.vertex);
				v2f.uv = v.texcoord;
				return v2f;
			}


			fixed4 frag(myV2F v2f) : COLOR {
				fixed4 c = tex2D(_MainTex, v2f.uv);
				return c;
			}

			ENDCG
		}
	}
}