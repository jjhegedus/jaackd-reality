using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using System;
using System.Collections.Generic;


namespace jaackd {

  [UpdateInGroup(typeof(GameObjectAfterConversionGroup))]

  public class JaackdEntityConversionSystem : GameObjectConversionSystem {


    EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;


    static bool ExecuteIfComponentExists<ComponentType>(Entity src, ref Entity dst, EntityManager mgr, Func<Entity, Entity, EntityManager, bool> lambda) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Executing the lambda function\n");
        return lambda(src, dst, mgr);
      };

      return false;
    }


    static bool CopyComponent<ComponentType>(Entity src, ref Entity dst, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Adding it to the destination type\n");
        mgr.AddComponent<ComponentType>(dst);
      };

      return mgr.HasComponent<ComponentType>(dst);
    }


    static ComponentType GetComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, IComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Component. Returning it.\n");
        return mgr.GetComponentData<ComponentType>(src);
      };

      Utilities.PrintIfEditor("-- Unable to find " + typeof(ComponentType).Name + " Component. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    static ComponentType GetSharedComponentIfExists<ComponentType>(Entity src, EntityManager mgr) where ComponentType : struct, ISharedComponentData {
      if (mgr.HasComponent<ComponentType>(src)) {
        Utilities.PrintIfEditor("-- Found " + typeof(ComponentType).Name + " Shared Component. Returning it.\n");
        return mgr.GetSharedComponentData<ComponentType>(src);
      };

      Utilities.PrintIfEditor("-- Unable to find " + typeof(ComponentType).Name + " SharedComponent. Returning an empty constucted component of " + typeof(ComponentType).Name + " type .\n");
      return new ComponentType { };
    }


    protected override void OnCreate() {
      endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
      base.OnCreate();
    }


    protected override void OnUpdate() {
      Utilities.PrintIfEditor("JaackdEntityConversionSystem: OnUpdate");


    } 

    private void ConvertGameObject(GameObject gameObject) {
      Utilities.PrintIfEditor("Converting gameObject: " + gameObject.name + Environment.NewLine);
    }


  }


}