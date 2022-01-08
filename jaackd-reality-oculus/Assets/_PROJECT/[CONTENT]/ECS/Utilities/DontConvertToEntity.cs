using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

// Add this component to a gameObject and it will not be converted to an entity
// because the entity system will run this code, which does nothing, instead of the default conversion code.
public class DontConvertToEntity : MonoBehaviour, IConvertGameObjectToEntity {
  public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
    Debug.Log("DontConvertToEntity:Convert doesn't do anything. This stops the object from being converted: " + name + "\n");
  }
}
