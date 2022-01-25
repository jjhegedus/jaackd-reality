using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using System;
using Unity.Physics;
using Unity.Rendering;


namespace jaackd {

  [UpdateInGroup(typeof(GameObjectAfterConversionGroup))]

  public class JaackdEntityConversionSystem : GameObjectConversionSystem {



    protected override void OnUpdate() {
      Utilities.PrintIfEditor("JaackdEntityConversionSystem: OnUpdate");

      // Iterate over all jaack authoring components
      Entities.ForEach((JaackdEntityAuthoringComponent input) => {
        Utilities.PrintIfEditor("If Editor: Converting entity " + input.name);

        EntityManager dstMgr = DstEntityManager;

        // Get the destination world entity associated with the authoring GameObject
        var jaackdEntity = GetPrimaryEntity(input);

        // Handle transformations
        DstEntityManager.AddComponentData(
          jaackdEntity,
          new JaackdTransformComponent(
            ECSUtilities.RemoveAndReturnComponentIfExists<Rotation>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<Translation>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<Scale>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<LocalToWorld>(jaackdEntity, ref dstMgr)
            )
          );


        // JJH: TODO: Not doing anything with this yet
        LocalToParent localToParent = ECSUtilities.RemoveAndReturnComponentIfExists<LocalToParent>(jaackdEntity, ref dstMgr);

        // Handle Physics
        DstEntityManager.AddComponentData(
          jaackdEntity,
          new JaackdPhysicsComponent(
            ECSUtilities.RemoveAndReturnComponentIfExists<PhysicsCollider>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<PhysicsMass>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<PhysicsVelocity>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<PhysicsDamping>(jaackdEntity, ref dstMgr)
            )
          );

        DstEntityManager.RemoveComponent<PerInstanceCullingTag>(jaackdEntity);

        // Handle render bounds
        DstEntityManager.AddComponentData(
          jaackdEntity,
          new JaackdRenderingComponent(
            ECSUtilities.RemoveAndReturnComponentIfExists<RenderBounds>(jaackdEntity, ref dstMgr),
            ECSUtilities.RemoveAndReturnComponentIfExists<WorldRenderBounds>(jaackdEntity, ref dstMgr),
            true)
          );

        DstEntityManager.AddSharedComponentData<JaackdMeshComponent>(
          jaackdEntity,
          new JaackdMeshComponent(ECSUtilities.RemoveAndReturnSharedComponentIfExists<RenderMesh>(jaackdEntity, DstEntityManager))
          );

      });

    }

    private void ConvertGameObject(GameObject gameObject) {
      Utilities.PrintIfEditor("Converting gameObject: " + gameObject.name + Environment.NewLine);
    }


  }


}