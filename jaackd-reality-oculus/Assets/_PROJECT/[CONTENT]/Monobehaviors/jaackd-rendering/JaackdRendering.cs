using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Experimental.Rendering;

namespace jaackd {

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

  [DisallowMultipleComponent]
  public class JaackdRendering : MonoBehaviour {
    static Vector3 red = new Vector3 { x = 1, y = 0, z = 0 };

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

    private Bounds computeBounds;

    public FilterMode filterMode = FilterMode.Point;
    public GraphicsFormat graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
    public int width = 1280;
    public int height = 720;



    public int instanceCount = 100000;
    public Mesh instanceMesh;
    public Material instanceMaterial;
    public ComputeShader computeShader;
    public int subMeshIndex = 0;

    private int cachedInstanceCount = -1;
    private int cachedSubMeshIndex = -1;
    private ComputeBuffer positionBuffer;
    private ComputeBuffer materialsBuffer;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };



    void Start() {
      argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
      UpdateBuffers();
      Init();
    }


    void Init() {
      Debug.Log("ComputeManager.cs:Init");

      // Set the boundary in which data will be rendered
      computeBounds.extents = new Vector3(100, 100, 100);

      // Create render textures
      CreateRenderTexture(ref displayTexture, width, height, filterMode, graphicsFormat);
      UpdateMaterialsBuffer();

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

      //materials = new MaterialData[] {
      //  new MaterialData{ materialColor = red }
      //};

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

      //materialsComputeBuffer = new ComputeBuffer(1, sizeof(float) * 4);
      //computeShader.SetInt("numAgents", settings.numAgents);
      //drawAgentsCS.SetBuffer(0, "agents", agentBuffer);
      //drawAgentsCS.SetInt("numAgents", settings.numAgents);


      //computeShader.SetInt("width", width);
      //computeShader.SetInt("height", height);


    }

    void Update() {
      // Update starting position buffer
      if (cachedInstanceCount != instanceCount || cachedSubMeshIndex != subMeshIndex)
        UpdateBuffers();

      // Pad input
      if (UnityEngine.Input.GetAxisRaw("Horizontal") != 0.0f)
        instanceCount = (int)Mathf.Clamp(instanceCount + UnityEngine.Input.GetAxis("Horizontal") * 40000, 1.0f, 5000000.0f);

      // Render
      Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
    }

    void OnGUI() {
      GUI.Label(new Rect(265, 25, 200, 30), "Instance Count: " + instanceCount.ToString());
      instanceCount = (int)GUI.HorizontalSlider(new Rect(25, 20, 200, 30), (float)instanceCount, 1.0f, 5000000.0f);
    }

    void UpdateBuffers() {
      // Ensure submesh index is in range
      if (instanceMesh != null)
        subMeshIndex = Mathf.Clamp(subMeshIndex, 0, instanceMesh.subMeshCount - 1);

      // Positions
      if (positionBuffer != null)
        positionBuffer.Release();
      positionBuffer = new ComputeBuffer(instanceCount, 16);
      Vector4[] positions = new Vector4[instanceCount];
      for (int i = 0; i < instanceCount; i++) {
        float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
        float distance = Random.Range(20.0f, 100.0f);
        float height = Random.Range(-2.0f, 2.0f);
        float size = Random.Range(0.05f, 0.25f);
        positions[i] = new Vector4(Mathf.Sin(angle) * distance, height, Mathf.Cos(angle) * distance, size);
      }
      positionBuffer.SetData(positions);
      instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

      // Indirect args
      if (instanceMesh != null) {
        args[0] = (uint)instanceMesh.GetIndexCount(subMeshIndex);
        args[1] = (uint)instanceCount;
        args[2] = (uint)instanceMesh.GetIndexStart(subMeshIndex);
        args[3] = (uint)instanceMesh.GetBaseVertex(subMeshIndex);
      } else {
        args[0] = args[1] = args[2] = args[3] = 0;
      }
      argsBuffer.SetData(args);

      cachedInstanceCount = instanceCount;
      cachedSubMeshIndex = subMeshIndex;
    }

    void OnDisable() {
      if (positionBuffer != null)
        positionBuffer.Release();
      positionBuffer = null;

      if (argsBuffer != null)
        argsBuffer.Release();
      argsBuffer = null;

      if (materialsBuffer != null)
        materialsBuffer.Release();
      materialsBuffer = null;
    }

    void UpdateMaterialsBuffer() {
      if (materialsBuffer != null)
        materialsBuffer.Release();
      materialsBuffer = new ComputeBuffer(1, (sizeof(float) * 4));
      Vector4[] materials = new Vector4[1];
      materials[0] = new Vector4(1, 1, 0, 1);

      materialsBuffer.SetData(materials);
      instanceMaterial.SetBuffer("materialsBuffer", materialsBuffer);
    }

    void CreateRenderTexture(ref RenderTexture tex, int width, int height, FilterMode filterMode, GraphicsFormat graphicsFormat) {
      tex = new RenderTexture(width, height, 0);
      tex.graphicsFormat = graphicsFormat;
      tex.enableRandomWrite = true;

      tex.autoGenerateMips = false;
      tex.Create();
    }

  }

}