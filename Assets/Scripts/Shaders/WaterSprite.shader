Shader "Custom/WaterSprite" {
	Properties {
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Alpha Color Key", Color) = (0,0,0,1)
		_XSpeed("Distortion Speed", Range(0, 10)) = 1
		_XDistiortionIntensity("Distortion Intensity", Range(0, 1)) = 0.01
		_YSpeed("Distortion Speed", Range(0, 10)) = 1
		_YDistiortionIntensity("Distortion Intensity", Range(0, 1)) = 0.01
	}
	SubShader {

		Tags { 
			"RenderType"="Opaque"
			"Queue" = "Transparent+1"
		}

		Pass{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			float _XSpeed;
			float _XDistiortionIntensity;
			float _YSpeed;
			float _YDistiortionIntensity;

			// Generic stuff
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				float x = i.uv.x;
				float y = i.uv.y;

				float2 pos = float2(x, y);

				pos.x = (pos.x + _XDistiortionIntensity * sin(pos.x * 6.283185307179586 + _Time.x * _XSpeed));
				pos.y = (pos.y + _YDistiortionIntensity * sin(pos.y * 6.283185307179586 + _Time.x * _YSpeed));

				return tex2D(_MainTex, pos).rgba;
			}
			
		ENDCG
		}
	}
	FallBack "Diffuse"
}
