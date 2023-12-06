using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    private Rigidbody2D _rigidbody;
    [SerializeField] private float _knockBackTime = .2f;

    private Vector3 _hitDirection;
    private float _knockBackThrust;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        OnKnockbackStart += ApplyKnockbackForce;
        OnKnockbackEnd += StopKnockRoutine;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void UnSubscribeEvents()
    {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKnockRoutine;
    }
    public void GetKnockedBack(Vector3 hitDirection, float knockBackThrust)
    {
        _hitDirection = hitDirection;
        _knockBackThrust = knockBackThrust;

        OnKnockbackStart?.Invoke();
    }
    private void ApplyKnockbackForce()
    {
        Vector3 difference = (transform.position - _hitDirection).normalized * _knockBackThrust * _rigidbody.mass;
        _rigidbody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }
    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockBackTime);
        OnKnockbackEnd?.Invoke();
    }
    private void StopKnockRoutine()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
