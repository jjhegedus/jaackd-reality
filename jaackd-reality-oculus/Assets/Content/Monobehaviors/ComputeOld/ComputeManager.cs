using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using UnityEngine.Experimental.Rendering;
using System.Runtime.InteropServices;


namespace jaackd {

  public class ComputeManager : MonoBehaviour {
    static Vector3 red = new Vector3 { x = 1, y = 0, z = 0 };

    public ComputeShader computeShader;
    public Material material;
    public FilterMode filterMode = FilterMode.Point;
    public GraphicsFormat graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;
    public int width = 1280;
    public int height = 720;
    int kernelNumber = 0; // JJH: May need to look this up in the compute shader as we have more than one kernel

    [SerializeField, HideInInspector] protected RenderTexture displayTexture;


    Vector4[] materials;
    ComputeBuffer materialsComputeBuffer;


    private Bounds computeBounds;

    protected virtual void Start() {
      Init();
    }

    private void Update() {
      Utilities.PrintIfEditor("ComputeManager.cs:Update");


      //material.SetBuffer("materials", materialsComputeBuffer);
      //if (material.HasBuffer("materials")) {
      //  Utilities.PrintIfEditor("Sucessfully added materials buffer");
      //} else {
      //  Utilities.PrintIfEditor("Failed to load materials buffer");
      //}



      //Draw triangles
      Graphics.DrawProcedural(material, computeBounds, MeshTopology.Triangles, 3, 1, Camera.main, null, ShadowCastingMode.On, true);
    }


    void Init() {
      Utilities.PrintIfEditor("ComputeManager.cs:Init");

      // Set the boundary in which data will be rendered
      computeBounds.extents = new Vector3(100, 100, 100);

      // Create render textures
      //ComputeUtilities.CreateRenderTexture(ref displayTexture, width, height, filterMode, graphicsFormat);


      displayTexture = new RenderTexture(width, height, 0);
      displayTexture.graphicsFormat = graphicsFormat;
      displayTexture.enableRandomWrite = true;

      displayTexture.autoGenerateMips = false;
      displayTexture.Create();


      computeShader.SetTexture(kernelNumber, "_MainTex", displayTexture);

      materials = new Vector4[] {
      red
    };

      Utilities.PrintIfEditor("sizeof(float) *4 = " + sizeof(float) * 4);

      materialsComputeBuffer = new ComputeBuffer(1, sizeof(float) * 4, ComputeBufferType.Structured);
      material.SetBuffer("materials", materialsComputeBuffer);
    }

    void OnDestroy() {

      materialsComputeBuffer.Release();
    }

  }

}