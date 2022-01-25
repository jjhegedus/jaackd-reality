using Unity.Entities;
using Unity.Transforms;
using System;

[Serializable]
[WriteGroup(typeof(LocalToWorld))]
public struct JaackdTransformComponent : IComponentData {

  public Scale scale;
  public Rotation rotation;
  public Translation translation;
  public LocalToWorld localToWorld;

  public JaackdTransformComponent(Rotation rotation, Translation translation, Scale scale = new Scale(), LocalToWorld localToWorld = new LocalToWorld()) {
    this.scale = scale;
    this.rotation = rotation;
    this.translation = translation;

    if (scale.Value == 0) {
      this.scale.Value = 1;
    }

    this.localToWorld = localToWorld;

  }

}
