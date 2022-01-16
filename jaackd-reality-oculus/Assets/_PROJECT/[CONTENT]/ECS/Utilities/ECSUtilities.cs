using Unity.Entities;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace jaackd {

  public class ECSUtilities {

    public static void PrintIfEditor(string message) {
#if UNITY_EDITOR
      Debug.Log(message);
#endif
    }

    public static void SetNameIfEditor(Entity dstEntity, EntityManager mgr, string name) {
#if UNITY_EDITOR
      mgr.SetName(dstEntity, name);
#endif
    }

    public static string GetNameIfEditor(Entity dstEntity, EntityManager mgr) {
#if UNITY_EDITOR
      return mgr.GetName(dstEntity);
#else
      return "";
#endif
    }

    public static void CopyNameIfEditor(Entity srcEntity, Entity dstEntity, EntityManager mgr) {
#if UNITY_EDITOR
      mgr.SetName(dstEntity, mgr.GetName(srcEntity));
#endif
    }

  }


}
