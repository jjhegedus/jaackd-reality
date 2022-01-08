using Unity.Entities;
using System;
using Unity.Transforms;

namespace jaackd {

  [Serializable]
  public struct JaackdRenderingComponent : IComponentData {

    public Scale scale;
    public Rotation rotation;
    public Translation translation;

    public JaackdRenderingComponent(Rotation rotation, Translation translation, Scale scale = new Scale() ) {
      this.scale = scale;
      this.rotation = rotation;
      this.translation = translation;

      if(scale.Value == 0) {
        scale.Value = 1;
      }
    }

  }

}
