using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  public List<GameObject> prefabs;

  public void Spawn(int index) {
    Instantiate(prefabs[index], new Vector3(Random.Range(0, 5), Random.Range(0, 2), Random.Range(1, 5)), Random.rotation);
  }

}
