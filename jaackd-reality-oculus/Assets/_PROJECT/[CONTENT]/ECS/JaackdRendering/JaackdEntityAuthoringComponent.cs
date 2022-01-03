using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

// Add this MonoBehavior to a GameObject and it will be recognized and coverted
// by the JaackdEntityConversionSystem
public class JaackdEntityAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity {

  public bool recurse = true;
  public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
    Debug.Log("JaackdEntityAuthoringComponent:Convert");
  }
}
