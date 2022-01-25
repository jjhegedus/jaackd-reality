using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


namespace jaackd {

  public class JaackdRenderingSystem : SystemBase {

    protected override void OnUpdate() {

      //Entities
      //  .WithAny<JaackdDeleteEntityTag>()
      //  .ForEach(
      //    (in Entity entity) => {
      //      //ECSUtilities.PrintIfEditor("--Found component JaackdDeleteEntityComponent on Entity " + ECSUtilities.GetNameIfEditor(entity, entityManager) + "\n");
      //      //ecb.DestroyEntity(entity);
      //      Utilities.PrintIfEditor("JaackdRenderingSystem::OnUpdate\n");
      //      UnityEngine.Debug.Log("jaackd:JaackdRenderingSystem:OnUpdate:entiyFound");
      //    }).Schedule();

    }
  }


}