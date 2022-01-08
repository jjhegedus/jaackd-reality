using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace jaackd{

  // Add this MonoBehavior to a GameObject and it will be recognized and coverted
  // by the JaackdEntityConversionSystem
  public class JaackdEntityAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity {

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

      Scale scale = new Scale();
      if (dstManager.HasComponent<Scale>(entity)){
        scale = dstManager.GetComponentData<Scale>(entity);
      }
      Rotation rotation = dstManager.GetComponentData<Rotation>(entity);
      Translation translation = dstManager.GetComponentData<Translation>(entity);


      if (dstManager.HasComponent<Scale>(entity)) {
        dstManager.AddComponentData(entity, new JaackdRenderingComponent(rotation, translation, scale));
      } else {
        dstManager.AddComponentData(entity, new JaackdRenderingComponent(rotation, translation));
      }

      dstManager.RemoveComponent<Scale>(entity);
      dstManager.RemoveComponent<Rotation>(entity);
      dstManager.RemoveComponent<Translation>(entity);
      dstManager.RemoveComponent<JaackdEntityAuthoringComponent>(entity);
    }

  }
}
