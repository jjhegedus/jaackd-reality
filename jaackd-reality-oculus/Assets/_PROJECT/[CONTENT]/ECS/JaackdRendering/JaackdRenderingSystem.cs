using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace jaackd {

  public class JaackdRenderingSystem : SystemBase {

    //private World currentWorld;
    //private EntityManager entityManager;
    //private EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;

    //protected override void OnCreate() {
    //  currentWorld = World.DefaultGameObjectInjectionWorld;
    //  entityManager = currentWorld.EntityManager;
    //  endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    //  base.OnCreate();
    //}

    protected override void OnUpdate() {

      //EntityCommandBuffer ecb = endSimCommandBufferSystem.CreateCommandBuffer();

      //  Entities
      //.ForEach(
      //    (ref LocalToWorld localToWorld, in JaackdTransformComponent jaackTransformComponent) => {
      //      //localToWorld.Value = ... // Assign localToWorld as needed for UserTransform
      //      Debug.Log("Ran JaackdRenderingSystem on something");
      //                }).ScheduleParallel();

      Entities
        .WithAny<JaackdDeleteEntityTag>()
        .ForEach(
          (in Entity entity) => {
            //ECSUtilities.PrintIfEditor("--Found component JaackdDeleteEntityComponent on Entity " + ECSUtilities.GetNameIfEditor(entity, entityManager) + "\n");
            //ecb.DestroyEntity(entity);
            ECSUtilities.PrintIfEditor("JaackdRenderingSystem::OnUpdate\n");
          }).WithoutBurst().Run();

    }

  }

}
