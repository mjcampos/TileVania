using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 1f;
    
    private Rigidbody2D _Rigidbody2D;

    // Start is called before the first frame update
    void Start() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 velocity;
        
        if (isFacingRight()) {
            velocity = Vector2.right * _moveSpeed;
        } else {
            velocity = Vector2.left * _moveSpeed;
        }
        
        _Rigidbody2D.velocity = velocity;
    }

    private bool isFacingRight() {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D other) {
        transform.localScale = new Vector3(-(Mathf.Sign(_Rigidbody2D.velocity.x)), 1f);
    }
}
