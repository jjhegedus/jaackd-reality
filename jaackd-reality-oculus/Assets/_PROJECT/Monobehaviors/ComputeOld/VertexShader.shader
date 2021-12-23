Shader "JJH/VertexShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vertexMain
			#pragma fragment pixelMain
			#pragma target 5.0
			#pragma enable_d3d11_debug_symbols

			// make fog work
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"

			static const float4	red = float4(1, 0, 0, 1);
			static const float4 green = float4(0, 1, 0, 1);
			static const float4 blue = float4(0, 0, 1, 1);
			static const float4 black = float4(0, 0, 0, 1);
			static const float4 white = float4(1, 1, 1, 1);
			static const float4 yellow = float4(1, 1, 0, 1);

			StructuredBuffer<float4>      materials;

			// Per-vertex data used as input to the vertex shader.
			struct VertexShaderInput
			{
				uint        vid     : SV_VertexID;
			};

			// Per-vertex data passed to the geometry shader.
			// Note that the render target array index will be set by the geometry shader
			// using the value of viewId.
			struct VertexShaderOutput
			{
				min16float4 pos     : SV_POSITION;
				min16float3 color   : COLOR0;
				uint        viewId  : TEXCOORD0;  // SV_InstanceID % 2
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			VertexShaderOutput vertexMain(VertexShaderInput input)
			{
				VertexShaderOutput output = (VertexShaderOutput)0;



				//Testing

				if (input.vid == 0) {
					output.pos = float4(-0.5f, -0.5f, 0.0, 1.0f);
				}
				else if (input.vid == 1) {
					output.pos = float4(0.5f, -0.5f, 0.0f, 1.0f);
				}
				else if (input.vid == 2) {
					output.pos = float4(0.0f, 0.5f, 0.0f, 1.0f);
				}

				uint numStructs = 0;
				uint stride = 0;

				//materials.GetDimensions(numStructs, stride);

				//Material material = materials[0];
				//float4 materialCol = materials[0];

				//switch (materialCol.w)
				//{
				//case 0:
				//	output.color = black;
				//	break;
				//case 1:
				//	output.color = red;
				//	break;
				//case 2:
				//	output.color = green;
				//	break;
				//case 3:
				//	output.color = blue;
				//	break;
				//case 4:
				//	output.color = yellow;
				//	break;
				//default:
				//	output.color = white;
				//	break;
				//}

				//output.color = materials[0];



#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				output.color = red;
#else
				output.color = green;
#endif

				return output;
			}



			// Per-pixel color data passed through the pixel shader.
			struct PixelShaderInput
			{
				min16float4 pos     : SV_POSITION;
				min16float3 color   : COLOR0;
			};

			// The pixel shader passes through the color data. The color data from 
			// is interpolated and assigned to a pixel at the rasterization step.
			min16float4 pixelMain(PixelShaderInput input) : SV_TARGET
			{
				return min16float4(input.color, 1.0f);
			}

			ENDCG
		}
	}
}
