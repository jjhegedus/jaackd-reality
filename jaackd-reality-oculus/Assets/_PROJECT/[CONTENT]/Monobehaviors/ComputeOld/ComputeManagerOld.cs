using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using UnityEngine.Experimental.Rendering;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct ModelVertex {
  public Vector3 position;
  public float pad;
}

[StructLayout(LayoutKind.Sequential)]
public struct VertexData {
  public int instanceId;
  public int index;
  public int eye;
  public int materialId;
}

[StructLayout(LayoutKind.Sequential)]
public struct InstanceData {
  public Matrix4x4 worldMatrix;
}

[StructLayout(LayoutKind.Sequential)]
public struct MaterialData {
  public Vector4 materialColor;
}

[StructLayout(LayoutKind.Sequential)]
public struct ViewProjectionData {
  public Matrix4x4[] viewProjection;
}

public class ComputeManagerOld : MonoBehaviour {
  static Vector3 red = new Vector3 { x = 1, y = 0, z = 0 };

  public ComputeShader computeShader;
  public Material material;
  public FilterMode filterMode = FilterMode.Point;
  public GraphicsFormat graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
  public int width = 1280;
  public int height = 720;
  int kernelNumber = 0; // JJH: May need to look this up in the compute shader as we have more than one kernel

  [SerializeField, HideInInspector] protected RenderTexture displayTexture;

  ModelVertex[] modelVertices;
  ComputeBuffer modelVerticesComputeBuffer;

  MaterialData[] materials;
  ComputeBuffer materialsComputeBuffer;

  VertexData[] perVertexData;
  ComputeBuffer perVertexComputeBuffer;

  InstanceData[] perInstanceData;
  ComputeBuffer perInstanceComputeBuffer;

  ViewProjectionData viewProjectionData;
  ComputeBuffer viewProjectionComputeBuffer;




  Texture2D colourMapTexture;

  private Bounds computeBounds;

  protected virtual void Start() {
    Init();
    //transform.GetComponentInChildren<MeshRenderer>().material.mainTexture = displayTexture;
  }

  private void Update() {
    Debug.Log("ComputeManager.cs:Update");

    ////Assign Buffers to material
    //material.SetBuffer("perVertexData", perVertexComputeBuffer);
    //if (material.HasBuffer("perVertexData")) {
    //  Debug.Log("Sucessfully added perVertexData buffer");
    //} else {
    //  Debug.Log("Failed to load perVertexData buffer");
    //}

    //material.SetBuffer("modelVertices", modelVerticesComputeBuffer);
    //if (material.HasBuffer("modelVertices")) {
    //  Debug.Log("Sucessfully added modelVertices buffer");
    //} else {
    //  Debug.Log("Failed to load modelVertices buffer");
    //}

    //material.SetBuffer("perInstanceData", perInstanceComputeBuffer);
    //if (material.HasBuffer("perInstanceData")) {
    //  Debug.Log("Sucessfully added perInstanceData buffer");
    //} else {
    //  Debug.Log("Failed to load perInstanceData buffer");
    //}

    material.SetBuffer("materials", materialsComputeBuffer);
    if (material.HasBuffer("materials")) {
      Debug.Log("Sucessfully added perInstanceData buffer");
    } else {
      Debug.Log("Failed to load perInstanceData buffer");
    }

    //material.SetBuffer("ViewProjectionConstantBuffer", perInstanceComputeBuffer);
    //if (material.HasBuffer("ViewProjectionConstantBuffer")) {
    //  Debug.Log("Sucessfully added ViewProjectionConstantBuffer");
    //} else {
    //  Debug.Log("Failed to load ViewProjectionConstantBuffer");
    //}

    //Draw triangles
    Graphics.DrawProcedural(material, computeBounds, MeshTopology.Triangles, 3, 1, Camera.main, null, ShadowCastingMode.On, true);
  }


