using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool CanMove => _canMove;

    [SerializeField] private float moveSpeed = 10f;

    private float _moveX;
    private bool _canMove = true;

    private Rigidbody2D _rigidbody;
    private Knockback _knockback;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void UnSubscribeEvents()
    {
        _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }
    private void Move()
    {
        if (!_canMove) { return ;}

        Vector2 movement = new Vector2(_moveX * moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = movement;
    }
    private void CanMoveTrue()
    {
        _canMove = true;
    }
    private void CanMoveFalse()
    {
        _canMove = false;
    }
}
