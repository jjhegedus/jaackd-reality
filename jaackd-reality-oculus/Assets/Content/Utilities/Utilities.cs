using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


  }

}