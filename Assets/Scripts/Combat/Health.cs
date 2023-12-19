 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour , IDamageable
{
    public static Action<Health> OnDeath;

    public GameObject SplatterPrefab => splatterPrefab;
    public GameObject DeathVFX => deathVFX;

    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;
    private Knockback _knockback;
    private Flash _flash;
    private Health _health;
    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
    }
    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() 
    {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
    public void TakeDamage(Vector2 damageSourceDir, int damageAmount, float knocbackThroust)
    {
        TakeDamage(damageAmount);
        _knockback.GetKnockedBack(damageSourceDir, knocbackThroust);
        //Debug.Log(gameObject.name + " hit direct: " + damageSourceDir);
    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }
}
