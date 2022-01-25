using Unity.Entities;
using System;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace jaackd {

  public class ECSUtilities {

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



    public static bool ExecuteIfComponentExists<ComponentType>(Entity src, ref Entity dst, EntityManager mgr, Func<Entity, Entity, EntityManager, bool> lambda) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- ExecuteIfComponentExists: Found " + typeof(ComponentType).Name + " Component. Executing the lambda function\n");
        return lambda(src, dst, mgr);
      };

      return false;
    }


    public static bool CopyComponent<ComponentType>(Entity src, ref Entity dst, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- CopyComponent: Found " + typeof(ComponentType).Name + " Component. Adding it to the destination type\n");
        mgr.AddComponent<ComponentType>(dst);
      };

      return mgr.HasComponent<ComponentType>(dst);
    }


    public static ComponentType GetComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- GetComponentIfExists: Found " + typeof(ComponentType).Name + " Component. Returning it.\n");
        return mgr.GetComponentData<ComponentType>(src);
      };

      Utilities.PrintIfEditor("-- GetComponentIfExists: Unable to find " + typeof(ComponentType).Name + " Component. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    public static ComponentType RemoveAndReturnComponentIfExists<ComponentType>(Entity src, ref EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("--  RemoveAndReturnComponentIfExists: Found " + typeof(ComponentType).Name + " Component. Returning it.\n");
        ComponentType component = mgr.GetComponentData<ComponentType>(src);
        mgr.RemoveComponent<ComponentType>(src);
        return component;
      };

      Utilities.PrintIfEditor("-- RemoveAndReturnComponentIfExists: Unable to find " + typeof(ComponentType).Name + " Component. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    public static ComponentType GetSharedComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, ISharedComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- GetSharedComponentIfExists: Found " + typeof(ComponentType).Name + " Shared Component. Returning it.\n");
        return mgr.GetSharedComponentData<ComponentType>(src);
      };

      Utilities.PrintIfEditor("-- GetSharedComponentIfExists: Unable to find " + typeof(ComponentType).Name + " SharedComponent. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    public static ComponentType RemoveAndReturnSharedComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, ISharedComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- GetSharedComponentIfExists: Found " + typeof(ComponentType).Name + " Shared Component. Returning it.\n");
        ComponentType component = mgr.GetSharedComponentData<ComponentType>(src);
        mgr.RemoveComponent<ComponentType>(src);
        return component;
      };

      Utilities.PrintIfEditor("-- GetSharedComponentIfExists: Unable to find " + typeof(ComponentType).Name + " SharedComponent. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


  }


}
