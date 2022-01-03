using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using System;
using System.Collections.Generic;

[UpdateInGroup(typeof(GameObjectBeforeConversionGroup))]

public class JaackdEntityConversionSystem : GameObjectConversionSystem {

  HashSet<JaackdEntityAuthoringComponent> jaackdEntityAuthoringComponents = new HashSet<JaackdEntityAuthoringComponent> { };
  HashSet<GameObject> gameObjectsToConvert = new HashSet<GameObject> { };
  bool recurse = false;



  protected override void OnUpdate() {
    Debug.Log("JaackdEntityConversionSystem: OnUpdate");

    GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

    for (int i = 0; i < rootGameObjects.Length; i++) {
      LoadJaackdEntityAuthoringComponents(rootGameObjects[i]);
    }

    foreach (JaackdEntityAuthoringComponent jaackdEntityAuthoringComponent in jaackdEntityAuthoringComponents) {
      LoadGameObjectsFromJaackdEntityAuthoringComponent(jaackdEntityAuthoringComponent);
    }

    foreach(GameObject gameObject in gameObjectsToConvert) {
      ConvertGameObject(gameObject);
    }

  }

  void LoadJaackdEntityAuthoringComponents(GameObject gameObject) {
    foreach (JaackdEntityAuthoringComponent jaackdEntityAuthoringComponent in gameObject.GetComponentsInChildren<JaackdEntityAuthoringComponent>()) {
      jaackdEntityAuthoringComponents.Add(jaackdEntityAuthoringComponent);
    }

  }


  void LoadGameObjectsFromJaackdEntityAuthoringComponent(JaackdEntityAuthoringComponent jaackdEntityAuthoringComponent) {
    Debug.Log("LoadGameObjectsFromJaackdEntityAuthoringComponent(" + jaackdEntityAuthoringComponent.gameObject + ") \n" + GetGameObjectDescription(jaackdEntityAuthoringComponent.gameObject));

    recurse = jaackdEntityAuthoringComponent.recurse;

    LoadGameObjectsToConvertFromGameObject(jaackdEntityAuthoringComponent.gameObject);

  }

  private void LoadGameObjectsToConvertFromGameObject(GameObject gameObject) {
    Debug.Log("LoadGameObjectsToConvertFromGameObject(" + gameObject.name + ")\n" + GetGameObjectDescription(gameObject));
    gameObjectsToConvert.Add(gameObject);

    if (recurse) {
      foreach (Transform child in (Transform)gameObject.GetComponent(typeof(Transform))) {
        LoadGameObjectsToConvertFromGameObject(child.gameObject);
      }
    }

  }

  string GetGameObjectDescription(GameObject gameObject) {
    string messageString = String.Empty;

    Component[] components = gameObject.GetComponents(typeof(Component));
    for (int k = 0; k < components.Length; k++) {
      try {
        messageString += "--component[" + k + "] = " + components[k].GetType().ToString() + Environment.NewLine;
      } catch (System.Exception e) {
        Debug.Log("Error in JaackdEntityConversionsystem : OnUpdate : " + e.Message);
      }
    }

    return messageString;
  }

  private void ConvertGameObject(GameObject gameObject) {
    Debug.Log("Converting gameObject: " + gameObject.name + Environment.NewLine);
  }


}
