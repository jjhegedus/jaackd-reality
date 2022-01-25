using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandTrackingUI : MonoBehaviour
{
  public OVRHand hand;
  public OVRInputModule inputModule;

  public void Start() {
    if (hand != null && hand.PointerPose != null) {
      inputModule.rayTransform = hand.PointerPose;
    }
  }
}
