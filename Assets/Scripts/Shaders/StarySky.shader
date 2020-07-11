Shader "Custom/StarySky" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BoardX ("BoardX", int) = 10
		_BoardY ("BoardY", int) = 10
		_Radius ("Radius", float) = 1.0
		_Frequency ("Frequency", float) = 1.0
		_P ("Dropout", range(0,1)) = 0.5
	}
	SubShader {
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "Random.cginc"

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

				// Variables
				float4 _Color;
				sampler2D _MainTex;
				int _BoardX;
				int _BoardY;
				float _Radius;
				float _Frequency;
				float _P;
				static const float PI = 3.14159265359;

				// Colors
				static const float4 darkRed = float4(0.5, 0, 0, 1);
				static const float4 darkBlue = float4(0, 0, 0.5, 1);
				static const float4 darkPurple = float4(0.1, 0, 0.3, 1);
				static const float4 pink = float4(0.58, 0.06, 0.32, 1);
				static const float4 yellow = float4(1, 1, 0, 1);
				static const float4 mwhite = float4(1, 1, 1, 1);

				static int mBoardX[100];
				static int mBoardY[100];
				
				float outline(float2 uv, float pct)
				{
					return smoothstep(pct - 0.005, pct, uv.y) -
				     	   smoothstep(pct, pct + 0.005, uv.y);
				}

				float4 circle(float2 uv, float4 col)
				{
					float p = abs(sin(_Frequency*PI*_Time.x));
					float t = 0.8 + 0.4 * p;

					_Radius = mul(_Radius, t);

					for (float x; x < _BoardX; x ++)
					{
						for (float y; y < _BoardY; y ++)
						{

							if (mBoardX[int(x)] == 0 && mBoardX[int(y)] == 0) 
							{
								continue;
							}

							float d = sqrt(abs(distance(uv.x, x/_BoardX + 0.05)) +
									   abs(distance(uv.y, y/_BoardY + 0.05)));

							if (d + 0.002 > _Radius && d - 0.002 < _Radius)
							{
								col = lerp(mwhite, yellow, p);
							}
							else if (d < _Radius)
							{
								col = lerp(darkPurple, pink, p);
							}
						}
					}
					return col;
				}

				float4 eye(float2 uv)
				{
					float4 col;
					float y = 1.*(pow(sin(2.*PI*uv.y + _Time.x), 2.) + pow(cos(2.*PI*uv.x), 2.));

					float pct = outline(uv, y);

					if (uv.y > y)
					{
						col = darkRed * abs(sin(_Frequency*PI*_Time.x));
					}

					return col;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					if (mBoardX[99] == 0) 
					{
						for (int i = 0; i < _BoardX; i++)
						{
							mBoardX[i] = rand1dTo1d(i) < _P ? 1 : 0;
							mBoardY[i] = rand1dTo1d(i) < _P ? 1 : 0;
						}
						mBoardX[99] = 1;
					}

					// Normalized pixel coordinates (from 0 to 1)
					float2 uv = i.uv;
					float4 col;

					//col = eye(uv);
					col = circle(uv, col);

					return col;
				}
			ENDCG
		}
	}
}
