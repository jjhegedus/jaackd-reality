using Unity.Entities;
using System;
using Unity.Rendering;

[Serializable]
public struct JaackdMeshComponent : ISharedComponentData, IEquatable<JaackdMeshComponent> {

  public RenderMesh renderMesh;

  public JaackdMeshComponent(RenderMesh renderMesh) {
    this.renderMesh = renderMesh;
  }

  public bool Equals(JaackdMeshComponent other) {
    return Equals((object)other);
  }

  public override bool Equals(object obj) {
    return base.Equals(obj);
  }

  public override int GetHashCode() {
    return base.GetHashCode();
  }

  public override string ToString() {
    return base.ToString();
  }
}
