using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TMP_Text _scoreText;
    private int _currentScore = 0;
    private Enemy _enemy;
    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        Health.OnDeath += OnEnemyDeath;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        Health.OnDeath -= OnEnemyDeath;
    }
    private void OnEnemyDeath(Health health)
    {
        _enemy = health.gameObject.GetComponent<Enemy>();
        if (_enemy)
        {
            _currentScore++;
            _scoreText.text = _currentScore.ToString("D3");
            _enemy = null;
        }
    }
}
