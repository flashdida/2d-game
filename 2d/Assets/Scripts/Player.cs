﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  Rigidbody2D _rb;
  public float speed;
  public float jumpHeight;
  public Transform groundCheck;
  bool isGrounded;
  Animator anim;
  int curHp;
  int maxHp = 3;
  bool isHit = false;
  public Main main;
  public bool key = false;
  bool canTP = true;
  public bool inWater = false;
  bool isClimbing = false;
  // Start is called before the first frame update

  void Start() {
    _rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    curHp = maxHp;
  }
  // Update is called once per frame
  void Update() {
    if (inWater&&!isClimbing) {
      anim.SetInteger("State", 4);
      isGrounded = true;
      if (Input.GetAxis("Horizontal") != 0) {
        Flip();
      }
    } else {
      CheckGround();
      if (Input.GetAxis("Horizontal") == 0 && (isGrounded)&&!isClimbing) {
        anim.SetInteger("State", 1);
      } else {
        Flip();
        if (isGrounded&&!isClimbing)
          anim.SetInteger("State", 2);
      }
    }

  }
  void FixedUpdate() {
    _rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, _rb.velocity.y);
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
      _rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
    }
  }
  void Flip() {
    if (Input.GetAxis("Horizontal") > 0)
      transform.localRotation = Quaternion.Euler(0, 0, 0);
    if (Input.GetAxis("Horizontal") < 0) {
      transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
  }
  void CheckGround() {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
    isGrounded = colliders.Length > 1;
    if (!isGrounded&&!isClimbing)
      anim.SetInteger("State", 3);
  }
  public void RecountHp(int deltaHp) {
    curHp += deltaHp;
    if (deltaHp < 0) {
      StopCoroutine(OnHit());
      isHit = true;
      StartCoroutine(OnHit());
    }
    if (curHp <= 0) {
      GetComponent<CapsuleCollider2D>().enabled = false;
      Invoke("Lose", 1.5f);
    }
  }
  IEnumerator OnHit() {
    if (isHit)
      GetComponent<SpriteRenderer>().color = new Color(1f,
        GetComponent<SpriteRenderer>().color.g - 0.04f,
        GetComponent<SpriteRenderer>().color.b - 0.04f);
    else
      GetComponent<SpriteRenderer>().color = new Color(1f,
          GetComponent<SpriteRenderer>().color.g + 0.04f,
          GetComponent<SpriteRenderer>().color.b + 0.04f);
    if (GetComponent<SpriteRenderer>().color.g == 1)
      StopCoroutine(OnHit());
    if (GetComponent<SpriteRenderer>().color.g <= 0) {
      isHit = false;
    }
    yield return new WaitForSeconds(0.02f);
    StartCoroutine(OnHit());
  }
  void Lose() {
    main.GetComponent<Main>().Lose();
  }
  IEnumerator TPwait() {
    yield return new WaitForSeconds(1f);
    canTP = true;
  }
  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.tag == "Key") {
      Destroy(collision.gameObject);
      key = true;
    }
    if (collision.gameObject.tag == "Door") {
      if (collision.gameObject.GetComponent<Door>().isOpen && canTP) {
        collision.gameObject.GetComponent<Door>().Teleport(gameObject);
        canTP = false;
        StartCoroutine(TPwait());
      } else if (key)
        collision.gameObject.GetComponent<Door>().Unlock();
    }
  }
  private void OnTriggerStay2D(Collider2D collision) {
    if (collision.gameObject.tag == "ladder") {
      isClimbing = true;
      _rb.bodyType = RigidbodyType2D.Kinematic;
      if (Input.GetAxis("Vertical") == 0)
        anim.SetInteger("State", 5);
      else {
        anim.SetInteger("State", 6);
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
      }
    }
  }
  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.gameObject.tag == "ladder") {
      isClimbing = false;
      _rb.bodyType = RigidbodyType2D.Dynamic;
    }
  }
  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "Trampoline") {
      StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
    } 
  }
  IEnumerator TrampolineAnim(Animator an) {
    an.SetBool("isJump", true);
    yield return new WaitForSeconds(0.5f);
    an.SetBool("isJump", false);
  }

}
