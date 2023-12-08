using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput => _frameInput.Move;
    public static PlayerController Instance;
    public static Action OnJump;
    public static Action action1;

    #region Serialized Fields
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private Transform feetTransform;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float extraGravity = 700f;
    [SerializeField] private float gravityDelay = .2f;
    [SerializeField] private float coyoteTime = .5f;
    #endregion

    #region Privates
    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private bool _isGrounded = false;
    private bool _doubleJumpAvaible;
    private float _timeInAir;
    private float _coyoteTimer;
    private Rigidbody2D _rigidBody;
    private Movement _movement;
    #endregion

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        OnJump += ApplyJumpForce;
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
    private void UnsubscribeEvents()
    {
        OnJump -= ApplyJumpForce;
    }
    private void Update()
    {
        GatherInput();
        Movement();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
    }

    private void FixedUpdate()
    {
        ExtraGravity();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = false;
        }
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }
    public bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(feetTransform.position, groundCheck, 0f, groundLayer);
        return isGrounded;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetTransform.position, groundCheck);
    }
    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            _timeInAir += Time.deltaTime;
        }
        else
        {
            _timeInAir = 0f;
        }
    }
    private void ExtraGravity()
    {
        if(! (_timeInAir > gravityDelay))
        {
            return;
        }
        else
        {
            _rigidBody.AddForce(new Vector2(0f, -extraGravity * Time.deltaTime));
        }
    }
    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement() {

        _movement.SetCurrentDirection(_frameInput.Move.x);
    }

    private void HandleJump()
    {
        if (!_frameInput.Jump)
            return;

        if (CheckGrounded())
        {
            OnJump?.Invoke();
        }
        else if( _coyoteTimer > 0)
        {
            OnJump?.Invoke();
        }
        else if (_doubleJumpAvaible)
        {
            _doubleJumpAvaible = false;           
            OnJump?.Invoke();
        }
    }
    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = coyoteTime;
            _doubleJumpAvaible = true;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }
    private void ApplyJumpForce()
    {
        _timeInAir = 0f;
        _coyoteTimer = 0f;
        //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0f);
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }
    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 
}
