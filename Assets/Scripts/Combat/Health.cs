 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public static Action<Health> OnDeath;

    public GameObject SplatterPrefab => splatterPrefab;
    public GameObject DeathVFX => deathVFX;

    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;
    
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
    
}
