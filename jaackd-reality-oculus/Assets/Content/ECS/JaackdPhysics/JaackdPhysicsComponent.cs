using Unity.Entities;
using System;
using Unity.Transforms;
using Unity.Physics;

namespace jaackd {

  [Serializable]
  public struct JaackdPhysicsComponent : IComponentData {


    public PhysicsCollider physicsCollider;
    public PhysicsMass physicsMass;
    public PhysicsVelocity physicsVelocity;
    public PhysicsDamping physicsDamping;

    public JaackdPhysicsComponent(PhysicsCollider physicsCollider = new PhysicsCollider(), PhysicsMass physicsMass = new PhysicsMass(), PhysicsVelocity physicsVelocity = new PhysicsVelocity(), PhysicsDamping physicsDamping = new PhysicsDamping()) {

      this.physicsCollider = physicsCollider;
      this.physicsMass = physicsMass;
      this.physicsVelocity = physicsVelocity;
      this.physicsDamping = physicsDamping;
    }

  }

}