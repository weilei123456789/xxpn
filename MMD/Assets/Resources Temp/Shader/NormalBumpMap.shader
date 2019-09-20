Shader "Custom/NormalBumpMap"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white"{}
		_BumpMap("Bump Map", 2D) = "bump"{}
		_BumpScale("Bump Scale", Range(0.1, 30.0)) = 10.0
	}

	SubShader{
		Pass{
			Tags{ "RenderType" = "Opaque" }

			CGPROGRAM
			#include "Lighting.cginc"  

			#pragma vertex vert  
			#pragma fragment frag    

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float _BumpScale;

			struct v2f{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 lightDir : TEXCOORD1;
			};

			v2f vert(appdata_tan v){
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				TANGENT_SPACE_ROTATION;
				o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Color.xyz;
				float3 tangentNormal = UnpackNormal(tex2D(_BumpMap, i.uv));
				float3 tangentLight = normalize(i.lightDir);
				fixed3 lambert = 0.5 * dot(tangentNormal, tangentLight) + 0.5;
				fixed3 diffuse = lambert * _Color.xyz * _LightColor0.xyz + ambient;
				fixed4 color = tex2D(_MainTex, i.uv);
				return fixed4(diffuse * color.rgb, 1.0);
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
	