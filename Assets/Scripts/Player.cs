using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Config
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private Vector2 _deathKick = new Vector2(25f, 25f);
    
    // State
    private bool _isAlive = true;
    private float _gravityScaleAtStart;

    // Cache
    private Animator _Animator;
    private Rigidbody2D _Rigidbody2D;
    private CapsuleCollider2D _Collider2D;
    private BoxCollider2D _myFeet;
    
    // Start is called before the first frame update
    void Start() {
        _Animator = GetComponent<Animator>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _Collider2D = GetComponent<CapsuleCollider2D>();
        _myFeet = GetComponent<BoxCollider2D>();
        _gravityScaleAtStart = _Rigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update() {

        if (!_isAlive) {
            return;
        }
        
        Run();
        Jump();
        ClimbLadder();
        Die();
    }

    private void Run() {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        ChangeRunningState(horizontalInput);
        FlipSprite(horizontalInput);
        
        Vector2 velocity = new Vector2(horizontalInput * _speed, _Rigidbody2D.velocity.y);

        _Rigidbody2D.velocity = velocity;
    }

    private void ClimbLadder() {
        if (!_myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            _Animator.SetBool("IsClimbing", false);
            _Rigidbody2D.gravityScale = _gravityScaleAtStart;
            return;
        }
        
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 velocity = new Vector2(_Rigidbody2D.velocity.x, verticalInput * _speed);

        _Rigidbody2D.velocity = velocity;
        _Rigidbody2D.gravityScale = 0f;
        _Animator.SetBool("IsClimbing", true);
    }

    private void Jump() {
        if (!_myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }
        
        if (Input.GetButtonDown("Jump")) {
            Vector2 jumpVelocityToAdd = new Vector2(0, _jumpSpeed);

            _Rigidbody2D.velocity += jumpVelocityToAdd;
        }
    }

    private void Die() {
        if (_Collider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Obstacles"))) {
            _isAlive = false;
            _Animator.SetTrigger("Dying");
            _Rigidbody2D.velocity = _deathKick;
        }
    }

    private void FlipSprite(float horizontalInput) {
        if (horizontalInput < 0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (horizontalInput > 0f) {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ChangeRunningState(float horizontalInput) {
        bool isItRunning = Mathf.Abs(horizontalInput) > Mathf.Epsilon;
        
        if (isItRunning) {
            _Animator.SetBool("IsRunning", true);
        } else {
            _Animator.SetBool("IsRunning", false);
        }
    }
}
