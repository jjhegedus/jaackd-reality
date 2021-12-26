using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct Gesture {
  public string name;
  public List<Vector3> fingersData;

  public UnityEvent onRecognized;

}

public class GestureDetector : MonoBehaviour {
  public OVRSkeleton skeleton;
  public List<Gesture> gestures;
  public bool debugMode = true;
  public float threshhold = 0.1f;
  public Text resultText;
  private List<OVRBone> fingerBones;
  private Gesture previousGesture;

  // Start is called before the first frame update
  void Start() {
    fingerBones = new List<OVRBone>(skeleton.Bones);
    previousGesture = new Gesture();
  }

  // Update is called once per frame
  void Update() {
    fingerBones = new List<OVRBone>(skeleton.Bones);

    try {
      if (debugMode && Input.GetKeyDown(KeyCode.Space)) {
        Save();
      }
    } catch (System.Exception e) {
      Debug.Log(e.Message);
    }

    Gesture currentGesture = previousGesture;

    if (debugMode && Input.GetKeyDown(KeyCode.KeypadEnter)) {
      currentGesture = Recognize();
    }

    bool hasRecognized = !currentGesture.Equals(new Gesture());

    if (!hasRecognized && !currentGesture.Equals(previousGesture)) {
      Debug.Log("Gesture Found: " + currentGesture.name);
      previousGesture = currentGesture;
      currentGesture.onRecognized.Invoke();
    }
  }

  void Save() {
    Gesture g = new Gesture();
    g.name = "New Gesture";
    List<Vector3> data = new List<Vector3>();
    if (fingerBones.Count == 0) {
      fingerBones = new List<OVRBone>(skeleton.Bones);
    }
    foreach (var bone in fingerBones) {
      data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
    }
    g.fingersData = data;
    gestures.Add(g);
  }

  Gesture Recognize() {
    Gesture currentGesture = new Gesture();
    string resultMessage = "";

    float currentMin = Mathf.Infinity;

    foreach (Gesture g in gestures) {
      float sumDistance = 0;
      bool isDiscarded = false;

      // If the number of fingers does not match, it returns an error
      if (fingerBones.Count != g.fingersData.Count)
        throw new System.Exception("Different number of tracked fingers");

      try {
        float distance = 0;
        for (int i = 0; i < fingerBones.Count; i++) {
          Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
          distance = Vector3.Distance(currentData, g.fingersData[i]);
          if (distance > threshhold) {
            isDiscarded = true;
            break;
          }
          sumDistance += distance;
        }

        if (!isDiscarded && sumDistance < currentMin) {
          currentMin = sumDistance;
          currentGesture = g;
          resultText.text = "Found gesture " + g.name;
          g.onRecognized.Invoke();
        } else {
          resultText.text += "distance:" + distance + "> threshhold:" + threshhold + " \n";
        }
      } catch (System.Exception e) {
        resultText.text += e.Message;
      }
    }

    return currentGesture;
  }

}
