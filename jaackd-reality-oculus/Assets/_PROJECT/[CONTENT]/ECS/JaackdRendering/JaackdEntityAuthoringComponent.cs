using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Rendering;

namespace jaackd {

  // Add this MonoBehavior to a GameObject and it will be recognized and coverted
  // by the JaackdEntityConversionSystem
  //public class JaackdEntityAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity {
    public class JaackdEntityAuthoringComponent : MonoBehaviour {

      //public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

      //  // Handle transformations
      //  Scale scale = new Scale();
      //  if (dstManager.HasComponent<Scale>(entity)) {
      //    scale = dstManager.GetComponentData<Scale>(entity);
      //  }

      //  Rotation rotation = dstManager.GetComponentData<Rotation>(entity);

      //  Translation translation = dstManager.GetComponentData<Translation>(entity);

      //  LocalToWorld localToWorld = new LocalToWorld();
      //  if (dstManager.HasComponent<LocalToWorld>(entity)) {
      //    localToWorld = dstManager.GetComponentData<LocalToWorld>(entity);
      //  }

      //  dstManager.AddComponentData(entity, new JaackdTransformComponent(rotation, translation, scale, localToWorld));

      //  dstManager.RemoveComponent<Scale>(entity);
      //  dstManager.RemoveComponent<Rotation>(entity);
      //  dstManager.RemoveComponent<Translation>(entity);
      //  dstManager.RemoveComponent<LocalToWorld>(entity);



      //  // Handle Physics
      //  bool physicsCollider = false;
      //  if (dstManager.HasComponent<PhysicsMass>(entity)) {
      //    physicsCollider = true;
      //  }

      //  if (physicsCollider) {

      //    PhysicsMass physicsMass = new PhysicsMass();
      //    if (dstManager.HasComponent<PhysicsMass>(entity)) {
      //      physicsMass = dstManager.GetComponentData<PhysicsMass>(entity);
      //    }

      //    PhysicsVelocity physicsVelocity = new PhysicsVelocity();
      //    if (dstManager.HasComponent<PhysicsVelocity>(entity)) {
      //      physicsVelocity = dstManager.GetComponentData<PhysicsVelocity>(entity);
      //    }

      //    PhysicsDamping physicsDamping = new PhysicsDamping();
      //    if (dstManager.HasComponent<PhysicsDamping>(entity)) {
      //      physicsDamping = dstManager.GetComponentData<PhysicsDamping>(entity);
      //    }

      //    dstManager.AddComponentData(entity, new JaackdPhysicsComponent(physicsCollider, physicsMass, physicsVelocity, physicsDamping));

      //    dstManager.RemoveComponent<PhysicsCollider>(entity);
      //    dstManager.RemoveComponent<PhysicsMass>(entity);
      //    dstManager.RemoveComponent<PhysicsVelocity>(entity);
      //    dstManager.RemoveComponent<PhysicsDamping>(entity);
      //  }


      //  // Handle render bounds
      //  RenderBounds renderBounds = new RenderBounds();
      //  if (dstManager.HasComponent<RenderBounds>(entity)) {
      //    renderBounds = dstManager.GetComponentData<RenderBounds>(entity);
      //  }

      //  if (renderBounds.Value.Size.x > 0 || renderBounds.Value.Size.y > 0 || renderBounds.Value.Size.z > 0) {

      //    WorldRenderBounds worldRenderBounds = new WorldRenderBounds();
      //    if (dstManager.HasComponent<WorldRenderBounds>(entity)) {
      //      worldRenderBounds = dstManager.GetComponentData<WorldRenderBounds>(entity);
      //    }

      //    bool perInstanceCullingTag = false;
      //    if (dstManager.HasComponent<PerInstanceCullingTag>(entity)) {
      //      perInstanceCullingTag = true;
      //    }

      //    dstManager.AddComponentData(entity, new JaackdRenderingComponent(renderBounds, worldRenderBounds, perInstanceCullingTag));

      //    dstManager.RemoveComponent<RenderBounds>(entity);
      //    dstManager.RemoveComponent<WorldRenderBounds>(entity);
      //  }

      //  if (dstManager.HasComponent<RenderMesh>(entity)) {
      //    RenderMesh renderMesh = dstManager.GetSharedComponentData<RenderMesh>(entity);
      //    dstManager.AddSharedComponentData(entity, new JaackdMeshComponent(renderMesh));
      //    dstManager.RemoveComponent<RenderMesh>(entity);
      //  }



      //  // Finally remove the authoring component
      //  dstManager.RemoveComponent<JaackdEntityAuthoringComponent>(entity);
      //}

    }
}
