﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
      
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "Player") {
      collision.gameObject.GetComponent<Player>().RecountHp(-1);
      collision.gameObject.GetComponent<Rigidbody2D>().
        AddForce(transform.up * 8f, ForceMode2D.Impulse);
      gameObject.GetComponentInParent<Enemy>().startDeath();
    }
  }
  
}