  void Init() {
    Debug.Log("ComputeManager.cs:Init");

    // Set the boundary in which data will be rendered
    computeBounds.extents = new Vector3(100, 100, 100);

    // Create render textures
    CreateRenderTexture(ref displayTexture, width, height, filterMode, graphicsFormat);
    computeShader.SetTexture(kernelNumber, "ColourMap", displayTexture);

    //modelVertices = new ModelVertex[] {
    //    new ModelVertex{ position = new Vector3(-.1f, -.1f, .1f)}, // 0 bottom front left
    //    new ModelVertex{ position = new Vector3(-.1f, 0.1f, .1f)}, // 1 top front left
    //    new ModelVertex{ position = new Vector3(0.1f, -.1f, .1f)}, // 2 bottom front right
    //    new ModelVertex{ position = new Vector3(0.1f, 0.1f, .1f)}, // 3 top front right
    //    new ModelVertex{ position = new Vector3(-.1f, -.1f, -.1f)}, // 4 bottom back left
    //    new ModelVertex{ position = new Vector3(-.1f, 0.1f, -.1f)}, // 5 top back left
    //    new ModelVertex{ position = new Vector3(0.1f, -.1f, -.1f)}, // 6 bottom back right
    //    new ModelVertex{ position = new Vector3(0.1f, 0.1f, -.1f)}, // 7 top back right
    //};

    //VertexData vd = new VertexData { instanceId = 0, index = 0, eye = 0, materialId = 0 };


    //perVertexData = new VertexData[] {
    //    // Left eye
    //    new VertexData { instanceId = 0, index = 0, eye = 0, materialId = 0}, // 0 bottom front left
    //    new VertexData { instanceId = 0, index = 1, eye = 0, materialId = 0}, // 1 top front left
    //    new VertexData { instanceId = 0, index = 2, eye = 0, materialId = 0}, // 2 bottom front right
    //    new VertexData { instanceId = 0, index = 3, eye = 0, materialId = 0}, // 3 top front right
    //    new VertexData { instanceId = 0, index = 4, eye = 0, materialId = 0}, // 4 bottom back left
    //    new VertexData { instanceId = 0, index = 5, eye = 0, materialId = 0}, // 5 top back left
    //    new VertexData { instanceId = 0, index = 6, eye = 0, materialId = 0}, // 6 bottom back right
    //    new VertexData { instanceId = 0, index = 7, eye = 0, materialId = 0}, // 7 top back right
        
    //    // Right eye
    //    new VertexData { instanceId = 0, index = 0, eye = 0, materialId = 0}, // 0 bottom front left
    //    new VertexData { instanceId = 0, index = 1, eye = 0, materialId = 0}, // 1 top front left
    //    new VertexData { instanceId = 0, index = 2, eye = 0, materialId = 0}, // 2 bottom front right
    //    new VertexData { instanceId = 0, index = 3, eye = 0, materialId = 0}, // 3 top front right
    //    new VertexData { instanceId = 0, index = 4, eye = 0, materialId = 0}, // 4 bottom back left
    //    new VertexData { instanceId = 0, index = 5, eye = 0, materialId = 0}, // 5 top back left
    //    new VertexData { instanceId = 0, index = 6, eye = 0, materialId = 0}, // 6 bottom back right
    //    new VertexData { instanceId = 0, index = 7, eye = 0, materialId = 0}, // 7 top back right
    //};

    //float scale = Random.Range(1, 100) / 10;
    //perInstanceData = new InstanceData[] {
    //  new InstanceData { worldMatrix = Matrix4x4.identity * Matrix4x4.Scale(new Vector3{ x = scale, y = scale, z = scale}) * Matrix4x4.Rotate(Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f))) * Matrix4x4.Translate(new Vector3{x = Random.Range(0.0f, 20.0f), y = Random.Range(0.0f, 20.0f), z = Random.Range(0.0f, 20.0f) }) }
    //};

    materials = new MaterialData[] {
      new MaterialData{ materialColor = red }
    };

    //viewProjectionData.viewProjection = new Matrix4x4[] {
    //Camera.main.previousViewProjectionMatrix,
    //Camera.main.previousViewProjectionMatrix
    //};


    //std::vector < unsigned short> indices1({
    //  0,1,2, // Front
    //   1,3,2, // Front
    //   5,1,4, // Left
    //   1,0,4, // Left
    //   1,5,7, // Top
    //   1,7,3, // Top
    //   0,2,6, // Bottom
    //   0,6,4, // Bottom
    //   2,3,7, // Right
    //   2,7,6, // Right
    //   6,7,5, // Back
    //   6,5,4, // Back
    //});

    //ComputeUtilities.CreateAndSetBuffer<int>(ref modelVerticesComputeBuffer, modelVertices.Length, computeShader, "modelVertices", kernelNumber);
    //ComputeUtilities.CreateAndSetBuffer<int>(ref perVertexComputeBuffer, perVertexData.Length, computeShader, "perVertexData", kernelNumber);
    //ComputeUtilities.CreateAndSetBuffer<int>(ref perInstanceComputeBuffer, perInstanceData.Length, computeShader, "perInstanceData", kernelNumber);
    //ComputeUtilities.CreateAndSetBuffer<int>(ref materialsComputeBuffer, materials.Length, computeShader, "materials", kernelNumber);
    //ComputeUtilities.CreateAndSetBuffer<int>(ref viewProjectionComputeBuffer, viewProjectionData.viewProjection.Length, computeShader, "ViewProjectionConstantBuffer", kernelNumber);

    Debug.Log("sizeof(float) *4 = " + sizeof(float) * 4);

    materialsComputeBuffer = new ComputeBuffer(1, sizeof(float) *4);
    //computeShader.SetInt("numAgents", settings.numAgents);
    //drawAgentsCS.SetBuffer(0, "agents", agentBuffer);
    //drawAgentsCS.SetInt("numAgents", settings.numAgents);


    //computeShader.SetInt("width", width);
    //computeShader.SetInt("height", height);


  }

