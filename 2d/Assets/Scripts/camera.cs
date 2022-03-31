using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {
  // Start is called before the first frame update
  float speed = 3f;
  public Transform target;
  void Start() {
    transform.position = new Vector3(target.transform.position.x,
      target.transform.position.y, transform.position.z);
  }

  // Update is called once per frame
  void Update() {
    Vector3 position = target.position;
    position.z = transform.position.z;
    transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
  }
}
