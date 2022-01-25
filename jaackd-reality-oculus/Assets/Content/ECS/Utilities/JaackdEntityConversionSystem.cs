using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using System;
using Unity.Physics;
using Unity.Rendering;


namespace jaackd {

  [UpdateInGroup(typeof(GameObjectAfterConversionGroup))]

  public class JaackdEntityConversionSystem : GameObjectConversionSystem {


    EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;


    static bool ExecuteIfComponentExists<ComponentType>(Entity src, ref Entity dst, EntityManager mgr, Func<Entity, Entity, EntityManager, bool> lambda) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Executing the lambda function\n");
        return lambda(src, dst, mgr);
      };

      return false;
    }


    static bool CopyComponent<ComponentType>(Entity src, ref Entity dst, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Adding it to the destination type\n");
        mgr.AddComponent<ComponentType>(dst);
      };

      return mgr.HasComponent<ComponentType>(dst);
    }


    static ComponentType GetComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Returning it.\n");
        return mgr.GetComponentData<ComponentType>(src);
      };

      Utilities.PrintIfEditor("-- Unable to find " + typeof(ComponentType).Name + " Component. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    static ComponentType GetSharedComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, ISharedComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Shared Component. Returning it.\n");
        return mgr.GetSharedComponentData<ComponentType>(src);
      };

      Utilities.PrintIfEditor("-- Unable to find " + typeof(ComponentType).Name + " SharedComponent. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    protected override void OnCreate() {
      UnityEngine.Debug.Log("JaackdEntityConversionsystem:OnCreate ");
      endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
      base.OnCreate();
    }


    protected override void OnUpdate() {
      Utilities.PrintIfEditor("JaackdEntityConversionSystem: OnUpdate");
      EntityCommandBuffer ecb = endSimCommandBufferSystem.CreateCommandBuffer();

      // Iterate over all jaack authoring components
      Entities.ForEach((JaackdEntityAuthoringComponent input) => {
        UnityEngine.Debug.Log("Converting and entity");

        // Get the destination world entity associated with the authoring GameObject
        var primaryEntity = GetPrimaryEntity(input);

        // Create a new entity to hold the Jaackd components
        var jaackdEntity = CreateAdditionalEntity(input);

        ECSUtilities.CopyNameIfEditor(primaryEntity, jaackdEntity, DstEntityManager);
        Utilities.PrintIfEditor("\n\nEntity name = " + ECSUtilities.GetNameIfEditor(jaackdEntity, DstEntityManager) + "\n");


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

    private void ConvertGameObject(GameObject gameObject) {
      Utilities.PrintIfEditor("Converting gameObject: " + gameObject.name + Environment.NewLine);
    }


  }


}