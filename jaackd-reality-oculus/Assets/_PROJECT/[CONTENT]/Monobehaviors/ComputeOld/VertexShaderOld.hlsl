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

			// make fog work
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"

			static const float4	red = float4(1, 0, 0, 1);
			static const float4 green = float4(0, 1, 0, 1);
			static const float4 blue = float4(0, 0, 1, 1);
			static const float4 black = float4(0, 0, 0, 1);
			static const float4 white = float4(1, 1, 1, 1);
			static const float4 yellow = float4(1, 1, 0, 1);



			//struct ModelVertex {
			//	float3 position;
			//	float pad;
			//};
			//StructuredBuffer<ModelVertex>      modelVertices : register(t0);


			//struct PerVertexData {
			//	unsigned int instanceId;
			//	unsigned int index;
			//	unsigned int eye;
			//	unsigned int materialId;
			//};
			//StructuredBuffer<PerVertexData>      perVertexData : register(t3);


			//struct PerInstanceData {
			//	float4x4 world;
			//};
			//StructuredBuffer<PerInstanceData>      perInstanceData : register(t2);


			//// A constant buffer that stores each set of view and projection matrices in column-major format.
			//cbuffer ViewProjectionConstantBuffer : register(b1)
			//{
			//	float4x4 viewProjection[2];
			//};


			struct Material {
				float4 materialColor;
			};
			RWStructuredBuffer<Material>      materials;


			//struct DirectionalLight {
			//	float3 color;
			//	float  intensity;
			//	float3 direction;
			//};
			//StructuredBuffer<DirectionalLight>  directionalLights : register(t4);



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




			//struct appdata
			//{
			//	float4 vertex : POSITION;
			//	float2 uv : TEXCOORD0;
			//};

			//struct v2f
			//{
			//	float2 uv : TEXCOORD0;
			//	UNITY_FOG_COORDS(1)
			//	float4 vertex : SV_POSITION;
			//};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			//v2f vert(appdata v)
			//{
			//	v2f o;
			//	o.vertex = UnityObjectToClipPos(v.vertex);
			//	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			//	UNITY_TRANSFER_FOG(o,o.vertex);
			//	return o;
			//}

			VertexShaderOutput vertexMain(VertexShaderInput input)
			{
				VertexShaderOutput output = (VertexShaderOutput)0;

				//PerVertexData vertexData = perVertexData[input.vid];
				//PerInstanceData instanceData = perInstanceData[vertexData.instanceId];

				//float4 pos = float4(modelVertices[vertexData.index].position, 1);

				//// Transform the vertex position into world space.
				//pos = mul(pos, transpose(instanceData.world));

				//// Correct for perspective and project the vertex position onto the screen.
				//pos = mul(pos, viewProjection[vertexData.eye]);
				//output.pos = (min16float4)pos;

				// Set the output color based on the material color
				//output.color = float4(materials[vertexData.materialId], 1.0);
				//output.color = float4(materials[0].materialColor, 1.0);

				//output.viewId = vertexData.eye;


				//// Lighting
				//uint numLights = 0;
				//uint stride = 0;

				//directionalLights.GetDimensions(numLights, stride);

				//uint lightNumber = 0;
				//for (lightNumber = 0; lightNumber < numLights) {

				//}

				//if (numLights >= 1) {
				//  if (directionalLights[0].color.y == 0.0) {
				//    output.color = float4(1.0, 0.0, 0.0, 1.0);
				//  }
				//}


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

				materials.GetDimensions(numStructs, stride);

				Material material = materials[0];
				float3 materialCol = material.materialColor;

				[branch] switch (stride)
				{
				case 0:
					output.color = black;
					break;
				case 1:
					output.color = red;
					break;
				case 2:
					output.color = green;
					break;
				case 3:
					output.color = blue;
					break;
				case 4:
					output.color = yellow;
					break;
				default:
					output.color = white;
					break;
				}

				//if (input.vid == 0) {

				//	switch (materialCol.x)
				//	{
				//	case 0:
				//		output.color = black;
				//		break;
				//	case 1:
				//		output.color = red;
				//		break;
				//	case 2:
				//		output.color = green;
				//		break;
				//	case 3:
				//		output.color = blue;
				//		break;
				//	default:
				//		output.color = white;
				//		break;
				//	};

				//}
				//else if (input.vid == 1) {

				//	switch (materialCol.y)
				//	{
				//	case 0:
				//		output.color = black;
				//		break;
				//	case 1:
				//		output.color = red;
				//		break;
				//	case 2:
				//		output.color = green;
				//		break;
				//	case 3:
				//		output.color = blue;
				//		break;
				//	default:
				//		output.color = white;
				//		break;
				//	};
				//}
				//else if (input.vid == 2) {

				//	switch (materialCol.z)
				//	{
				//	case 0:
				//		output.color = black;
				//		break;
				//	case 1:
				//		output.color = red;
				//		break;
				//	case 2:
				//		output.color = green;
				//		break;
				//	case 3:
				//		output.color = blue;
				//		break;
				//	default:
				//		output.color = white;
				//		break;
				//	};
				//}
				//else {
				//	output.color = black;
				//}

				//output.color = float3(1, 0, 0);
				//output.color = white;

				//if (input.vid == 0) {
				//	output.color = float3(0, 0, 1);
				//}
				//else if (input.vid == 1) {
				//	output.color = float3(0, 1, 0);
				//}
				//else if (input.vid == 2) {
				//	output.color = float3(1, 0, 0);
				//}
				//else {
				//	output.color = float3(1, 1, 0);
				//}

				// End Testing

				// Set the render target array index.
				//output.viewId = idx;

				return output;
			}


			//	fixed4 frag(v2f i) : SV_Target
			//	{
			//		// sample the texture
			//		fixed4 col = tex2D(_MainTex, i.uv);
			//	// apply fog
			//	UNITY_APPLY_FOG(i.fogCoord, col);
			//	return col;
			//}




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
				//min16float4 p = input.pos;
				//min16float3 pw = input.posW;
				//min16float3 pc = input.color;
				//min16float3 pn = input.normal;
				//min16float2 pt = input.tex;

				////return ((input.posW.x * input.posW.x) + (input.posW.y * input.posW.y)) < (input.size.x * input.size.x) ? min16float4(input.color, 1.0f) : min16float4(0.f, 0.f, 0.f, 0.f);
				return min16float4(input.color, 1.0f);
			//return min16float4(1.0f, 0.0f, 0.0f, 1.0f);
			}

			ENDCG
		}
	}
}
