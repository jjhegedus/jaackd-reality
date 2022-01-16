using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Rendering;
using UnityEngine;
using System;

namespace jaackd {

  [UpdateInGroup(typeof(GameObjectAfterConversionGroup))]
  public class JaackdRenderingConversionSystem : GameObjectConversionSystem {

    EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;

    static bool ExecuteIfComponentExists<ComponentType>(Entity src, ref Entity dst, EntityManager mgr, Func<Entity, Entity, EntityManager, bool> lambda) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        ECSUtilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Executing the lambda function\n");
        return lambda(src, dst, mgr);
      };

      return false;
    }

    static bool CopyComponent<ComponentType>(Entity src, ref Entity dst, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        ECSUtilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Adding it to the destination type\n");
        mgr.AddComponent<ComponentType>(dst);
      };

      return mgr.HasComponent<ComponentType>(dst);
    }

    static ComponentType GetComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        ECSUtilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Returning it.\n");
        return mgr.GetComponentData<ComponentType>(src);
      };

      ECSUtilities.PrintIfEditor("-- Unable to find " + typeof(ComponentType).Name + " Component. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }
    static ComponentType GetSharedComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, ISharedComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        ECSUtilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Shared Component. Returning it.\n");
        return mgr.GetSharedComponentData<ComponentType>(src);
      };

      ECSUtilities.PrintIfEditor("-- Unable to find " + typeof(ComponentType).Name + " SharedComponent. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }

    protected override void OnCreate() {
      endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
      base.OnCreate();
    }

    protected override void OnUpdate() {
      ECSUtilities.PrintIfEditor("****** OnUpdate fired");
      EntityCommandBuffer ecb = endSimCommandBufferSystem.CreateCommandBuffer();

      // Iterate over all authoring components of type FooAuthoring
      Entities.ForEach((JaackdEntityAuthoringComponent input) => {

        // Get the destination world entity associated with the authoring GameObject
        var primaryEntity = GetPrimaryEntity(input);

        // Create a new entity to hold the Jaackd components
        var jaackdEntity = CreateAdditionalEntity(input);

        ECSUtilities.CopyNameIfEditor(primaryEntity, jaackdEntity, DstEntityManager);
        ECSUtilities.PrintIfEditor("\n\nEntity name = " + ECSUtilities.GetNameIfEditor(jaackdEntity, DstEntityManager) + "\n");

        // Handle transformations
        DstEntityManager.AddComponentData(
          jaackdEntity,
          new JaackdTransformComponent(
            GetComponentIfExists<Rotation>(primaryEntity, DstEntityManager),
            GetComponentIfExists<Translation>(primaryEntity, DstEntityManager),
            GetComponentIfExists<Scale>(primaryEntity, DstEntityManager),
            GetComponentIfExists<LocalToWorld>(primaryEntity, DstEntityManager)
            )
          );

        // JJH: TODO: Not doing anything with this yet
        LocalToParent localToParent = GetComponentIfExists<LocalToParent>(primaryEntity, DstEntityManager);

        // Handle Physics
        DstEntityManager.AddComponentData(
          jaackdEntity,
          new JaackdPhysicsComponent(
            true,
            GetComponentIfExists<PhysicsMass>(primaryEntity, DstEntityManager),
            GetComponentIfExists<PhysicsVelocity>(primaryEntity, DstEntityManager),
            GetComponentIfExists<PhysicsDamping>(primaryEntity, DstEntityManager)
            )
          );

        // Handle render bounds
        DstEntityManager.AddComponentData(
          jaackdEntity,
          new JaackdRenderingComponent(
            GetComponentIfExists<RenderBounds>(primaryEntity, DstEntityManager),
            GetComponentIfExists<WorldRenderBounds>(primaryEntity, DstEntityManager),
            true)
          );

        // Handle the render mesh
        DstEntityManager.AddSharedComponentData<JaackdMeshComponent>(
          jaackdEntity,
          new JaackdMeshComponent(GetSharedComponentIfExists<RenderMesh>(primaryEntity, DstEntityManager))
          );

        //// Finally disable the primary entity
        //ECSUtilities.PrintIfEditor("--disable the primary entity");
        //DstEntityManager.SetEnabled(srcEntity, false);
        //DstEntityManager.AddComponent<Disabled>(srcEntity);
        //ECSUtilities.PrintIfEditor("--after disabling the primary entity");

        //// Finally destroy the primary entity
        //ECSUtilities.PrintIfEditor("-- destroying " + ECSUtilities.GetNameIfEditor(srcEntity, DstEntityManager));
        //endSimCommandBufferSystem.EntityManager.DestroyEntity(srcEntity);
        //ecb.DestroyEntity(primaryEntity);
        DstEntityManager.AddComponent<JaackdDeleteEntityTag>(primaryEntity);
      });

    }

  }

}