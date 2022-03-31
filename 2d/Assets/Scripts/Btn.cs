using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject[] block;
  public Sprite btnDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "MarkBox") {
      GetComponent<SpriteRenderer>().sprite = btnDown;
      GetComponent<CircleCollider2D>().enabled = false;
      foreach(var item in block) {
        Destroy(item);
      }
    }
  }
}
