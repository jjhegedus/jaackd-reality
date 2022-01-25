using UnityEngine;
using System;
using jaackd;

namespace jaackd {

  public class MonobehaviorUtilities {

    // Gets an array of the root game objects
    public static GameObject[] GetRootGameObjects() {
      return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
    }

    // Returns the description of a game object
    public static string GetGameObjectDescription(GameObject gameObject) {
      string messageString = String.Empty;

      Component[] components = gameObject.GetComponents(typeof(Component));
      for (int k = 0; k < components.Length; k++) {
        try {
          messageString += "--component[" + k + "] = " + components[k].GetType().ToString() + Environment.NewLine;
        } catch (System.Exception e) {
          Utilities.PrintIfEditor("Error in JaackdEntityConversionsystem : OnUpdate : " + e.Message);
        }
      }

      return messageString;
    }

  }

}