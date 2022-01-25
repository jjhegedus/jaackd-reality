using Unity.Entities;
using Unity.Transforms;
using System;

[Serializable]
[WriteGroup(typeof(LocalToWorld))]
public struct JaackdTransformComponent : IComponentData {

  public CompositeScale scale;
  public Rotation rotation;
  public Translation translation;
  public LocalToWorld localToWorld;

  public JaackdTransformComponent(Rotation rotation, Translation translation, Scale scale = new Scale(), LocalToWorld localToWorld = new LocalToWorld()) {
    this.scale = new CompositeScale { Value = Unity.Mathematics.float4x4.Scale(scale.Value) };
    this.rotation = rotation;
    this.translation = translation;

    if (scale.Value == 0) {
      this.scale = new CompositeScale { Value = Unity.Mathematics.float4x4.Scale(0) };
    }

    this.localToWorld = localToWorld;

  }

  public JaackdTransformComponent(Rotation rotation, Translation translation, CompositeScale scale = new CompositeScale(), LocalToWorld localToWorld = new LocalToWorld()) {
    this.scale = scale;
    this.rotation = rotation;
    this.translation = translation;

    this.localToWorld = localToWorld;

  }

}
