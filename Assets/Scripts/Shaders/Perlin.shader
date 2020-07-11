Shader "Perlin/special" {
	Properties
	{
		_CellSize("Cell Size", Range(0, 10)) = 1
		_ScrollSpeed("Scroll Speed", Range(0, 2)) = 1
		_Color("Color", Color) = (0, 0, 0, 1)
		_ColorLine("Line Color", Color) = (0, 0, 0, 1)
		_LineSize("Line Size", Range(0, 1)) = 0.5
	}
		SubShader
	{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			#include "Random.cginc"

			float _CellSize;
			float _ScrollSpeed;

			// Colors
			fixed4 _ColorLine;
			fixed4 _Color;

			fixed _LineSize;

			struct Input {
				float3 worldPos;
			};


			float easeIn(float interpolator) {
				return interpolator * interpolator;
			}

			float easeOut(float interpolator) {
				return 1 - easeIn(1 - interpolator);
			}

			float easeInOut(float interpolator) {
				float easeInValue = easeIn(interpolator);
				float easeOutValue = easeOut(interpolator);
				return lerp(easeInValue, easeOutValue, interpolator);
			}

			float perlinNoise(float3 value) {
				float3 fraction = frac(value);

				float interpolatorX = easeInOut(fraction.x);
				float interpolatorY = easeInOut(fraction.y);
				float interpolatorZ = easeInOut(fraction.z);

				float3 cellNoiseZ[2];
				[unroll]
				for (int z = 0; z <= 1; z++) {
					float3 cellNoiseY[2];
					[unroll]
					for (int y = 0; y <= 1; y++) {
						float3 cellNoiseX[2];
						[unroll]
						for (int x = 0; x <= 1; x++) {
							float3 cell = floor(value) + float3(x, y, z);
							float3 cellDirection = rand3dTo3d(cell) * 2 - 1;
							float3 compareVector = fraction - float3(x, y, z);
							cellNoiseX[x] = dot(cellDirection, compareVector);
						}
						cellNoiseY[y] = lerp(cellNoiseX[0], cellNoiseX[1], interpolatorX);
					}
					cellNoiseZ[z] = lerp(cellNoiseY[0], cellNoiseY[1], interpolatorY);
				}
				float3 noise = lerp(cellNoiseZ[0], cellNoiseZ[1], interpolatorZ);
				return noise;
			}

			void surf(Input i, inout SurfaceOutputStandard o) {
				float3 value = i.worldPos / _CellSize;

				// Scroll effect
				value.y += _Time.y * _ScrollSpeed;

				// Lava effect
				value.z += _Time.y * _ScrollSpeed;

				//get noise and adjust it to be ~0-1 range
				float noise = perlinNoise(value) + 0.5;

				if (_LineSize != 0) {
					noise = frac(noise * 6) * (_LineSize + 0.9);
				}
				else {
					noise = 0;
				}

				float pixelNoiseChange = fwidth(noise / 2);

				float heightLine = smoothstep(1 - pixelNoiseChange, 1, noise);
				heightLine += smoothstep(2 * pixelNoiseChange, 0, noise) * 100;
				
				fixed4 col = heightLine == 0 ? _Color : _ColorLine ;
				
				o.Albedo = col.rgb;
			}
			ENDCG
		}
	FallBack "Standard"
}