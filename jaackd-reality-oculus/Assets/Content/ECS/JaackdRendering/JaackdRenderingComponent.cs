using Unity.Entities;
using Unity.Rendering;
using System;

[Serializable]
public struct JaackdRenderingComponent : IComponentData {
  public RenderBounds renderbounds;
  public WorldRenderBounds worldRenderBounds;
  public bool perInstanceCullingTag;
  public JaackdRenderingComponent(RenderBounds renderBounds, WorldRenderBounds worldRenderBounds, bool perInstanceCullingTag = false) {

    this.renderbounds = renderBounds;
    this.worldRenderBounds = worldRenderBounds;
    this.perInstanceCullingTag = perInstanceCullingTag;
  }
}
