using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


namespace jaackd {

  public class Utilities {

    public static void Print(string message) {
      Debug.Log(message);
    }

    public static void PrintIfEditor(string message) {
#if UNITY_EDITOR
      Debug.Log(message);
#endif
    }

    public static void PrintWarningIfEditor(string formatString, params object[] args) {
#if UNITY_EDITOR
      Debug.LogWarningFormat(formatString, args);
#endif
    }


    public static float3 GetScale(float4x4 matrix) { 
      return new float3(
      math.length(matrix.c0.xyz),
      math.length(matrix.c1.xyz),
      math.length(matrix.c2.xyz));
    }


  }

}