  void CreateRenderTexture(ref RenderTexture tex, int width, int height, FilterMode filterMode, GraphicsFormat graphicsFormat) {
    tex = new RenderTexture(width, height, 0);
    tex.graphicsFormat = graphicsFormat;
    tex.enableRandomWrite = true;

    tex.autoGenerateMips = false;
    tex.Create();
  }

  //void FixedUpdate() {
  //  for (int i = 0; i < settings.stepsPerFrame; i++) {
  //    RunSimulation();
  //  }
  //}

  //void LateUpdate() {
  //  if (showAgentsOnly) {
  //    ComputeUtilities.ClearRenderTexture(displayTexture);

  //    drawAgentsCS.SetTexture(0, "TargetTexture", displayTexture);
  //    ComputeUtilities.Dispatch(drawAgentsCS, settings.numAgents, 1, 1, 0);

  //  } else {
  //    ComputeUtilities.Dispatch(computeShader, width, height, 1, kernelIndex: colourKernel);
  //    //	ComputeUtilities.CopyRenderTexture(trailMap, displayTexture);
  //  }
  //}

  //void RunSimulation() {
  //  var speciesSettings = settings.speciesSettings;
  //  ComputeUtilities.CreateStructuredBuffer(ref settingsBuffer, speciesSettings);
  //  computeShader.SetBuffer(updateKernel, "speciesSettings", settingsBuffer);
  //  computeShader.SetBuffer(colourKernel, "speciesSettings", settingsBuffer);

  //  // Assign settings
  //  computeShader.SetFloat("deltaTime", Time.fixedDeltaTime);
  //  computeShader.SetFloat("time", Time.fixedTime);

  //  computeShader.SetFloat("trailWeight", settings.trailWeight);
  //  computeShader.SetFloat("decayRate", settings.decayRate);
  //  computeShader.SetFloat("diffuseRate", settings.diffuseRate);
  //  computeShader.SetInt("numSpecies", speciesSettings.Length);


  //  ComputeUtilities.Dispatch(computeShader, settings.numAgents, 1, 1, kernelIndex: updateKernel);
  //  ComputeUtilities.Dispatch(computeShader, width, height, 1, kernelIndex: diffuseMapKernel);

  //  ComputeUtilities.CopyRenderTexture(diffusedTrailMap, trailMap);
  //}

  void OnDestroy() {
    perVertexComputeBuffer.Release();
    modelVerticesComputeBuffer.Release();
    perInstanceComputeBuffer.Release();
    materialsComputeBuffer.Release();
    viewProjectionComputeBuffer.Release();
  }

}