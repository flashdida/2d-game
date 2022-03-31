using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
  float timer = 0f;
  float timerHit = 0f;
    void Start()
    {
        
    }
    void Update()
    {
    timer += Time.deltaTime;
    if (timer >= 2f) {
      timer = 0;
      transform.localScale = new Vector3(-1f, 1f, 1f);
    } else if (timer >= 1f)
      transform.localScale = new Vector3(1f, 1f, 1f);
    }
  void OnTriggerStay2D(Collider2D collision) {
    if (collision.gameObject.tag == "Player") {
      collision.GetComponent<Player>().inWater = true;
      timerHit += Time.deltaTime;
      if (timerHit >= 2) {
        collision.GetComponent<Player>().RecountHp(-1);
        timerHit = 0;
      }
    }
  }
  void OnTriggerExit2D(Collider2D collider) {
    if (collider.gameObject.tag == "Player") {
      collider.GetComponent<Player>().inWater = false;
      timerHit = 0;
    }
  }
}
